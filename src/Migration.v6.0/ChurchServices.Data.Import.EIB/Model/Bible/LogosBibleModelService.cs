using ChurchServices.Extensions;
using DocumentFormat.OpenXml;
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
        public BibleModel Export(BibleModel bibleModel, string outputFilePath, bool repairModel = false, string introductionHtml = null, bool addTitle = true) {
            FootnoteId = 1;
            if (bibleModel != null) {

                if (repairModel) {
                    bibleModel = RepairModel(bibleModel);
                }

                File.WriteAllBytes(outputFilePath, Properties.Resources.Template);
                using (WordprocessingDocument wordDocument =
                        WordprocessingDocument.Open(outputFilePath, true, new OpenSettings() { })) {
                    CurrentDocument = wordDocument;

                    if (addTitle) {
                        AppendTitle(bibleModel.Name);
                    }

                    if (introductionHtml != null) {
                        var srv = new HtmlToOpenXml.HtmlConverter(mainPart);
                        srv.ParseHtml(introductionHtml);
                    }

                    foreach (var book in bibleModel.Books) {

                        AppendHeading1($"{BibleType}{book.BookShortcut}]]{book.BookName}");

                        var tbl = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Table());
                        tbl.AppendChild(new TableProperties() {
                            TableStyle = new TableStyle() { Val = "Tabela-Siatka" },
                            TableWidth = new TableWidth() { Type = TableWidthUnitValues.Pct, Width = "5000" },
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
                                    var verseRunText = "";
                                    if (verse.IsTitle) {
                                        verseRunText = $"{BibleType}{book.BookShortcut} {chapter.NumberOfChapter}:title]]";
                                    }
                                    else {
                                        verseRunText = $"{BibleType}{book.BookShortcut} {chapter.NumberOfChapter}:{verse.NumberOfVerse}]]";
                                    }
                                    AppendRun(verseRunText, para);
                                    if (!verse.IsTitle) {
                                        AppendRun($"{verse.NumberOfVerse}.\u00A0", para, bold: true, sup: true);
                                    }
                                    AppendRun($"{{{{field-on:bible}}}}", para);

                                    foreach (var verseItem in verse.Items) {
                                        if (verseItem is SpanModel) {
                                            var span = verseItem as SpanModel;
                                            if (span.Language.IsNotNullOrEmpty()) {
                                                AppendRun(span.ToString(), para, SpaceProcessingModeValues.Preserve, lang: span.Language, rtl: span.RTL, bold: span.Bold, italic: span.Italic, removeOrphans: true, sup: span.Sup);
                                            }
                                            else {
                                                AppendRun(span.ToString(), para, SpaceProcessingModeValues.Preserve, bold: span.Bold, italic: span.Italic, removeOrphans: true, sup: span.Sup);
                                            }
                                        }
                                        else if (verseItem is WordOfGodModel) {
                                            AppendRun($"{{{{field-on:word-of-christ}}}}{(verseItem as WordOfGodModel)}{{{{field-off:word-of-christ}}}}", para, SpaceProcessingModeValues.Preserve, removeOrphans: true);
                                        }
                                        else if (verseItem is BreakLineModel) {
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
                                else if (item is BreakLineModel) {
                                    para = AppendParagraph();
                                }
                                else if (item is SpanModel) {
                                    var span = item as SpanModel;
                                    if (span.Language.IsNotNullOrEmpty()) {
                                        AppendRun(span.ToString(), para, SpaceProcessingModeValues.Preserve, lang: span.Language, rtl: span.RTL, bold: span.Bold, italic: span.Italic, removeOrphans: true, sup: span.Sup);
                                    }
                                    else {
                                        AppendRun(span.ToString(), para, SpaceProcessingModeValues.Preserve, bold: span.Bold, italic: span.Italic, removeOrphans: true, sup: span.Sup);
                                    }
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

            return bibleModel;
        }
        public BibleModel Export(string inputFilePath, string outputFilePath, bool repairModel = false, string introductionHtml = null, bool addTitle = true) {
            using (var service = new BibleModelService()) {
                var model = service.GetModelFromFile(inputFilePath);
                return Export(model, outputFilePath, repairModel, introductionHtml, addTitle);
            }
        }

        private BibleModel RepairModel(BibleModel model) {
            if (model != null) {
                var Ge = model.Books.Where(x => x.NumberOfBook == 10).FirstOrDefault();
                if (Ge != null) {
                    var versesCount31 = Ge.Chapters.Where(x => x.NumberOfChapter == 31).First().Verses().Count();
                    var versesCount32 = Ge.Chapters.Where(x => x.NumberOfChapter == 32).First().Verses().Count();
                    if (versesCount32 == 33 && versesCount31 == 54) {
                        MoveVersesToAnotherChapter(Ge, 32, 1, 1, 31, 55, true);
                    }
                }

                var Num = model.Books.Where(x => x.NumberOfBook == 40).FirstOrDefault();
                if (Num != null) {
                    MoveVersesToAnotherChapter(Num, 17, 1, 15, 16, 36, true);
                    MoveVersesToAnotherChapter(Num, 30, 1, 1, 29, 40, true);
                }

                var Dt = model.Books.Where(x => x.NumberOfBook == 50).FirstOrDefault();
                if (Dt != null) {
                    MoveVersesToAnotherChapter(Dt, 13, 1, 1, 12, 32, true);
                    MoveVersesToAnotherChapter(Dt, 23, 1, 1, 22, 30, true);
                    MoveLastToNextChapter(Dt, 28, 69);
                }

                var ISa = model.Books.Where(x => x.NumberOfBook == 90).FirstOrDefault();
                if (ISa != null) {
                    MoveFirstVerseOfChapterToLastVersePreviousChapter(ISa, 21);
                    MoveVersesToAnotherChapter(ISa, 24, 1, 1, 23, 23, true);
                }

                var IKi = model.Books.Where(x => x.NumberOfBook == 110).FirstOrDefault();
                if (IKi != null) {
                    MoveVerseToAnotherVerse(IKi, 22, 44, 43);
                }

                var IIKi = model.Books.Where(x => x.NumberOfBook == 120).FirstOrDefault();
                if (IIKi != null) {
                    MoveVersesToAnotherChapter(IIKi, 12, 1, 1, 11, 21, true);
                }

                var ICh = model.Books.Where(x => x.NumberOfBook == 130).FirstOrDefault();
                if (ICh != null) {
                    var ch5 = ICh.Chapters.Where(x => x.NumberOfChapter == 5).FirstOrDefault();
                    if (ch5 != null && ch5.Verses().Count() > 26) {
                        RenumerateChapter(ICh, 6, 16);
                        MoveVersesToBeginingOfChapter(ICh, 5, 27, 41, 6);
                    }
                    MoveVerseToAnotherVerse(ICh, 12, 5, 4);
                }

                var IICh = model.Books.Where(x => x.NumberOfBook == 140).FirstOrDefault();
                if (IICh != null) {
                    MoveLastToNextChapter(IICh, 1, 18);
                    MoveLastToNextChapter(IICh, 13, 23);
                }

                var Ne = model.Books.Where(x => x.NumberOfBook == 160).FirstOrDefault();
                if (Ne != null) {
                    MoveVersesToAnotherChapter(Ne, 10, 1, 1, 9, 38, true);
                }

                var Job = model.Books.Where(x => x.NumberOfBook == 220).FirstOrDefault();
                if (Job != null) {
                    var ch40 = Job.Chapters.Where(x => x.NumberOfChapter == 40).FirstOrDefault();
                    if (ch40.Verses().Count() > 24) {
                        RenumerateChapter(Job, 41, 9);
                        MoveVersesToBeginingOfChapter(Job, 40, 25, 32, 41);
                    }
                }

                var Ps = model.Books.Where(x => x.NumberOfBook == 230).FirstOrDefault();
                if (Ps != null) {
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 3).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 4).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 5).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 6).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 7).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 8).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 9).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 12).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 18).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 19).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 20).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 21).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 22).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 30).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 31).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 34).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 36).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 38).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 39).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 40).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 41).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 42).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 44).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 45).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 46).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 47).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 48).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 49).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 51).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 51).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 52).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 52).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 53).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 54).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 54).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 55).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 56).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 57).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 58).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 59).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 60).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 60).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 61).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 62).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 63).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 64).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 65).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 67).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 68).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 69).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 70).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 75).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 76).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 77).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 80).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 81).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 83).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 84).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 85).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 88).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 89).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 92).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 102).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 108).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 140).FirstOrDefault());
                    //AddSecondVerseUp(Ps.Chapters.Where(x => x.NumberOfChapter == 142).FirstOrDefault());
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 3);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 4);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 5);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 6);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 7);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 8);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 9);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 12);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 18);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 19);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 20);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 21);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 22);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 30);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 31);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 34);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 36);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 38);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 39);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 40);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 41);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 42);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 44);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 45);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 46);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 47);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 48);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 49);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 51, 2);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 52, 2);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 53);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 54, 2);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 55);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 56);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 57);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 58);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 59);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 60, 2);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 61);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 62);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 63);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 64);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 65);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 67);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 68);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 69);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 70);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 75);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 76);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 77);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 80);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 81);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 83);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 84);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 85);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 88);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 89);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 92);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 102);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 108);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 140);
                    MarkFirstAsTitleAndRenumerateChapter(Ps, 142);
                }

                var Ecc = model.Books.Where(x => x.NumberOfBook == 250).FirstOrDefault();
                if (Ecc != null) {
                    MoveLastToNextChapter(Ecc, 4, 17);
                    AddSecondVerseUp(Ecc, 8, 18);
                }

                var Song = model.Books.Where(x => x.NumberOfBook == 260).FirstOrDefault();
                if (Song != null) {
                    MoveVersesToAnotherChapter(Song, 7, 1, 1, 6, 13, true);
                }

                var Isa = model.Books.Where(x => x.NumberOfBook == 290).FirstOrDefault();
                if (Isa != null) {
                    MoveLastToNextChapter(Isa, 8, 23);
                }
                var Je = model.Books.Where(x => x.NumberOfBook == 300).FirstOrDefault();
                if (Je != null) {
                    MoveLastToNextChapter(Je, 8, 23);
                }
                var Da = model.Books.Where(x => x.NumberOfBook == 340).FirstOrDefault();
                if (Da != null) {
                    var ch3 = Da.Chapters.Where(x => x.NumberOfChapter == 3).FirstOrDefault();
                    
                    if (ch3 != null && ch3.Verses().Count() == 100) {
                        var v23 = ch3.Verses().Where(x => x.NumberOfVerse == 23).FirstOrDefault();
                        var idxStart = ch3.Items.IndexOf(ch3.Verses().Where(x => x.NumberOfVerse == 24).FirstOrDefault());
                        var idxEnd = ch3.Items.IndexOf(ch3.Verses().Where(x => x.NumberOfVerse == 90).FirstOrDefault());
                        var v = ch3.Items.Between(idxStart, idxEnd);
                        var vr = new List<object>();
                        foreach (var item in v) {
                            var itemVerse = item as VerseModel;

                            if (itemVerse != null) {
                                if (itemVerse.NumberOfVerse == 25) {
                                    vr.Add(new FormattedText("Pieśń Azariasza"));
                                    vr.Add(new BreakLineModel());
                                }
                                if (itemVerse.NumberOfVerse == 51) {
                                    vr.Add(new FormattedText("Pieśń trzech młodzieńców"));
                                    vr.Add(new BreakLineModel());
                                }
                                vr.Add(new SpanModel($" ({itemVerse.NumberOfVerse}) ") { Sup = true });
                                vr.Add(new SpanModel(itemVerse.ToString()));
                            }
                        }

                        vr.Add(new BreakLineModel());
                        vr.Add(new FormattedText("Nabuchodonozor ułaskawia trzech młodzieńców"));
                        vr.Add(new BreakLineModel());

                        ch3.Items.RemoveRange(idxStart, (idxEnd - idxStart) + 1);
                        RenumerateChapter(ch3, 1);
                        ch3.Items.AddRangeAfterSelf(vr, v23);
                    }

                    if (ch3 != null && ch3.Verses().Count() == 33) {
                        RenumerateChapter(Da, 4, 4);
                        MoveVersesToBeginingOfChapter(Da, 3, 31, 33, 4, 1);
                    }
                }

                var Zach = model.Books.Where(x => x.NumberOfBook == 450).FirstOrDefault();
                if (Zach != null) {
                    MoveVersesToAnotherChapter(Zach, 2, 1, 4, 1, 18, true);
                }

                var Mal = model.Books.Where(x => x.NumberOfBook == 460).FirstOrDefault();
                if (Mal != null) {
                    MoveVersesToAnotherChapter(Mal, 3, 19, 24, 4, 1);
                }
            }
            return model;
        }

        private void MoveFirstVerseOfChapterToLastVersePreviousChapter(BookModel book, int ch) {
            var chapterFrom = book.Chapters.Where(x => x.NumberOfChapter == ch).FirstOrDefault();
            if (chapterFrom != null) {
                var verseFrom = chapterFrom.Verses().First();
                if (verseFrom != null) {
                    var chapterTo = book.Chapters.Where(x => x.NumberOfChapter == (ch - 1)).FirstOrDefault();
                    if (chapterTo != null) {
                        var verseTo = chapterTo.Verses().Last();
                        if (verseTo != null) {
                            verseTo.Items.Add(new SpanModel(" "));
                            verseTo.Items.AddRange(verseFrom.Items);
                            chapterFrom.Items.Remove(verseFrom);

                            foreach (var verse in chapterFrom.Verses()) {
                                verse.NumberOfVerse = verse.NumberOfVerse - 1;
                            }
                        }
                    }
                }
            }
        }

        private void RenumerateChapter(BookModel book, int ch, int vs) {
            var chapter = book.Chapters.Where(x => x.NumberOfChapter == ch).FirstOrDefault();
            if (chapter != null) {
                var chToVerses = chapter.Verses().ToArray();
                for (int i = 0; i < chToVerses.Length; i++) {
                    var item = chToVerses[i];
                    item.NumberOfVerse = vs;
                    vs++;
                }
            }
        }
        private void RenumerateChapter(ChapterModel chapter, int vs) {
            if (chapter != null) {
                var chToVerses = chapter.Verses().ToArray();
                for (int i = 0; i < chToVerses.Length; i++) {
                    var item = chToVerses[i];
                    item.NumberOfVerse = vs;
                    vs++;
                }
            }
        }
        private void MoveVersesToBeginingOfChapter(BookModel book, int ch, int vs, int ve, int chTo, int vf = 1) {
            var chapterFrom = book.Chapters.Where(x => x.NumberOfChapter == ch).FirstOrDefault();
            var chapterTo = book.Chapters.Where(x => x.NumberOfChapter == chTo).FirstOrDefault();
            if (chapterFrom != null) {
                if (chapterTo == null) {
                    chapterTo = new ChapterModel() { NumberOfChapter = chTo, Items = new List<object>() };
                    book.Chapters.Add(chapterTo);
                }

                var itemStart = chapterFrom.Verses().Where(x => x.NumberOfVerse == vs).FirstOrDefault();
                if (itemStart != null) {
                    var idxStart = chapterFrom.Items.IndexOf(itemStart);
                    if (idxStart > 0) {
                        var previous = chapterFrom.Items[idxStart - 1];
                        if (previous is FormattedText) {
                            idxStart--;
                        }
                    }
                    var idxEnd = chapterFrom.Items.IndexOf(chapterFrom.Verses().Where(x => x.NumberOfVerse == ve).FirstOrDefault());
                    var items = chapterFrom.Items.Between(idxStart, idxEnd);
                    var vn = vf;
                    foreach (var item in items) {
                        chapterFrom.Items.Remove(item);
                        if (item is VerseModel) {
                            (item as VerseModel).NumberOfVerse = vn;
                            vn++;
                        }
                    }

                    items.Reverse();

                    foreach (var item in items) {
                        chapterTo.Items.Insert(0, item);
                    }
                }
            }
        }
        private void MoveVersesToAnotherChapter(BookModel book, int ch, int vs, int ve, int chTo, int vf, bool renumerateChapterFrom = false, int renumerateChapterFromNum = 1) {
            var chapterFrom = book.Chapters.Where(x => x.NumberOfChapter == ch).FirstOrDefault();
            var chapterTo = book.Chapters.Where(x => x.NumberOfChapter == chTo).FirstOrDefault();
            if (chapterFrom != null) {
                if (chapterTo == null) {
                    chapterTo = new ChapterModel() { NumberOfChapter = chTo, Items = new List<object>() };
                    book.Chapters.Add(chapterTo);
                }

                var itemStart = chapterFrom.Verses().Where(x => x.NumberOfVerse == vs).FirstOrDefault();

                var idxStart = chapterFrom.Items.IndexOf(itemStart);
                if (idxStart > 0) {
                    var previous = chapterFrom.Items[idxStart - 1];
                    if (previous is FormattedText) {
                        idxStart--;
                    }
                }
                var idxEnd = chapterFrom.Items.IndexOf(chapterFrom.Verses().Where(x => x.NumberOfVerse == ve).FirstOrDefault());
                var items = chapterFrom.Items.Between(idxStart, idxEnd);
                var vn = vf;

                foreach (var item in items) {
                    chapterFrom.Items.Remove(item);
                    if (item is VerseModel) {
                        (item as VerseModel).NumberOfVerse = vn;
                        vn++;
                    }
                    chapterTo.Items.Add(item);
                }

                if (renumerateChapterFrom) {
                    var chFromVerses = chapterFrom.Verses().ToArray();
                    for (int i = 0; i < chFromVerses.Length; i++) {
                        var item = chFromVerses[i];
                        item.NumberOfVerse = renumerateChapterFromNum;
                        renumerateChapterFromNum++;
                    }
                }
            }
        }

        private void MoveLastToNextChapter(BookModel book, int ch, int ve) {
            var chapter = book.Chapters.Where(x => x.NumberOfChapter == ch).FirstOrDefault();
            if (chapter != null) {
                var last = chapter.Verses().Where(x => x.NumberOfVerse == ve).FirstOrDefault();
                if (last != null) {
                    var nchapter = book.Chapters.Where(x => x.NumberOfChapter == ch + 1).FirstOrDefault();
                    if (nchapter != null) {

                        var vn = 2;
                        foreach (var item in nchapter.Items) {
                            if (item is VerseModel) {
                                (item as VerseModel).NumberOfVerse = vn;
                                vn++;
                            }
                        }

                        var idx = chapter.Items.IndexOf(last);
                        if (idx > 0) {
                            (chapter.Items[idx] as VerseModel).NumberOfVerse = 1;
                            nchapter.Items.Insert(0, chapter.Items[idx]);
                            chapter.Items.RemoveAt(idx);

                            var previous = chapter.Items[idx - 1];
                            if (previous != null && previous is FormattedText) {
                                nchapter.Items.Insert(0, previous);
                                chapter.Items.RemoveAt(idx - 1);
                            }
                        }
                    }
                }
            }
        }

        private void MoveVerseToAnotherVerse(BookModel book, int ch, int v1, int v2, bool renumerateChapterVerses = true) {
            var chapter = book.Chapters.Where(x => x.NumberOfChapter == ch).FirstOrDefault();
            if (chapter != null) {
                var verse1 = chapter.Verses().Where(x => x.NumberOfVerse == v1).FirstOrDefault();
                var verse2 = chapter.Verses().Where(x => x.NumberOfVerse == v2).FirstOrDefault();
                if (verse1 != null && verse2 != null) {
                    verse2.Items.Add(new SpanModel(" "));
                    verse2.Items.AddRange(verse1.Items);
                    chapter.Items.Remove(verse1);

                    var vn = 1;
                    foreach (var item in chapter.Verses()) {
                        item.NumberOfVerse = vn;
                        vn++;
                    }
                }
            }
        }

        private void MarkFirstAsTitleAndRenumerateChapter(BookModel book, int ch, int numOfVerses = 1) {
            var chapter = book.Chapters.Where(x => x.NumberOfChapter == ch).FirstOrDefault();
            if (chapter != null) {
                var first = chapter.Verses().Where(x => x.NumberOfVerse == 1).FirstOrDefault();
                if (first != null) {
                    first.NumberOfVerse = 0;
                    first.IsTitle = true;

                    if (numOfVerses > 1) {
                        for (int i = 1; i < numOfVerses; i++) {
                            var second = chapter.Verses().Where(x => x.NumberOfVerse > 0).FirstOrDefault();
                            if (second != null) {
                                first.Items.Add(new BreakLineModel());
                                first.Items.AddRange(second.Items);
                                chapter.Items.Remove(second);
                            }
                        }
                    }

                    var vn = 1;
                    foreach (var item in chapter.Verses()) {
                        if (item == first) { continue; }
                        item.NumberOfVerse = vn;
                        vn++;
                    }
                }
            }
        }

        private void AddSecondVerseUp(BookModel book, int ch, int ifVersesCountEquals = 0) {
            var chapter = book.Chapters.Where(x => x.NumberOfChapter == ch).FirstOrDefault();
            if (chapter != null) {
                if (ifVersesCountEquals > 0 && ifVersesCountEquals != chapter.Verses().Count()) { return; }
                AddSecondVerseUp(chapter);
            }
        }

        private void AddSecondVerseUp(ChapterModel chapter) {
            if (chapter != null && chapter.Verses().Count() > 1) {
                var first = chapter.Verses().Where(x => x.NumberOfVerse == 1).FirstOrDefault();
                var second = chapter.Verses().Where(x => x.NumberOfVerse == 2).FirstOrDefault();
                if (first != null & second != null) {
                    first.Items.Add(new SpanModel(" "));
                    while (second.Items.Count > 0) {
                        var item = second.Items.First();
                        first.Items.Add(item);
                        second.Items.Remove(item);
                    }
                    chapter.Items.Remove(second);
                }
                var vn = 1;
                foreach (var item in chapter.Verses()) {
                    item.NumberOfVerse = vn;
                    vn++;
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
                        TableCellWidth = new TableCellWidth() { Width = "800", Type = TableWidthUnitValues.Pct }
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
                        TableCellWidth = new TableCellWidth() { Width = "4200", Type = TableWidthUnitValues.Pct }
                    }
                });
                var p = tc.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph() {
                    ParagraphProperties = new ParagraphProperties() { }
                });
                var run = p.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Run());
                run.AppendChild(new Text(text));
            }
        }

        private void AppendFootnote(NoteModel note, DocumentFormat.OpenXml.Wordprocessing.Paragraph para) {
            if (note != null && note.Items.Count > 0) {
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
                    if (item is SpanModel) {
                        var span = item as SpanModel;
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
            else {
                //throw new Exception("")
            }
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
        DocumentFormat.OpenXml.Wordprocessing.Paragraph GetLastParagraph() {
            return body.ChildElements.Where(x => x is DocumentFormat.OpenXml.Wordprocessing.Paragraph).LastOrDefault() as DocumentFormat.OpenXml.Wordprocessing.Paragraph;
        }
        DocumentFormat.OpenXml.Wordprocessing.Paragraph AppendParagraph(string styleId = "Normalny") {
            var para = GetLastParagraph();
            if (para != null &&
                para.InnerText.IsNullOrEmpty()) {
                if (para.ParagraphProperties == null) { para.ParagraphProperties = new ParagraphProperties(); }
                para.ParagraphProperties.ParagraphStyleId = new ParagraphStyleId() {
                    Val = styleId
                };
                return para;
            }
            para = body.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Paragraph() { ParagraphProperties = new ParagraphProperties() });
            para.ParagraphProperties.ParagraphStyleId = new ParagraphStyleId() {
                Val = styleId
            };

            return para;
        }
        Run AppendRun(string text, DocumentFormat.OpenXml.Wordprocessing.Paragraph para, SpaceProcessingModeValues space = SpaceProcessingModeValues.Default, string lang = null, bool rtl = false, bool bold = false, bool sup = false, bool removeOrphans = false, bool italic = false, bool underline = false) {
            var run = para.AppendChild(new Run() { RunProperties = new RunProperties() });
            if (lang.IsNotNullOrEmpty()) {
                if (lang == "he") {
                    run.RunProperties.AddChild(new Languages() { Bidi = "he-IL" });
                    if (rtl) { run.RunProperties.AddChild(new RightToLeftText()); }
                }
                else if (lang == "gr") {
                    run.RunProperties.AddChild(new Languages() { Val = "el-GR" });
                }
                else if (lang == "la") {
                    run.RunProperties.AddChild(new Languages() { Val = "la-Latn" });
                }
            }
            if (bold) {
                run.RunProperties.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Bold());
            }
            if (italic) {
                run.RunProperties.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Italic());
            }
            if (underline) {
                run.RunProperties.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Underline());
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
