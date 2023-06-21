using ChurchServices.Extensions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;

namespace ChurchServices.Data.Import.EIB.Model.Bible {
    public class LogosBibleModelService : IDisposable {
        const string BibleType = "[[@Bible:";
        int FootnoteId = 1;
        WordprocessingDocument CurrentDocument { get; set; }
        MainDocumentPart mainPart => CurrentDocument != null ? CurrentDocument.MainDocumentPart : null;
        Body body => mainPart != null ? mainPart.Document.Body : null;

        public LogosBibleModelService() { }
        public void Export(BibleModel bibleModel, string outputFilePath) {
            FootnoteId = 1;
            if (bibleModel != null) {
                File.WriteAllBytes(outputFilePath, Properties.Resources.Template);
                using (WordprocessingDocument wordDocument =
                        WordprocessingDocument.Open(outputFilePath, true, new OpenSettings() { })) {
                    CurrentDocument = wordDocument;

                    AppendTitle(bibleModel.Name);

                    foreach (var book in bibleModel.Books) {
                        
                        AppendHeading1($"{BibleType}{book.BookShortcut}]]{book.BookName}");

                        var tbl = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Table());
                        tbl.AppendChild(new TableProperties() {
                            TableStyle = new TableStyle() { Val = "Tabela-Siatka" },
                            TableWidth = new TableWidth() { Type = TableWidthUnitValues.Auto },
                            TableBorders = new TableBorders() {
                                TopBorder = new TopBorder() { Val = BorderValues.None, Color = "auto", Space = 0, Size = 0 },
                                LeftBorder = new LeftBorder() { Val = BorderValues.None, Color = "auto", Space = 0, Size = 0 },
                                RightBorder = new RightBorder() { Val = BorderValues.None, Color = "auto", Space = 0, Size = 0 },
                                BottomBorder = new BottomBorder() { Val = BorderValues.None, Color = "auto", Space = 0, Size = 0 }
                            },
                            TableLook = new TableLook() {
                                Val = "04A0",
                                NoVerticalBand = new OnOffValue(true),
                                NoHorizontalBand = new OnOffValue(false),
                                LastColumn = new OnOffValue(false),
                                FirstColumn = new OnOffValue(true),
                                LastRow = new OnOffValue(false),
                                FirstRow = new OnOffValue(true),
                            }
                        });
                        var tblGrid = tbl.AppendChild(new TableGrid());
                        tblGrid.AppendChild(new GridColumn() { Width = "2122" });
                        tblGrid.AppendChild(new GridColumn() { Width = "6940" });

                        if (book.AuthorName != null && book.AuthorName.Items.Count > 0) {                            
                            AppendTableRow(tbl, "Autor:", book.AuthorName.ToString());
                        }
                        if (book.TimeOfWriting != null && book.TimeOfWriting.Items.Count > 0) {
                            AppendTableRow(tbl, "Czas:", book.TimeOfWriting.ToString());
                        }
                        if (book.PlaceWhereBookWasWritten != null && book.PlaceWhereBookWasWritten.Items.Count > 0) {
                            AppendTableRow(tbl, "Miejsce:", book.PlaceWhereBookWasWritten.ToString());
                        }
                        if (book.Purpose != null && book.Purpose.Items.Count > 0) {
                            AppendTableRow(tbl, "Cel:", book.Purpose.ToString());
                        }
                        if (book.Subject != null && book.Subject.Items.Count > 0) {
                            AppendTableRow(tbl, "Temat:", book.Subject.ToString());
                        }

                        AppendParagraph();

                        
                        var isPsalm = book.NumberOfBook == 230;
                        var chapterPrefix = !isPsalm ? bibleModel.ChapterName : bibleModel.PsalmName;
                        foreach (var chapter in book.Chapters) {
                            if (book.NumberOfChapters == 0) { book.NumberOfChapters = book.Chapters.Count; }
                            if (book.NumberOfChapters > 1) {
                                AppendHeading2($"{BibleType}{book.BookShortcut} {chapter.NumberOfChapter}]]{chapterPrefix} {chapter.NumberOfChapter}");
                            }
                            DocumentFormat.OpenXml.Wordprocessing.Paragraph para = null;
                            var firstInSection = true;
                            foreach (var item in chapter.Items) {
                                if (item is VerseModel) {
                                    var verse = item as VerseModel;
                                    if (para == null || firstInSection || verse.NumberOfVerse == 1 || isPsalm) {
                                        para = AppendParagraph();
                                        firstInSection = false;
                                    }
                                    var verseRunText = $"{BibleType}{book.BookShortcut} {chapter.NumberOfChapter}:{verse.NumberOfVerse}]]";
                                    AppendRun(verseRunText, para);
                                    AppendRun($"{verse.NumberOfVerse}.\u00A0", para, bold: true, sup: true);
                                    AppendRun($"{{{{field-on:bible}}}}", para);

                                    foreach (var verseItem in verse.Items) {
                                        if (verseItem is Span) {
                                            AppendRun((verseItem as Span).ToString(), para, SpaceProcessingModeValues.Preserve, removeOrphans: true);
                                        }
                                        else if (verseItem is BreakLine) {
                                            para = AppendParagraph();
                                        }
                                        else if (verseItem is NoteModel) {
                                            var note = verseItem as NoteModel;
                                            AppendFootnote(note, para);
                                        }
                                        else if (item is string) {
                                            AppendRun(item.ToString(), para, removeOrphans: true);
                                        }
                                    }

                                    AppendRun("{{field-off:bible}} ", para, SpaceProcessingModeValues.Preserve);

                                }
                                else if (item is FormattedText) {
                                    AppendHeading3((item as FormattedText).ToString());
                                    firstInSection = true;
                                }
                            }
                        }
                    }
                    wordDocument.Save();
                }
            }
        }

        private void AppendTableRow(DocumentFormat.OpenXml.Wordprocessing.Table tbl,
            string label, string text) {
            // Label
            var tr = tbl.AppendChild(new TableRow());
            {
                var tc = tr.AppendChild(new TableCell() {
                    TableCellProperties = new TableCellProperties() {
                        TableCellWidth = new TableCellWidth() { Width = "2122", Type = TableWidthUnitValues.Dxa }
                    }
                });
                var p = tc.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph() { 
                    ParagraphProperties = new ParagraphProperties() { }
                });
                var run = p.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run() { 
                    RunProperties = new RunProperties() {
                        Bold = new DocumentFormat.OpenXml.Wordprocessing.Bold(),
                        BoldComplexScript = new BoldComplexScript()
                    }                    
                });
                run.AppendChild(new Text(label));
            }
            {
                var tc = tr.AppendChild(new TableCell() {
                    TableCellProperties = new TableCellProperties() {
                        TableCellWidth = new TableCellWidth() { Width = "6940", Type = TableWidthUnitValues.Dxa }
                    }
                });
                var p = tc.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph() {
                    ParagraphProperties = new ParagraphProperties() { }
                });
                var run = p.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                run.AppendChild(new Text(text));
            }
        }

        public void Export(string inputFilePath, string outputFilePath) {
            using (var service = new BibleModelService()) {
                var model = service.GetModelFromFile(inputFilePath);
                Export(model, outputFilePath);
            }
        }

        private void AppendFootnote(NoteModel note, DocumentFormat.OpenXml.Wordprocessing.Paragraph para) {
            var footnote = new Footnote() { Id = FootnoteId };
            var footnoteTextParagraph = new DocumentFormat.OpenXml.Wordprocessing.Paragraph() {
                ParagraphProperties = new ParagraphProperties() {
                    ParagraphStyleId = new ParagraphStyleId() { Val = "Tekstprzypisudolnego" }
                }
            };
            var footnoteTextRun = footnoteTextParagraph.AppendChild(new Run() {
                RunProperties = new RunProperties() {
                    VerticalTextAlignment = new VerticalTextAlignment() {
                        Val = VerticalPositionValues.Superscript
                    }
                }
            });
            footnoteTextRun.AppendChild(new Text($"{note.Number}\u00A0"));

            foreach (var item in note.Items) {
                if (item is Span) {
                    var span = item as Span;
                    if (span.Language.IsNotNullOrEmpty()) {
                        AppendRun(span.ToString(), footnoteTextParagraph, SpaceProcessingModeValues.Preserve, lang: span.Language, rtl: span.RTL);
                    }
                    else {
                        AppendRun(span.ToString(), footnoteTextParagraph, SpaceProcessingModeValues.Preserve, removeOrphans: true);
                    }
                }
                else if (item is NoteReferenceModel) {
                    var reference = item as NoteReferenceModel;
                    var index = reference.Index;
                    if (index.SecondChapterNumber > 0 && index.SecondChapterNumber != index.ChapterNumber) {
                        AppendRun($"[[{reference.Text}>>{reference.Index.BookShortcut} {reference.Index.ChapterNumber}:{reference.Index.VerseStartNumber}-{reference.Index.SecondChapterNumber}:{reference.Index.VerseEndNumber}]]", footnoteTextParagraph);
                    }
                    else {
                        AppendRun($"[[{reference.Text}>>{index.BookShortcut} {index.ChapterNumber}:{index.VerseStartNumber}{(index.VerseEndNumber > 0 ? "-" + index.VerseEndNumber : "")}]]", footnoteTextParagraph);
                    }
                }
                else if (item is string) {
                    AppendRun(item.ToString(), footnoteTextParagraph, removeOrphans: true);
                }
            }

            footnote.AppendChild(footnoteTextParagraph);

            if (mainPart.FootnotesPart == null) {
                var footPart = mainPart.AddNewPart<FootnotesPart>();
                footPart.Footnotes = new Footnotes();
            }

            mainPart.FootnotesPart.Footnotes.Append(footnote);

            var runRef = para.AppendChild(new Run() {
                RunProperties = new RunProperties() {
                    VerticalTextAlignment = new VerticalTextAlignment() {
                        Val = VerticalPositionValues.Superscript
                    }
                }
            });
            runRef.AppendChild(new FootnoteReference() { CustomMarkFollows = new OnOffValue(true), Id = FootnoteId });
            runRef.AppendChild(new Text(note.Number));
            AppendRun(" ", para, SpaceProcessingModeValues.Preserve);
            FootnoteId++;
        }

        DocumentFormat.OpenXml.Wordprocessing.Paragraph AppendTitle(string text) {
            var para = AppendParagraph("Tytu");
            AppendRun(text, para);
            return para;
        }
        DocumentFormat.OpenXml.Wordprocessing.Paragraph AppendHeading1(string text) {
            var para = AppendParagraph("Nagwek1");
            AppendRun(text, para);
            return para;
        }
        DocumentFormat.OpenXml.Wordprocessing.Paragraph AppendHeading2(string text) {
            var para = AppendParagraph("Nagwek2");
            AppendRun(text, para);
            return para;
        }
        DocumentFormat.OpenXml.Wordprocessing.Paragraph AppendHeading3(string text) {
            var para = AppendParagraph("Nagwek3");
            AppendRun(text, para);
            return para;
        }
        DocumentFormat.OpenXml.Wordprocessing.Paragraph AppendParagraph(string styleId = "Normalny") {
            var para = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph() { ParagraphProperties = new ParagraphProperties() });
            para.ParagraphProperties.ParagraphStyleId = new ParagraphStyleId() {
                Val = styleId
            };

            return para;
        }
        Run AppendRun(string text, DocumentFormat.OpenXml.Wordprocessing.Paragraph para, SpaceProcessingModeValues space = SpaceProcessingModeValues.Default, string lang = null, bool rtl = false, bool bold = false, bool sup = false, bool removeOrphans = false) {
            var run = para.AppendChild(new Run() { RunProperties = new RunProperties() });
            if (lang.IsNotNullOrEmpty()) {
                if (lang == "he") {
                    run.RunProperties.AddChild(new Languages() { Bidi = "he-IL" });
                    if (rtl) { run.RunProperties.AddChild(new RightToLeftText()); }
                }
                else if (lang == "gr") {
                    run.RunProperties.AddChild(new Languages() { Val = "el-GR" });
                }
            }
            if (bold) {
                run.RunProperties.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Bold());
            }
            if (sup) {
                run.RunProperties.AppendChild(new VerticalTextAlignment() { Val = VerticalPositionValues.Superscript });
            }

            if (removeOrphans) {
                var pattern = @"((?<text1>\s[a-zA-Z])(?<space1>\s)(?<text2>[a-zA-Z])(?<space2>\s))|((?<text3>\s[a-zA-Z])(?<space3>\s))|((?<text4>\s[0-9]+)(?<space4>\s))|(^(?<text5>[a-zA-Z])(?<space5>\s))";
                text = Regex.Replace(text, pattern, delegate (Match m) {
                    if (m.Groups["text1"] != null && m.Groups["text1"].Value.IsNotNullOrEmpty()) {
                        return $"{m.Groups["text1"].Value}\u00A0{m.Groups["text2"].Value}\u00A0";
                    }
                    if (m.Groups["text3"] != null && m.Groups["text3"].Value.IsNotNullOrEmpty()) {
                        return $"{m.Groups["text3"].Value}\u00A0";
                    }
                    if (m.Groups["text4"] != null && m.Groups["text4"].Value.IsNotNullOrEmpty()) {
                        return $"{m.Groups["text4"].Value}\u00A0";
                    }
                    if (m.Groups["text5"] != null && m.Groups["text5"].Value.IsNotNullOrEmpty()) {
                        return $"{m.Groups["text5"].Value}\u00A0";
                    }
                    return m.Value; // nic nie zmieniam
                });
            }

            run.AppendChild(new Text(text) { Space = space });
            return run;
        }

        public void Dispose() { }
    }
}
