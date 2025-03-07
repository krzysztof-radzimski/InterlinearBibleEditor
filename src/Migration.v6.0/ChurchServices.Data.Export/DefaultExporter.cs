/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using ChurchServices.Data.Export.Controllers;

namespace ChurchServices.Data.Export {
    public class DefaultExporter : BaseDefaultExporter {
        private int footNoteIndex = 1;
        private TranslationControllerModel TranslateModel;
        private BibleTagController BibleTagController;
        private DefaultExporter() : base() {

        }
        public DefaultExporter(byte[] asposeLicense, string host) : base(asposeLicense, host) { }

        protected override void ExportVerse(Verse verse, ref Paragraph par, DocumentBuilder builder) {
            Initialize(verse);

            var footNotes = new List<FootnoteIndexInfo>();

            var book = verse.ParentChapter.ParentBook;
            if (verse.ParentChapter.Subtitles.Count > 0) {
                var subtitles = verse.ParentChapter.Subtitles.Where(x => x.BeforeVerseNumber == verse.NumberOfVerse).OrderBy(x => x.Level);
                if (subtitles.Any()) {
                    foreach (var subtitle in subtitles) {
                        var _par = builder.InsertParagraph();
                        _par.ParagraphFormat.Style = subtitle.Level == 1 ? builder.Document.Styles["Nagłówek 3"] : builder.Document.Styles["Nagłówek 4"];
                        _par.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                        _par.ParagraphFormat.KeepWithNext = true;

                        var storyText = subtitle.Text;
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

                        if (book.BaseBook.Status.BiblePart == BiblePart.OldTestament) {
                            storyText = Regex.Replace(storyText, @"\sPAN(A)?(EM)?(U)?(IE)?", delegate (Match m) {
                                return " JAHWE";
                            });
                        }

                        var run = new Run(builder.Document) {
                            Text = storyText
                        };
                        run.Font.Bold = true;

                        _par.AppendChild(run);
                    }

                    par = builder.InsertParagraph();
                    par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
                    par.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    par.ParagraphFormat.KeepWithNext = false;
                    builder.MoveTo(par);
                }
            }


            var text = " " + verse.Text;

            // text contains footnotes
            if (Regex.IsMatch(text, @"\<n\>(\s+)?\[\*")) {
                var lastIndex = Regex.Matches(text, @"\<n\>(\s+)?\[\*").Last().Index;
                var table = new string[] { text.Substring(0, lastIndex), text.Substring(lastIndex) };

                var verseText = table[0].Trim();
                var footnotesText = table[1].Trim();
                var footNoteTextPattern = GetFootnotesPattern();
                footnotesText = Regex.Replace(footnotesText, footNoteTextPattern, delegate (Match m) {
                    if (m.Groups != null && m.Groups.Count > 0) {
                        for (var i = 1; i < 8; i++) {
                            var groupName = $"f{i}";
                            if (m.Groups[groupName] != null && m.Groups[groupName].Success) {
                                var groupValue = m.Groups[groupName].Value;
                                if (groupValue.Contains("<x>")) {
                                    groupValue = BibleTagController.GetInternalVerseRangeText(groupValue, TranslateModel);
                                    groupValue = BibleTagController.GetInternalVerseText(groupValue, TranslateModel);
                                    groupValue = BibleTagController.GetExternalVerseRangeText(groupValue, TranslateModel);
                                    groupValue = BibleTagController.GetExternalVerseText(groupValue, TranslateModel);
                                    groupValue = BibleTagController.GetInternalVerseListText(groupValue, TranslateModel);
                                    groupValue = BibleTagController.GetMultiChapterRangeText(groupValue, TranslateModel);
                                }
                                footNotes.Add(new FootnoteIndexInfo() { Index = footNoteIndex, Value = groupValue, IndexInVerse = i });
                                footNoteIndex++;
                            }
                        }
                    }

                    var result = String.Empty;
                    return result;
                }, RegexOptions.IgnoreCase);

                builder.InsertHtml($"<b>{verse.NumberOfVerse}</b>.&nbsp;");
                var fragments = Regex.Split(verseText, @"(\*)+").ToArray();
                var numInVerse = 1;
                for (int i = 0; i < fragments.Length; i++) {
                    var fragment = fragments[i];
                    if (Regex.IsMatch(fragment, @"(\*)+")) { continue; }
                    if (footNotes.Count <= i) {
                        fragment = FormatVerseText(fragment, book.BaseBook.Status.BiblePart);
                        builder.InsertHtml($"{fragment}", HtmlInsertOptions.RemoveLastEmptyParagraph);
                    }
                    else {
                        fragment = FormatVerseText(fragment, book.BaseBook.Status.BiblePart);
                        builder.InsertHtml($"{fragment}", HtmlInsertOptions.RemoveLastEmptyParagraph);

                        var footnoteText = footNotes.Where(x => x.IndexInVerse == numInVerse).FirstOrDefault();
                        if (footnoteText != null) {
                            var _par = builder.CurrentParagraph;
                            builder.Font.Size = 10;
                            var footnoteMark = $"{verse.NumberOfVerse}{numInVerse.ToAlphabetString()}";
                            var footnote = builder.InsertFootnote(FootnoteType.Footnote, "", $"{footnoteMark})");
                            builder.Write(" ");

                            builder.MoveTo(footnote.LastParagraph);
                            builder.InsertHtml($"&nbsp;{footnoteText.Value}");
                            foreach (Inline run in builder.CurrentParagraph.GetChildNodes(NodeType.Run, true)) {
                                run.Font.Size = 8;
                            }
                            builder.MoveTo(_par);

                            numInVerse++;
                        }
                    }
                }
                builder.Write(" ");
            }
            else {
                text = FormatVerseText(text, book.BaseBook.Status.BiblePart);

                builder.CurrentParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
                builder.CurrentParagraph.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
                builder.CurrentParagraph.ParagraphFormat.LineSpacing = 18;

                builder.InsertHtml($"<b>{verse.NumberOfVerse}</b>.&nbsp;");

                builder.InsertHtml($"{text}");
                if (footNotes.Count > 0) {
                    foreach (var item in footNotes) {
                        builder.InsertFootnote(FootnoteType.Footnote, item.Value, $"{item.Index})");
                        builder.Write(" ");
                    }
                }
                builder.Write(" ");
            }
        }

        private void Initialize(Verse verse) {
            if (BibleTagController.IsNull()) { this.BibleTagController = new BibleTagController(); }
            if (TranslateModel == null) {
                var uow = verse.Session as UnitOfWork;
                var books = GetBookBases(uow);
                var translate = verse.ParentTranslation;
                var numberOfChapter = verse.ParentChapter.NumberOfChapter.ToString();
                var numberOfBook = verse.ParentChapter.ParentBook.NumberOfBook.ToString();
                if (verse.ParentChapter.ParentBook.BaseBook.StatusBookType != TheBookType.Bible) {
                    translate = new XPQuery<Translation>(uow).Where(x => x.Name == "BW").FirstOrDefault();
                    numberOfChapter = "1";
                    numberOfBook = "10";
                }
                TranslateModel = new TranslationControllerModel(translate, numberOfBook, numberOfChapter, null, books);
                var view = new XPView(uow, typeof(Translation)) {
                    CriteriaString = $"[Books][[NumberOfBook] = '{verse.ParentChapter.ParentBook.NumberOfBook}'] AND [Hidden] = 0"
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
        }
    }

    class FootnoteIndexInfo {
        public int Index { get; set; }
        public int IndexInVerse { get; set; }
        public string Value { get; set; }
    }
}
