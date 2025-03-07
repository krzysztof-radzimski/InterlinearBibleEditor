/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Export {
    public class InterlinearExporter : BaseExporter {
        private int footNoteIndex = 1;
        private int currentVerseNumber = 1;

        private Controllers.IBibleTagController BibleTagController;
        private TranslationControllerModel TranslateModel;

        private InterlinearExporter() : base() { }
        public InterlinearExporter(byte[] asposeLicense, string host) : base(asposeLicense, host) { }

        public void ExportBookTranslation(Book book, ExportSaveFormat saveFormat, string outputPath, bool addFooter = true, bool addBookAndHeader = true) {
            if (book.IsNull()) { throw new ArgumentNullException("book"); }
            if (outputPath.IsNullOrEmpty()) { throw new ArgumentNullException("outputPath"); }

            if (book.ParentTranslation.Type != TranslationType.Interlinear) { throw new Exception("Wrong translation type!"); }

            footNoteIndex = 1;

            var builder = GetDocumentBuilder();

            ExportBookName(book, builder);

            var chapters = book.Chapters.Where(x => x.IsTranslated).OrderBy(x => x.NumberOfChapter).ToArray();
            foreach (var chapter in chapters) {

                ExportChapterNumber(chapter, builder, false);

                Paragraph par = null;
                if (chapter.Subtitles.Count == 0) {
                    par = builder.InsertParagraph();
                    par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
                    par.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                }
                foreach (var item in chapter.Verses.OrderBy(x => x.NumberOfVerse)) {
                    ExportVerseTranslation(item, ref par, builder);
                }

                if (chapters.Last() == chapter && addFooter) { WriteFooter(book.ParentTranslation, par, builder); }
                else {
                    builder.MoveTo(par);
                }
            }


            if (addBookAndHeader) {
                builder.MoveToDocumentEnd();
                AddBookHeaderAndFooter(book, builder);
            }

            builder.MoveToDocumentEnd();

            SaveBuilder(saveFormat, outputPath, builder);
        }
        public byte[] ExportBookTranslation(Book book, ExportSaveFormat saveFormat, bool addFooter = true, bool addBookAndHeader = true) {
            if (book.IsNull()) { throw new ArgumentNullException("book"); }

            if (book.ParentTranslation.Type != TranslationType.Interlinear) { throw new Exception("Wrong translation type!"); }

            footNoteIndex = 1;

            var builder = GetDocumentBuilder();

            ExportBookName(book, builder);

            var chapters = book.Chapters.Where(x => x.IsTranslated).OrderBy(x => x.NumberOfChapter).ToArray();
            foreach (var chapter in chapters) {
                ExportChapterNumber(chapter, builder, false);

                Paragraph par = null;
                if (chapter.Subtitles.Count == 0) {
                    par = builder.InsertParagraph();
                    par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
                    par.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                }

                foreach (var item in chapter.Verses.OrderBy(x => x.NumberOfVerse)) {
                    ExportVerseTranslation(item, ref par, builder);
                }

                if (chapters.Last() == chapter && addFooter) { WriteFooter(book.ParentTranslation, par, builder); }
                else {
                    builder.MoveTo(par);
                    //builder.InsertBreak(BreakType.SectionBreakContinuous);
                }
            }


            if (addBookAndHeader) {
                builder.MoveToDocumentEnd();
                AddBookHeaderAndFooter(book, builder);
            }

            builder.MoveToDocumentEnd();

            return SaveBuilder(saveFormat, builder);
        }

        public void Export(Translation translation, ExportSaveFormat saveFormat, string outputPath, bool addFooter = false) {
            if (translation.IsNull()) { throw new ArgumentNullException("translation"); }
            if (outputPath.IsNullOrEmpty()) { throw new ArgumentNullException("outputPath"); }

            if (translation.Type != TranslationType.Interlinear) { throw new Exception("Wrong translation type!"); }

            var builder = GetDocumentBuilder();
            var isFirstBook = true;

            foreach (var book in translation.Books.OrderBy(x => x.NumberOfBook)) {
                if (!book.IsTranslated) { continue; }
                if (!isFirstBook) {
                    builder.InsertParagraph();
                    builder.ParagraphFormat.ClearFormatting();
                }
                else {
                    isFirstBook = false;
                }

                ExportBookName(book, builder);

                var chapters = book.Chapters.Where(x => x.IsTranslated).OrderBy(x => x.NumberOfChapter).ToArray();
                foreach (var chapter in chapters) {
                    ExportChapterNumber(chapter, builder, false);

                    Paragraph par = null;
                    if (chapter.Subtitles.Count == 0) {
                        par = builder.InsertParagraph();
                        par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
                        par.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    }

                    foreach (var item in chapter.Verses.OrderBy(x => x.NumberOfVerse)) {
                        ExportVerse(item, ref par, builder);
                    }

                    if (par != null) { builder.MoveTo(par); }
                }
            }

            if (addFooter) {
                builder.MoveToDocumentEnd();
                var _par = builder.InsertParagraph();
                WriteFooter(translation, _par, builder);
            }

            builder.MoveToDocumentEnd();

            SaveBuilder(saveFormat, outputPath, builder);
        }

        public byte[] Export(Translation translation, ExportSaveFormat saveFormat, bool addFooter = false) {
            if (translation.IsNull()) { throw new ArgumentNullException("translation"); }

            if (translation.Type != TranslationType.Interlinear) { throw new Exception("Wrong translation type!"); }

            var builder = GetDocumentBuilder();
            var isFirstBook = true;

            foreach (var book in translation.Books.OrderBy(x => x.NumberOfBook)) {
                if (!book.IsTranslated) { continue; }
                if (!isFirstBook) {
                    builder.InsertParagraph();
                    builder.ParagraphFormat.ClearFormatting();
                }
                else {
                    isFirstBook = false;
                }

                ExportBookName(book, builder);

                var chapters = book.Chapters.Where(x => x.IsTranslated).OrderBy(x => x.NumberOfChapter).ToArray();
                foreach (var chapter in chapters) {
                    ExportChapterNumber(chapter, builder, false);

                    Paragraph par = null;
                    if (chapter.Subtitles.Count == 0) {
                        par = builder.InsertParagraph();
                        par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
                        par.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                    }

                    foreach (var item in chapter.Verses.OrderBy(x => x.NumberOfVerse)) {
                        ExportVerse(item, ref par, builder);
                    }

                    if (par != null) { builder.MoveTo(par); }
                }
            }

            if (addFooter) {
                builder.MoveToDocumentEnd();
                var _par = builder.InsertParagraph();
                WriteFooter(translation, _par, builder);
            }

            builder.MoveToDocumentEnd();
            return SaveBuilder(saveFormat, builder);
        }

        public void Export(Book book, ExportSaveFormat saveFormat, string outputPath, bool addFooter = true, bool addBookAndHeader = true) {
            if (book.IsNull()) { throw new ArgumentNullException("book"); }
            if (outputPath.IsNullOrEmpty()) { throw new ArgumentNullException("outputPath"); }

            if (book.ParentTranslation.Type != TranslationType.Interlinear) { throw new Exception("Wrong translation type!"); }

            var builder = GetDocumentBuilder();

            ExportBookName(book, builder);

            var chapters = book.Chapters.Where(x => x.IsTranslated).OrderBy(x => x.NumberOfChapter).ToArray();
            foreach (var chapter in chapters) {
                ExportChapterNumber(chapter, builder, false);

                Paragraph par = null;
                if (chapter.Subtitles.Count == 0) {
                    par = builder.InsertParagraph();
                    par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
                    par.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                }

                foreach (var item in chapter.Verses.OrderBy(x => x.NumberOfVerse)) {
                    ExportVerse(item, ref par, builder);
                }

                if (chapters.Last() == chapter && addFooter) { WriteFooter(book.ParentTranslation, par, builder); }
                else {
                    builder.MoveTo(par);
                }
            }

            if (addBookAndHeader) {
                builder.MoveToDocumentEnd();
                AddBookHeaderAndFooter(book, builder);
            }

            builder.MoveToDocumentEnd();

            SaveBuilder(saveFormat, outputPath, builder);
        }
        public byte[] Export(Book book, ExportSaveFormat saveFormat, bool addFooter = true, bool addBookAndHeader = true) {
            if (book.IsNull()) { throw new ArgumentNullException("book"); }

            if (book.ParentTranslation.Type != TranslationType.Interlinear) { throw new Exception("Wrong translation type!"); }

            var builder = GetDocumentBuilder();

            ExportBookName(book, builder);

            var chapters = book.Chapters.Where(x => x.IsTranslated).OrderBy(x => x.NumberOfChapter).ToArray();
            foreach (var chapter in chapters) {
                ExportChapterNumber(chapter, builder, false);

                Paragraph par = null;
                if (chapter.Subtitles.Count == 0) {
                    par = builder.InsertParagraph();
                    par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
                    par.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                }

                foreach (var item in chapter.Verses.OrderBy(x => x.NumberOfVerse)) {
                    ExportVerse(item, ref par, builder);
                }

                if (chapters.Last() == chapter && addFooter) { WriteFooter(book.ParentTranslation, par, builder); }
                else {
                    builder.MoveTo(par);
                }
            }

            if (addBookAndHeader) {
                builder.MoveToDocumentEnd();
                AddBookHeaderAndFooter(book, builder);
            }

            builder.MoveToDocumentEnd();

            return SaveBuilder(saveFormat, builder);
        }
        public void Export(Chapter chapter, ExportSaveFormat saveFormat, string outputPath, bool addFooter = true, bool addChapterHeaderAndFooter = true) {
            if (chapter.IsNull()) { throw new ArgumentNullException("chapter"); }
            if (outputPath.IsNullOrEmpty()) { throw new ArgumentNullException("outputPath"); }

            if (chapter.ParentTranslation.Type != TranslationType.Interlinear) { throw new Exception("Wrong translation type!"); }

            var builder = GetDocumentBuilder();

            ExportChapterNumber(chapter, builder, true);

            Paragraph par = null;
            if (chapter.Subtitles.Count == 0) {
                par = builder.InsertParagraph();
                par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
                par.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            }

            foreach (var item in chapter.Verses.OrderBy(x => x.NumberOfVerse)) {
                ExportVerse(item, ref par, builder);
            }

            if (addFooter) WriteFooter(chapter.ParentTranslation, par, builder);
            if (addChapterHeaderAndFooter) AddChapterHeaderAndFooter(chapter, builder);

            SaveBuilder(saveFormat, outputPath, builder);
        }
        public byte[] Export(Chapter chapter, ExportSaveFormat saveFormat, bool addFooter = true, bool addChapterHeaderAndFooter = true) {
            if (chapter.IsNull()) { throw new ArgumentNullException("chapter"); }
            if (chapter.ParentTranslation.Type != TranslationType.Interlinear) { throw new Exception("Wrong translation type!"); }

            var builder = GetDocumentBuilder();
            ExportChapterNumber(chapter, builder, true);

            Paragraph par = null;
            if (chapter.Subtitles.Count == 0) {
                par = builder.InsertParagraph();
                par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
                par.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            }

            foreach (var item in chapter.Verses.OrderBy(x => x.NumberOfVerse)) {
                ExportVerse(item, ref par, builder);
            }

            if (addFooter) WriteFooter(chapter.ParentTranslation, par, builder);
            if (addChapterHeaderAndFooter) AddChapterHeaderAndFooter(chapter, builder);

            return SaveBuilder(saveFormat, builder);
        }

        private void ExportVerseTranslation(Verse verse, ref Paragraph par, DocumentBuilder builder) {
            var footNotes = new Dictionary<int, string>();

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

                        if (subtitle.Level == 1) {
                            storyText = storyText.ToUpper();
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
            if (text.Contains("<n>") && text.Contains("*")) {
                var footNoteTextPattern = GetFootnotesPattern();

                text = Regex.Replace(text, footNoteTextPattern, delegate (Match m) {
                    if (m.Groups != null && m.Groups.Count > 0) {
                        for (var i = 1; i < 8; i++) {
                            var groupName = $"f{i}";
                            if (m.Groups[groupName] != null && m.Groups[groupName].Success) {
                                var groupValue = m.Groups[groupName].Value;
                                footNotes.Add(footNoteIndex, groupValue);
                                footNoteIndex++;
                            }
                        }
                    }

                    var result = String.Empty;
                    return result;
                }, RegexOptions.IgnoreCase);
            }

            text = FormatVerseText(text, book.BaseBook.Status.BiblePart);

            //// Słowa Jezusa
            //text = text.Replace("<J>", @"<span style=""color: darkred;"">").Replace("</J>", "</span>");

            //// Elementy dodane
            //text = text.Replace("<n>", @"<span style=""color: darkgray;"">").Replace("</n>", "</span>");

            //text = text.Replace("<pb/>", "").Replace("<t>", "").Replace("</t>", "").Replace("<e>", "").Replace("</e>", "");

            //// zamiana na imię Boże
            //if (book.BaseBook.Status.BiblePart == BiblePart.OldTestament) {
            //    text = Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>PAN(A)?(EM)?(U)?(IE)?)[\s\,\.\:\""\'\”ʼ]", delegate (Match m) {
            //        var prefix = m.Groups["prefix"].Value;
            //        return $"{prefix}JAHWE{m.Value.Last()}";
            //    });
            //}
            //if (book.BaseBook.Status.BiblePart == BiblePart.OldTestament) {
            //    text = Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>JHWH)[\s\,\.\:\""\'\”ʼ]", delegate (Match m) {
            //        var prefix = m.Groups["prefix"].Value;
            //        return $"{prefix}JAHWE{m.Value.Last()}";
            //    });
            //}
            //if (book.BaseBook.Status.BiblePart == BiblePart.OldTestament) {
            //    text = Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>Jehow(a)?(y)?(ie)?(ę)?(o)?)[\s\,\.\:\""\'\”ʼ]", delegate (Match m) {
            //        var prefix = m.Groups["prefix"].Value;
            //        return $"{prefix}JAHWE{m.Value.Last()}";
            //    });
            //}
            //if (book.BaseBook.Status.BiblePart == BiblePart.NewTestament) {
            //    text = Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>Jehow(?<ending>(a)?(y)?(ie)?(ę)?(o)?))[\s\,\.\:\""\'\”ʼ]", delegate (Match m) {
            //        var prefix = m.Groups["prefix"].Value;
            //        var ending = m.Groups["ending"].Value;
            //        var root = "Pan";
            //        if (ending == "ie") { root += "u"; }
            //        if (ending == "o") { root += "ie"; }
            //        if (ending == "y" || ending == "ę") { root += "a"; }
            //        return $"{prefix}{root}{m.Value.Last()}";
            //    });
            //}

            //// usuwam sierotki
            //text = Regex.Replace(text, @"[\s\(\,\;][a,i,o,w,z]\s", delegate (Match m) {
            //    return " " + m.Value.Trim() + "&nbsp;";
            //}, RegexOptions.IgnoreCase);

            //// usuwam puste przypisy
            //text = Regex.Replace(text, @"\[[0-9]+\]", delegate (Match m) {
            //    return String.Empty;
            //}, RegexOptions.IgnoreCase);


            builder.CurrentParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            builder.CurrentParagraph.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
            builder.CurrentParagraph.ParagraphFormat.LineSpacing = 18;
            builder.CurrentParagraph.ParagraphFormat.KeepWithNext = false;

            builder.InsertHtml($"<b>{verse.NumberOfVerse}</b>.&nbsp;");

            builder.InsertHtml($"{text}");
            if (footNotes.Count > 0) {
                foreach (var item in footNotes) {
                    var fn = builder.InsertFootnote(FootnoteType.Footnote, item.Value, $"{item.Key})");
                    fn.Font.Bold = true;
                    fn.Font.Color = Color.Black;
                }
            }
            builder.Write(" ");
        }

        private void AddBookHeaderAndFooter(Book book, DocumentBuilder builder) {
            var bookTitle = book.BaseBook.BookTitle;
            var translationName = book.ParentTranslation.Description;

            builder.MoveToHeaderFooter(HeaderFooterType.HeaderPrimary);
            builder.InsertHtml($"<div style=\"font-size: 9; text-align: left; width: 100%; border-bottom: solid 1px darkgray;\">{translationName}<br/>{bookTitle}</div>");

            builder.MoveToHeaderFooter(HeaderFooterType.FooterPrimary);
            builder.CurrentParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            builder.Font.Size = 9;
            builder.Write("Strona ");
            builder.InsertField("PAGE", null);
            builder.Write(" z ");
            builder.InsertField("NUMPAGES", null);
        }
        private void AddChapterHeaderAndFooter(Chapter chapter, DocumentBuilder builder) {
            var chapterString = chapter.ParentBook.NumberOfBook == 230 ? chapter.ParentTranslation.ChapterPsalmString : chapter.ParentTranslation.ChapterString;
            var chapterNumber = chapter.ParentTranslation.ChapterRomanNumbering ? chapter.NumberOfChapter.ArabicToRoman() : chapter.NumberOfChapter.ToString();
            var bookTitle = chapter.ParentBook.BaseBook.BookTitle;
            var translationName = chapter.ParentTranslation.Description;

            builder.MoveToHeaderFooter(HeaderFooterType.HeaderPrimary);
            builder.InsertHtml($"<div style=\"font-size: 9; text-align: left; width: 100%; border-bottom: solid 1px darkgray;\">{translationName}<br/>{bookTitle} {chapterString} {chapterNumber}</div>");

            builder.MoveToHeaderFooter(HeaderFooterType.FooterPrimary);
            builder.CurrentParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            builder.Font.Size = 9;
            builder.Write("Strona ");
            builder.InsertField("PAGE", null);
            builder.Write(" z ");
            builder.InsertField("NUMPAGES", null);
        }
        private void WriteFooter(Translation translation, Paragraph par, DocumentBuilder builder) {
            if (par.IsNotNull()) builder.MoveTo(par);

            builder.InsertParagraph();

            builder.InsertHtml($"<div style=\"font-size: 11; text-align: left;\">{translation.DetailedInfo}</div>");
        }
        private void ExportBookName(Book book, DocumentBuilder builder) {
            builder.CurrentParagraph.ParagraphFormat.Style = builder.Document.Styles["Nagłówek 1"];
            builder.CurrentParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Center;

            builder.Write(book.BaseBook.BookTitle);
        }
        private void ExportChapterNumber(Chapter chapter, DocumentBuilder builder, bool withBookName = false) {
            if (withBookName) {
                ExportBookName(chapter.ParentBook, builder);
            }

            if (chapter.ParentBook.NumberOfChapters == 1) { return; }

            var par = builder.InsertParagraph();
            par.ParagraphFormat.Style = builder.Document.Styles["Nagłówek 2"];
            par.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            par.ParagraphFormat.KeepWithNext = true;

            if (chapter.NumberOfChapter > 0) {
                var chapterString = chapter.ParentBook.NumberOfBook == 230 ? chapter.ParentTranslation.ChapterPsalmString : chapter.ParentTranslation.ChapterString;
                var chapterNumber = chapter.ParentTranslation.ChapterRomanNumbering ? chapter.NumberOfChapter.ArabicToRoman() : chapter.NumberOfChapter.ToString();
                builder.Write($"{chapterString} {chapterNumber}".Trim());
            }
            else {
                builder.Write($"Prolog");
            }
        }
        private void ExportVerse(Verse verse, ref Paragraph par, DocumentBuilder builder) {
            if (verse.ParentChapter.Subtitles.Count > 0) {
                var subtitles = verse.ParentChapter.Subtitles.Where(x => x.BeforeVerseNumber == verse.NumberOfVerse).OrderBy(x => x.Level); ;
                if (subtitles.Any()) {
                    foreach (var subtitle in subtitles) {
                        var _par = builder.InsertParagraph();
                        _par.ParagraphFormat.Style = subtitle.Level == 1 ? builder.Document.Styles["Nagłówek 3"] : builder.Document.Styles["Nagłówek 4"];
                        _par.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                        _par.ParagraphFormat.KeepWithNext = true;

                        var storyText = subtitle.Text;
                        if (subtitle.Level == 1) {
                            storyText = storyText.ToUpper();
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

            if (par == null) {
                par = builder.InsertParagraph();
                par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
                par.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                par.ParagraphFormat.KeepWithNext = false;
                builder.MoveTo(par);
            }

            if (verse.ParentChapter.NumberOfChapter != 0) { ExportVerseNumber(verse, par, builder); }
            currentVerseNumber = verse.NumberOfVerse;
            foreach (var item in verse.VerseWords.OrderBy(x => x.NumberOfVerseWord)) {
                ExportVerseWord(item, par, builder);
            }

            builder.MoveTo(par);
        }
        private void ExportVerseNumber(Verse verse, Paragraph par, DocumentBuilder builder) {
            var chapterNumber = verse.ParentTranslation.ChapterRomanNumbering ? verse.ParentChapter.NumberOfChapter.ArabicToRoman() : verse.ParentChapter.NumberOfChapter.ToString();
            var text = $"{chapterNumber}:{verse.NumberOfVerse}";
            var shape = new Shape(builder.Document, ShapeType.TextBox) {
                Width = 30,
                Height = 80,
                BehindText = false,
                Stroked = false,
                WrapType = WrapType.Inline
            };
            shape.TextBox.FitShapeToText = true;
            shape.TextBox.TextBoxWrapMode = TextBoxWrapMode.None;

            var shapePar = new Paragraph(builder.Document);
            shape.AppendChild(shapePar);

            builder.MoveTo(shape.FirstParagraph);
            builder.Font.Size = 11;
            builder.Font.Bold = true;
            builder.Font.Color = Color.Black;
            builder.Write(text);

            (shape.FirstParagraph.Runs.First() as Run).Font.Position = 24;

            par.AppendChild(shape);
            shape.Width = PexelsToPoints(GetMaxTextSize(text, font11bold));//1.2F;
        }
        private void ExportVerseWord(VerseWord word, Paragraph par, DocumentBuilder builder) {
            var doc = par.Document;
            var shape = new Shape(doc, ShapeType.TextBox) {
                Width = 50,
                Height = 80,
                BehindText = false,
                Stroked = false,
                WrapType = WrapType.Inline,
                FillColor = Color.Transparent
            };
            shape.TextBox.FitShapeToText = true;
            shape.TextBox.TextBoxWrapMode = TextBoxWrapMode.Square;

            var shapePar = new Paragraph(doc);
            shape.AppendChild(shapePar);

            builder.MoveTo(shape.FirstParagraph);

            if (word.ParentVerse.ParentChapter.ParentBook.ParentTranslation.WithStrongs) {
                builder.Font.Size = 6;
                builder.Font.Bold = false;
                builder.Font.Italic = false;
                builder.Font.Color = Color.DarkBlue;
                if (word.StrongCode.IsNotNull() && word.StrongCode.Topic.IsNotNullOrEmpty()) { builder.Write(word.StrongCode.Topic); } else { builder.Write("–"); }
                builder.InsertBreak(BreakType.LineBreak);
            }
            if (word.ParentVerse.ParentChapter.ParentBook.ParentTranslation.WithGrammarCodes) {
                builder.Font.Size = 6;
                builder.Font.Bold = false;
                builder.Font.Italic = false;
                builder.Font.Color = Color.DarkBlue;
                if (word.GrammarCode.IsNotNull() && word.GrammarCode.GrammarCodeVariant1.IsNotNullOrEmpty()) { builder.Write(word.GrammarCode.GrammarCodeVariant1); } else { builder.Write("–"); }
                builder.InsertBreak(BreakType.LineBreak);
            }

            builder.Font.Size = 10;
            builder.Font.Bold = false;
            builder.Font.Italic = false;
            builder.Font.Color = Color.DarkGreen;
            builder.Write(word.SourceWord != null ? word.SourceWord : "–");
            builder.InsertBreak(BreakType.LineBreak);

            builder.Font.Size = 9;
            builder.Font.Bold = false;
            builder.Font.Italic = false;
            builder.Font.Color = Color.MidnightBlue;
            builder.Write(word.Transliteration != null ? word.Transliteration : "–");
            builder.InsertBreak(BreakType.LineBreak);

            builder.Font.Size = 11;

            if (word.Translation.IsNotNull()) {

                if (word.Translation.Contains("<n>")) {
                    var translation = word.Translation.Replace("<n>", "<span style=\"color: #6c757d; font-size: 11pt;\">").Replace("</n>", "</span>");
                    if (word.WordOfJesus) {
                        translation = $"<span style=\"color: #990000;\">{translation}</span>";
                    }
                    if (word.Citation) {
                        translation = $"<i>{translation}</i>";
                    }
                    builder.InsertHtml(translation);
                }
                else {
                    builder.Font.Italic = word.Citation;
                    builder.Font.Color = word.WordOfJesus ? Color.DarkRed : Color.Black;
                    builder.Write(word.Translation);
                }
            }
            else {
                builder.Write("–");
            }

            par.AppendChild(shape);

            shape.Width = GetMaxTextSize(word);

            if (word.FootnoteText.IsNotNullOrEmpty()) {
                var footnoteText = word.FootnoteText;
                if (footnoteText.IsNotNullOrEmpty()) {
                    var translation = word.ParentVerse.ParentChapter.ParentBook.ParentTranslation;

                    if (BibleTagController.IsNull()) { this.BibleTagController = new Controllers.BibleTagController(); }
                    if (TranslateModel.IsNull()) {
                        var uow = word.Session as UnitOfWork;
                        var books = GetBookBases(uow);
                        TranslateModel = new TranslationControllerModel(translation,
                            word.ParentVerse.ParentChapter.ParentBook.NumberOfBook.ToString(),
                            word.ParentVerse.ParentChapter.NumberOfChapter.ToString(), null, books);
                        var view = new XPView(uow, typeof(Translation)) {
                            CriteriaString = $"[Books][[NumberOfBook] = '{word.ParentVerse.ParentChapter.ParentBook.NumberOfBook}'] AND [Hidden] = 0"
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

                    footnoteText = BibleTagController.GetInternalVerseHtml(footnoteText, TranslateModel);
                    footnoteText = BibleTagController.GetInternalVerseListHtml(footnoteText, TranslateModel);
                    footnoteText = BibleTagController.GetInternalVerseRangeHtml(footnoteText, TranslateModel);
                    footnoteText = BibleTagController.GetMultiChapterRangeHtml(footnoteText, TranslateModel);
                    footnoteText = BibleTagController.GetExternalVerseHtml(footnoteText, TranslateModel);
                    footnoteText = BibleTagController.GetExternalVerseRangeHtml(footnoteText, TranslateModel);
                    footnoteText = RepairImagesPath(footnoteText);
                    footnoteText = RepairHrefPath(footnoteText);

                    builder.MoveTo(par);

                    var footnoteMark = $"{currentVerseNumber}{word.NumberOfVerseWord.ToAlphabetString()}";
                    var footnote = builder.InsertFootnote(FootnoteType.Footnote, "", $"{footnoteMark})");
                    footnote.Font.Position = 12;
                    footnote.Font.Color = Color.Black;
                    footnote.Font.Bold = true;

                    builder.MoveTo(footnote.LastParagraph);
                    builder.InsertHtml($"<sup>)</sup>&nbsp;{footnoteText}");
                    foreach (Inline run in builder.CurrentParagraph.GetChildNodes(NodeType.Run, true)) {
                        run.Font.Size = 8;
                    }
                    builder.MoveTo(par);
                }
            }
        }
        private string RepairImagesPath(string input) {
            input = Regex.Replace(input, @"\<img src\=\""\/", $@"<img src=""{Host}/");
            return input;
        }
        private string RepairHrefPath(string input) {
            input = Regex.Replace(input, @"\<a href\=\""\/", $@"<a href=""{Host}/");
            return input;
        }
        private float GetMaxTextSize(VerseWord word) {
            if (word.IsNotNull()) {
                var list = new List<float> {
                    g.MeasureString(word.SourceWord, font10).Width,
                    g.MeasureString(word.Transliteration, font10).Width,
                    g.MeasureString(word.Translation.ReplaceAnyWith(" ", "<n>","</n>"), word.Citation ? font11bold : font11).Width
                };
                if (word.GrammarCode.IsNotNull()) { list.Add(g.MeasureString(word.GrammarCode.GrammarCodeVariant1, font10).Width); }
                if (word.StrongCode.IsNotNull()) { list.Add(g.MeasureString(word.StrongCode.Topic, font10).Width); }

                return PexelsToPoints(list.Max()); //+ 1.3F; //* 1.1F;
            }
            return default;
        }
        private float GetMaxTextSize(string text, System.Drawing.Font font) {
            return g.MeasureString(text, font).Width;
        }
        private float PexelsToPoints(float pixels) {
            var points = pixels * 72 / 96;
            return points + 11F;
        }
    }
}
