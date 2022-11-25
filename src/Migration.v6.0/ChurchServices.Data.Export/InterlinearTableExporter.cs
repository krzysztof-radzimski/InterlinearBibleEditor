using Aspose.Words;
using Aspose.Words.Layout;
using Aspose.Words.Tables;
using ChurchServices.Data.Export.Controllers;
using ChurchServices.Data.Model;

namespace ChurchServices.Data.Export {
    public class InterlinearTableExporter {
        private DocumentBuilder DocumentBuilder;
        private bool TableIsOpened = false;
        private const int MaxWidth = 450;
        private const int SecureWidth = 400;
        private const int InsecureCharacters = 20;

        LayoutEnumerator LayoutEnumerator;
        LayoutCollector LayoutCollector;
        BibleTagController BibleTagController;
        TranslationControllerModel TranslateModel;

        public InterlinearTableExporter() {
            var document = new Document();

            LayoutEnumerator = new(document);
            LayoutCollector = new(document);

            document.FirstSection.PageSetup.DifferentFirstPageHeaderFooter = true;

            document.FirstSection.PageSetup.PaperSize = PaperSize.A4;
            document.FirstSection.PageSetup.Orientation = Orientation.Portrait;
            document.FirstSection.PageSetup.TopMargin = (2.5F).CentimetersToPoints();
            document.FirstSection.PageSetup.LeftMargin = (1.5F).CentimetersToPoints();
            document.FirstSection.PageSetup.BottomMargin = (1.5F).CentimetersToPoints();
            document.FirstSection.PageSetup.RightMargin = (1.5F).CentimetersToPoints();

            var normalStyle = document.Styles[StyleIdentifier.Normal];
            if (normalStyle.IsNotNull()) {
                normalStyle.Font.Name = "Times New Roman";
                normalStyle.Font.Size = 11;
                normalStyle.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                normalStyle.Font.LocaleId = (int)EditingLanguage.Polish;
                normalStyle.Font.NoProofing = true;
            }

            var hyperLinkStyle = document.Styles[StyleIdentifier.Hyperlink];
            if (hyperLinkStyle.IsNotNull()) {
                hyperLinkStyle.Font.Color = System.Drawing.Color.Black;
                hyperLinkStyle.Font.Underline = Underline.None;
                hyperLinkStyle.Font.LocaleId = (int)EditingLanguage.Polish;
            }

            var footnoteStyle = document.Styles[StyleIdentifier.FootnoteText];
            if (footnoteStyle.IsNotNull()) {
                footnoteStyle.Font.Size = 8;
                footnoteStyle.Font.Color = System.Drawing.Color.Black;
                footnoteStyle.Font.Underline = Underline.None;
                footnoteStyle.Font.LocaleId = (int)EditingLanguage.Polish;
            }

            var footnoteRefStyle = document.Styles[StyleIdentifier.FootnoteReference];
            if (footnoteRefStyle.IsNotNull()) {
                footnoteRefStyle.Font.Size = 10;
                footnoteRefStyle.Font.Color = System.Drawing.Color.Black;
                footnoteRefStyle.Font.Underline = Underline.None;
                footnoteRefStyle.Font.LocaleId = (int)EditingLanguage.Polish;
            }

            var h1 = document.Styles.Add(StyleType.Paragraph, "Nagłówek 1");
            h1.BaseStyleName = "Heading 1";
            h1.Font.Size = 16;
            h1.Font.Bold = true;
            h1.Font.Color = Color.Black;
            h1.Font.Italic = false;
            h1.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            h1.ParagraphFormat.LineSpacing = 18;
            h1.ParagraphFormat.KeepWithNext = true;
            h1.Font.LocaleId = (int)EditingLanguage.Polish;

            var h2 = document.Styles.Add(StyleType.Paragraph, "Nagłówek 2");
            h2.BaseStyleName = "Heading 2";
            h2.Font.Size = 14;
            h2.Font.Bold = true;
            h2.Font.Color = Color.Black;
            h2.Font.Italic = false;
            h2.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            h2.ParagraphFormat.KeepWithNext = true;
            h2.Font.LocaleId = (int)EditingLanguage.Polish;

            var h3 = document.Styles.Add(StyleType.Paragraph, "Nagłówek 3");
            h3.BaseStyleName = "Heading 3";
            h3.Font.Size = 12;
            h3.Font.Bold = true;
            h3.Font.Color = Color.Black;
            h3.Font.Italic = false;
            h3.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            h3.ParagraphFormat.KeepWithNext = true;
            h3.Font.LocaleId = (int)EditingLanguage.Polish;

            DocumentBuilder = new DocumentBuilder(document);
            DocumentBuilder.Font.NoProofing = true;
        }

        public void ExportBook(Book book) {
            DocumentBuilder.ParagraphFormat.ClearFormatting();
            DocumentBuilder.ParagraphFormat.Style = DocumentBuilder.Document.Styles["Nagłówek 1"];
            DocumentBuilder.Write(book.BaseBook.BookTitle);
            DocumentBuilder.InsertBreak(BreakType.ParagraphBreak);

            DocumentBuilder.ParagraphFormat.ClearFormatting();
            var chapters = book.Chapters.OrderBy(x => x.NumberOfChapter).ToList();
            foreach (var chapter in chapters) {
                if (!chapter.IsTranslated) { continue; }
                ExportChapter(chapter, false);
            }

            RemoveEmptyHeading();
        }

        public void ExportChapter(Chapter chapter, bool addBookInfo = true) {
            if (chapter != null) {
                DocumentBuilder.ParagraphFormat.ClearFormatting();
                DocumentBuilder.Font.ClearFormatting();
                DocumentBuilder.ParagraphFormat.Style = DocumentBuilder.Document.Styles["Nagłówek 1"];
                if (addBookInfo) {
                    DocumentBuilder.Write(chapter.ParentBook.BaseBook.BookTitle);
                    DocumentBuilder.InsertBreak(BreakType.LineBreak);
                }
                DocumentBuilder.Write(chapter.ParentBook.NumberOfBook == 230 ? chapter.ParentTranslation.ChapterPsalmString : chapter.ParentTranslation.ChapterString);
                DocumentBuilder.Write(" " + (chapter.ParentTranslation.ChapterRomanNumbering ? chapter.NumberOfChapter.ArabicToRoman() : chapter.NumberOfChapter));
                DocumentBuilder.InsertBreak(BreakType.ParagraphBreak);
                DocumentBuilder.ParagraphFormat.ClearFormatting();

                var verses = chapter.Verses.OrderBy(x => x.NumberOfVerse).ToList();
                var subtitles = chapter.Subtitles.ToList();
                foreach (var verse in verses) {
                    bool beginTable = AddSubtitles(chapter, subtitles, verse);

                    AddVerse(verse, beginTable);
                }

                EndTableAndAddNewParagraph();

                if (addBookInfo) {
                    RemoveEmptyHeading();
                }
            }
        }

        public void ExportVerse(Verse verse, bool addBookInfo = true) {
            if (verse != null) {
                var chapter = verse.ParentChapter;
                DocumentBuilder.ParagraphFormat.ClearFormatting();
                DocumentBuilder.Font.ClearFormatting();
                DocumentBuilder.ParagraphFormat.Style = DocumentBuilder.Document.Styles["Nagłówek 1"];
                if (addBookInfo) {
                    DocumentBuilder.Write(chapter.ParentBook.BaseBook.BookTitle);
                    DocumentBuilder.InsertBreak(BreakType.LineBreak);
                }
                DocumentBuilder.Write(chapter.ParentBook.NumberOfBook == 230 ? chapter.ParentTranslation.ChapterPsalmString : chapter.ParentTranslation.ChapterString);
                DocumentBuilder.Write(" " + (chapter.ParentTranslation.ChapterRomanNumbering ? chapter.NumberOfChapter.ArabicToRoman() : chapter.NumberOfChapter));
                DocumentBuilder.InsertBreak(BreakType.ParagraphBreak);
                DocumentBuilder.ParagraphFormat.ClearFormatting();

                var subtitles = chapter.Subtitles.ToList();
                bool beginTable = AddSubtitles(chapter, subtitles, verse);

                AddVerse(verse, beginTable);

                EndTableAndAddNewParagraph();

                if (addBookInfo) {
                    RemoveEmptyHeading();
                }
            }
        }

        private void RemoveEmptyHeading() {
            foreach (Paragraph par in DocumentBuilder.Document.LastSection.Body.Paragraphs) {
                if ((par.ParagraphFormat.Style == DocumentBuilder.Document.Styles["Nagłówek 1"] ||
                     par.ParagraphFormat.Style == DocumentBuilder.Document.Styles["Nagłówek 2"] ||
                     par.ParagraphFormat.Style == DocumentBuilder.Document.Styles["Nagłówek 3"])) {
                    var text = par.GetText();
                    if (text.Trim().IsNullOrEmpty()) {
                        par.ParagraphFormat.Style = DocumentBuilder.Document.Styles[StyleIdentifier.Normal];
                    }
                }
            }
        }

        private bool AddSubtitles(Chapter chapter, List<Subtitle> subtitles, Verse verse) {
            var beginTable = false;
            if (subtitles.Count > 0 && subtitles.Where(x => x.BeforeVerseNumber == verse.NumberOfVerse && x.Text.IsNotNullOrEmpty()).Any()) {
                var subs = subtitles.Where(x => x.BeforeVerseNumber == verse.NumberOfVerse && x.Text.IsNotNullOrEmpty()).OrderBy(x => x.Level);
                if (TableIsOpened) {
                    EndTableAndAddNewParagraph();
                }
                foreach (var sub in subs) {
                    var _par = subs.First() == sub ? DocumentBuilder.CurrentParagraph : DocumentBuilder.InsertParagraph();
                    _par.ParagraphFormat.ClearFormatting();
                    _par.ParagraphFormat.Style = sub.Level == 1 ? DocumentBuilder.Document.Styles["Nagłówek 2"] : DocumentBuilder.Document.Styles["Nagłówek 3"];
                    _par.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    _par.ParagraphFormat.KeepWithNext = true;

                    var storyText = sub.Text;
                    // <x>230 1-41</x>
                    if (storyText.Contains("<x>")) {
                        var pattern = @"\<x\>(?<book>[0-9]+)\s(?<num>[0-9]+\-[0-9]+)\<\/x\>";
                        var pattern2 = @"\<x\>(?<book>[0-9]+)\s(?<num>[0-9]+(\s)?\:(\s)?[0-9]+\-[0-9]+)\<\/x\>";

                        storyText = Regex.Replace(storyText, pattern, delegate (Match m) {
                            return $"({verse.ParentTranslation.Books.Where(x => x.NumberOfBook == Convert.ToInt32(m.Groups["book"].Value)).First().BookName} {m.Groups["num"].Value})";
                        }, RegexOptions.IgnoreCase);
                        storyText = Regex.Replace(storyText, pattern2, delegate (Match m) {
                            return $"({verse.ParentTranslation.Books.Where(x => x.NumberOfBook == Convert.ToInt32(m.Groups["book"].Value)).First().BookName} {m.Groups["num"].Value})";
                        }, RegexOptions.IgnoreCase);
                    }

                    if (chapter.ParentBook.BaseBook.Status.BiblePart == BiblePart.OldTestament) {
                        storyText = Regex.Replace(storyText, @"\sPAN(A)?(EM)?(U)?(IE)?", delegate (Match m) {
                            return " JAHWE";
                        });
                    }

                    //if (sub.Level == 1) {
                    //    storyText = storyText.ToUpper();
                    //}

                    var run = new Run(DocumentBuilder.Document) {
                        Text = storyText
                    };
                    run.Font.Bold = true;

                    _par.AppendChild(run);
                }

                beginTable = true;
            }

            return beginTable;
        }

        private void AddVerse(Verse verse, bool beginTable = false, bool addFootnotes = true) {
            if (verse != null) {
                var previusTableWidth = GetLastTableSize(DocumentBuilder.Document);
                if (previusTableWidth == -1 || beginTable) {
                    DocumentBuilder.StartTable();
                    TableIsOpened = true;
                }
                else if (previusTableWidth == 0 || previusTableWidth > MaxWidth) {
                    EndTableAndAddNewParagraph();
                    DocumentBuilder.StartTable();
                    TableIsOpened = true;
                }

                var tableWidth = 0F;

                var nrCell = DocumentBuilder.InsertCell();
                nrCell.CellFormat.VerticalAlignment = CellVerticalAlignment.Bottom;

                DocumentBuilder.ParagraphFormat.ClearFormatting();
                DocumentBuilder.ParagraphFormat.Style = DocumentBuilder.Document.Styles[StyleIdentifier.Normal];
                DocumentBuilder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                DocumentBuilder.Font.Size = 14;
                DocumentBuilder.Font.Bold = true;
                DocumentBuilder.Font.Color = Color.Black;
                DocumentBuilder.Write(verse.NumberOfVerse + ".");

                var words = verse.VerseWords.OrderBy(x => x.NumberOfVerseWord).ToList();
                foreach (var verseWord in words) {
                    tableWidth = GetLastTableSize(DocumentBuilder.Document);

                    var verseWordText = "";
                    if (verseWord.Translation != null) {
                        verseWordText = verseWord.Translation.Replace("<n>", "").Replace("</n>", "").Replace("<i>", "").Replace("</i>", "").Replace("<b>", "").Replace("</b>", "");
                    }

                    if (tableWidth > MaxWidth || (tableWidth > SecureWidth && verseWordText.Length > InsecureCharacters)) {
                        EndTableAndAddNewParagraph();
                        DocumentBuilder.StartTable();
                        TableIsOpened = true;
                    }

                    DocumentBuilder.ParagraphFormat.ClearFormatting();
                    DocumentBuilder.Font.Bold = false;
                    DocumentBuilder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    DocumentBuilder.InsertCell();

                    if (verseWord.StrongCode != null) {
                        DocumentBuilder.Font.Size = 6F;
                        DocumentBuilder.Font.Color = Color.Brown;
                        DocumentBuilder.Writeln(verseWord.StrongCode.Topic);
                    }
                    if (verseWord.GrammarCode != null) {
                        DocumentBuilder.Font.Size = 5F;
                        DocumentBuilder.Font.Color = Color.DarkGray;
                        DocumentBuilder.Writeln(verseWord.GrammarCode.GrammarCodeVariant1);
                    }
                    if (verseWord.SourceWord != null) {
                        DocumentBuilder.Font.Size = 9F;
                        DocumentBuilder.Font.Color = Color.DarkGreen;
                        DocumentBuilder.Writeln(verseWord.SourceWord);
                    }
                    if (verseWord.Transliteration != null) {
                        DocumentBuilder.Font.Size = 8F;
                        DocumentBuilder.Font.Color = Color.DarkViolet;
                        DocumentBuilder.Writeln(verseWord.Transliteration);
                    }
                    if (verseWord.Translation != null) {
                        DocumentBuilder.Font.Size = 12F;
                        DocumentBuilder.Font.Color = Color.Black;
                        var translation = verseWord.Translation;
                        translation = translation.Replace(" ", "&nbsp;");
                        translation = translation.Replace("<n>", "<span style=\"color: #6c757d;\">").Replace("</n>", "</span>");
                        if (verseWord.WordOfJesus) {
                            translation = $"<span style=\"color: #990000;\">{translation}</span>";
                        }
                        DocumentBuilder.InsertHtml(translation, HtmlInsertOptions.RemoveLastEmptyParagraph);
                        AddFootnotes(addFootnotes, verseWord);
                    }
                }
            }
        }

        private void AddFootnotes(bool addFootnotes, VerseWord verseWord) {
            if (addFootnotes && verseWord.FootnoteText.IsNotNullOrEmpty()) {
                var footnoteText = verseWord.FootnoteText;
                if (footnoteText.IsNotNullOrEmpty()) {
                    var trans = verseWord.ParentVerse.ParentChapter.ParentBook.ParentTranslation;

                    if (BibleTagController.IsNull()) { this.BibleTagController = new BibleTagController(); }
                    if (TranslateModel.IsNull()) {
                        var uow = verseWord.Session as UnitOfWork;
                        var books = GetBookBases(uow);
                        TranslateModel = new TranslationControllerModel(trans,
                            verseWord.ParentVerse.ParentChapter.ParentBook.NumberOfBook.ToString(),
                            verseWord.ParentVerse.ParentChapter.NumberOfChapter.ToString(), null, books);
                        var view = new XPView(uow, typeof(Translation)) {
                            CriteriaString = $"[Books][[NumberOfBook] = '{verseWord.ParentVerse.ParentChapter.ParentBook.NumberOfBook}'] AND [Hidden] = 0"
                        };
                        view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
                        view.Properties.Add(new ViewProperty("Description", SortDirection.None, "[Description]", false, true));
                        view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
                        view.Properties.Add(new ViewProperty("Catholic", SortDirection.None, "[Catolic]", false, true));
                        view.Properties.Add(new ViewProperty("Recommended", SortDirection.None, "[Recommended]", false, true));
                        view.Properties.Add(new ViewProperty("OpenAccess", SortDirection.None, "[OpenAccess]", false, true));
                        foreach (ViewRecord item in view) {
                            TranslateModel.Translations.Add(new TranslationInfo() {
                                Name = item["Name"].ToString(),
                                Description = item["Description"].ToString(),
                                Type = (TranslationType)item["Type"],
                                Catholic = (bool)item["Catholic"],
                                Recommended = (bool)item["Recommended"],
                                PasswordRequired = !((bool)item["OpenAccess"])
                            });
                        }
                    }

                    footnoteText = BibleTagController.GetInternalVerseText(footnoteText, TranslateModel);
                    footnoteText = BibleTagController.GetInternalVerseListText(footnoteText, TranslateModel);
                    footnoteText = BibleTagController.GetInternalVerseRangeText(footnoteText, TranslateModel);
                    footnoteText = BibleTagController.GetMultiChapterRangeText(footnoteText, TranslateModel);
                    footnoteText = BibleTagController.GetExternalVerseText(footnoteText, TranslateModel);
                    footnoteText = BibleTagController.GetExternalVerseRangeText(footnoteText, TranslateModel);
                    var par = DocumentBuilder.CurrentParagraph;

                    DocumentBuilder.Font.Size = 10;
                    var footnote = DocumentBuilder.InsertFootnote(FootnoteType.Footnote, "");

                    DocumentBuilder.MoveTo(footnote.LastParagraph);
                    DocumentBuilder.InsertHtml($"<sup>)</sup>&nbsp;{footnoteText}");
                    foreach (Inline run in DocumentBuilder.CurrentParagraph.ChildNodes) {
                        run.Font.Size = 8;
                    }
                    DocumentBuilder.MoveTo(par);
                }
            }
        }

        private void EndTableAndAddNewParagraph() {
            if (TableIsOpened) {
                DocumentBuilder.EndRow();
                var table = DocumentBuilder.EndTable();
                table.ClearBorders();
                table.LastRow.RowFormat.AllowBreakAcrossPages = false;
                table.AutoFit(AutoFitBehavior.AutoFitToContents);
                TableIsOpened = false;
            }
            else {
                DocumentBuilder.MoveToDocumentEnd();
            }

            DocumentBuilder.Font.Size = 6;
            var par = DocumentBuilder.InsertParagraph();
            par.ParagraphFormat.ClearFormatting();
            par.ParagraphFormat.Style = DocumentBuilder.Document.Styles[StyleIdentifier.Normal];
            par.ParagraphFormat.SpaceAfter = 0;
            par.ParagraphFormat.SpaceBefore = 0;

        }

        public void Save(string fileName) {
            if (DocumentBuilder.Document != null) {
                DocumentBuilder.Document.Save(fileName);
            }
        }

        private float GetLastTableSize(Document document) {
            var table = document.LastSection.Body.Tables != null && document.LastSection.Body.Tables.Count > 0 ? document.LastSection.Body.Tables.Last() as Table : null;
            if (table != null && table.FirstRow != null) {
                if (table.FirstRow.Count > 20) {
                    // cośjest nie tak
                }
                table.AllowAutoFit = true;
                table.AutoFit(AutoFitBehavior.AutoFitToContents);

                document.UpdatePageLayout();

                LayoutEnumerator.Current = LayoutCollector.GetEntity(table.FirstRow.FirstCell.FirstParagraph);
                LayoutEnumerator.MoveParent(); // move to Line
                LayoutEnumerator.MoveParent(); // move to Cell

                var width = 0F;
                do {
                    if (LayoutEnumerator.Type == LayoutEntityType.Cell) {
                        width += LayoutEnumerator.Rectangle.Width;
                    }
                } while (LayoutEnumerator.MoveNext());

                return width;
            }

            return -1;
        }

        private List<BookBaseInfo> GetBookBases(UnitOfWork uow = null) {
            var result = new List<BookBaseInfo>();
            if (uow == null) { uow = new UnitOfWork(); }
            var books = new XPQuery<BookBase>(uow).ToList();
            foreach (var item in books) {
                result.Add(new BookBaseInfo() {
                    BookName = item.BookName,
                    BookShortcut = item.BookShortcut,
                    BookTitle = item.BookTitle,
                    Color = item.Color,
                    NumberOfBook = item.NumberOfBook,
                    StatusBiblePart = item.StatusBiblePart,
                    StatusBookType = item.StatusBookType,
                    StatusCanonType = item.StatusCanonType
                });
            }
            return result;
        }
        /*
        private float GetTextSize(VerseWord verseWord) {
            if (verseWord != null) {
                var withs = new List<float>();
                if (verseWord.StrongCode != null) {
                    withs.Add(GetTextSize(verseWord.StrongCode.Topic, 8F));
                }
                if (verseWord.GrammarCode != null) {
                    withs.Add(GetTextSize(verseWord.GrammarCode.GrammarCodeVariant1, 8F));
                }
                if (verseWord.SourceWord != null) {
                    withs.Add(GetTextSize(verseWord.SourceWord, 13F));
                }
                if (verseWord.Transliteration != null) {
                    withs.Add(GetTextSize(verseWord.Transliteration, 13F));
                }
                if (verseWord.Translation != null) {
                    withs.Add(GetTextSize(verseWord.Translation, 16F));
                }

                return withs.Max();
            }
            return 0;
        }

        private float GetTextSize(string text, double fontSize, FontStyle style = FontStyle.Regular) {
            var document = new Document();
            document.FirstSection.PageSetup.DifferentFirstPageHeaderFooter = true;

            document.FirstSection.PageSetup.PaperSize = PaperSize.A4;
            document.FirstSection.PageSetup.Orientation = Orientation.Portrait;
            document.FirstSection.PageSetup.TopMargin = (2.5F).CentimetersToPoints();
            document.FirstSection.PageSetup.LeftMargin = (1.5F).CentimetersToPoints();
            document.FirstSection.PageSetup.BottomMargin = (1.5F).CentimetersToPoints();
            document.FirstSection.PageSetup.RightMargin = (1.5F).CentimetersToPoints();

            var normalStyle = document.Styles[StyleIdentifier.Normal];
            if (normalStyle.IsNotNull()) {
                normalStyle.Font.Name = "Times New Roman";
                normalStyle.Font.Size = 11;
                normalStyle.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                normalStyle.Font.LocaleId = (int)EditingLanguage.Polish;
                normalStyle.Font.NoProofing = true;
            }

            Paragraph paragraph;
            if (document.FirstSection.Body.ChildNodes.Count > 0 && document.FirstSection.Body.ChildNodes[0].NodeType == NodeType.Paragraph) {
                paragraph = document.FirstSection.Body.ChildNodes[0] as Paragraph;
            }
            else {
                paragraph = new Paragraph(document);
                document.FirstSection.Body.ChildNodes.Add(paragraph);
            }

            var run = new Run(document, text);
            run.Font.Name = "Times New Roman";
            run.Font.Size = fontSize;
            run.Font.Bold = style == FontStyle.Bold;
            run.Font.Italic = style == FontStyle.Italic;
            paragraph.Runs.Add(run);

            document.UpdatePageLayout();

            LayoutEnumerator layoutEnumerator = new(document);
            layoutEnumerator.MoveParent(LayoutEntityType.Page);
            layoutEnumerator.Reset();
            float width = 0;
            TraverseLayoutForward(layoutEnumerator, 1, ref width);

            return width;
        }

        private void TraverseLayoutForward(LayoutEnumerator layoutEnumerator, int depth, ref float width) {
            do {
                if (layoutEnumerator.Type == LayoutEntityType.Span) {
                    width += layoutEnumerator.Rectangle.Width;
                }

                if (layoutEnumerator.MoveFirstChild()) {
                    TraverseLayoutForward(layoutEnumerator, depth + 1, ref width);
                    layoutEnumerator.MoveParent();
                }
            } while (layoutEnumerator.MoveNext());
        }
        */

    }
}
