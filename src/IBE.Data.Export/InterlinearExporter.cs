using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Loading;
using Aspose.Words.Notes;
using Aspose.Words.Saving;
using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace IBE.Data.Export {
    public class InterlinearExporter {
        private System.Drawing.Graphics g;
        private System.Drawing.Font font11;
        private System.Drawing.Font font11bold;
        private System.Drawing.Font font10;
        private System.Drawing.Font font9;
        private System.Drawing.Font font8;

        private InterlinearExporter() { }
        public InterlinearExporter(byte[] asposeLicense) : this() {
            if (asposeLicense.IsNotNull()) {
                new License().SetLicense(new MemoryStream(asposeLicense));
            }
            var img = new Bitmap(1, 1);
            g = System.Drawing.Graphics.FromImage(img);
            font8 = new System.Drawing.Font("Times New Roman", 8F, FontStyle.Regular);
            font9 = new System.Drawing.Font("Times New Roman", 9F, FontStyle.Regular);
            font10 = new System.Drawing.Font("Times New Roman", 10F, FontStyle.Regular);
            font11 = new System.Drawing.Font("Times New Roman", 11F, FontStyle.Regular);
            font11bold = new System.Drawing.Font("Times New Roman", 11F, FontStyle.Bold);
        }

        public void Export(Translation translation, ExportSaveFormat saveFormat, string outputPath) {
            if (translation.IsNull()) { throw new ArgumentNullException("translation"); }
            if (outputPath.IsNullOrEmpty()) { throw new ArgumentNullException("outputPath"); }

            if (translation.Type != TranslationType.Interlinear) { throw new Exception("Wrong translation type!"); }

            var builder = GetDocumentBuilder();
            SaveBuilder(saveFormat, outputPath, builder);
        }

        public void Export(Book book, ExportSaveFormat saveFormat, string outputPath, bool addFooter = true, bool addBookAndHeader = true) {
            if (book.IsNull()) { throw new ArgumentNullException("book"); }
            if (outputPath.IsNullOrEmpty()) { throw new ArgumentNullException("outputPath"); }

            if (book.ParentTranslation.Type != TranslationType.Interlinear) { throw new Exception("Wrong translation type!"); }

            var builder = GetDocumentBuilder();

            ExportBookName(book, builder);

            var chapters = book.Chapters.Where(x => x.IsTranslated).OrderBy(x => x.NumberOfChapter).ToArray();
            foreach (var chapter in chapters) {

                if (chapter == chapters.First()) { builder.InsertParagraph(); }

                ExportChapterNumber(chapter, builder, false);

                var par = builder.InsertParagraph();
                par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
                par.ParagraphFormat.Alignment = ParagraphAlignment.Left;

                foreach (var item in chapter.Verses.OrderBy(x => x.NumberOfVerse)) {
                    ExportVerse(item, ref par, builder);
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

                var par = builder.InsertParagraph();
                par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
                par.ParagraphFormat.Alignment = ParagraphAlignment.Left;

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

            var par = builder.InsertParagraph();
            par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
            par.ParagraphFormat.Alignment = ParagraphAlignment.Left;

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

            var par = builder.InsertParagraph();
            par.ParagraphFormat.Style = builder.Document.Styles["Normal"];
            par.ParagraphFormat.Alignment = ParagraphAlignment.Left;

            foreach (var item in chapter.Verses.OrderBy(x => x.NumberOfVerse)) {
                ExportVerse(item, ref par, builder);
            }

            if (addFooter) WriteFooter(chapter.ParentTranslation, par, builder);
            if (addChapterHeaderAndFooter) AddChapterHeaderAndFooter(chapter, builder);

            return SaveBuilder(saveFormat, builder);
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
                var subtitles = verse.ParentChapter.Subtitles.Where(x => x.BeforeVerseNumber == verse.NumberOfVerse);
                if (subtitles.Any()) {
                    var _par = builder.InsertParagraph();
                    _par.ParagraphFormat.Style = builder.Document.Styles["Nagłówek 3"];
                    _par.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                    _par.ParagraphFormat.KeepWithNext = true;

                    foreach (var subtitle in subtitles) {
                        var run = new Run(builder.Document) {
                            Text = subtitle.Text
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

            if (verse.ParentChapter.NumberOfChapter != 0) { ExportVerseNumber(verse, par, builder); }
            foreach (var item in verse.VerseWords.OrderBy(x => x.NumberOfVerseWord)) {
                ExportVerseWord(item, par, builder);
            }

            builder.MoveTo(par);
        }
        private void ExportVerseNumber(Verse verse, Paragraph par, DocumentBuilder builder) {
            var chapterNumber = verse.ParentTranslation.ChapterRomanNumbering ? verse.ParentChapter.NumberOfChapter.ArabicToRoman() : verse.ParentChapter.NumberOfChapter.ToString();
            var text = $"{chapterNumber},{verse.NumberOfVerse}";
            var shape = new Shape(par.Document, ShapeType.TextBox) {
                Width = 30,
                Height = 80,
                BehindText = false,
                Stroked = false,
                WrapType = WrapType.Inline
            };
            shape.TextBox.FitShapeToText = true;
            shape.TextBox.TextBoxWrapMode = TextBoxWrapMode.None;

            var shapePar = new Paragraph(par.Document);
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
                WrapType = WrapType.Inline
            };
            shape.TextBox.FitShapeToText = true;
            shape.TextBox.TextBoxWrapMode = TextBoxWrapMode.Square;

            var shapePar = new Paragraph(doc);
            shape.AppendChild(shapePar);

            builder.MoveTo(shape.FirstParagraph);

            if (word.ParentVerse.ParentChapter.ParentBook.ParentTranslation.WithStrongs) {
                builder.Font.Size = 6;
                builder.Font.Bold = false;
                builder.Font.Color = Color.DarkBlue;
                if (word.StrongCode.IsNotNull() && word.StrongCode.Topic.IsNotNullOrEmpty()) { builder.Write(word.StrongCode.Topic); } else { builder.Write("–"); }
                builder.InsertBreak(BreakType.LineBreak);
            }
            if (word.ParentVerse.ParentChapter.ParentBook.ParentTranslation.WithGrammarCodes) {
                builder.Font.Size = 6;
                builder.Font.Bold = false;
                builder.Font.Color = Color.DarkBlue;
                if (word.GrammarCode.IsNotNull() && word.GrammarCode.GrammarCodeVariant1.IsNotNullOrEmpty()) { builder.Write(word.GrammarCode.GrammarCodeVariant1); } else { builder.Write("–"); }
                builder.InsertBreak(BreakType.LineBreak);
            }

            builder.Font.Size = 10;
            builder.Font.Bold = false;
            builder.Font.Color = Color.DarkGreen;
            builder.Write(word.SourceWord);
            builder.InsertBreak(BreakType.LineBreak);

            builder.Font.Size = 9;
            builder.Font.Bold = false;
            builder.Font.Color = Color.MidnightBlue;
            builder.Write(word.Transliteration);
            builder.InsertBreak(BreakType.LineBreak);

            builder.Font.Size = 11;
            builder.Font.Bold = word.Citation;
            builder.Font.Color = word.WordOfJesus ? Color.DarkRed : Color.Black;
            if (word.Translation.IsNotNull()) {

                if (word.Translation.Contains("<n>")) {
                    var translation = word.Translation.Replace("<n>", "<span style=\"color: #6c757d; font-size: 11pt;\">").Replace("</n>", "</span>");
                    if (word.WordOfJesus) {
                        translation = $"<span style=\"color: #990000;\">{translation}</span>";
                    }

                    builder.InsertHtml(translation);
                }
                else {
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
                    footnoteText = GetInternalVerseRangeText(footnoteText, translation);
                    footnoteText = GetInternalVerseText(footnoteText, translation);
                    footnoteText = GetExternalVerseRangeText(footnoteText, word);
                    footnoteText = GetExternalVerseText(footnoteText, word);

                    builder.MoveTo(par);

                    var footnote = builder.InsertFootnote(FootnoteType.Footnote, "");
                    footnote.Font.Position = 11;
                    footnote.Font.Color = Color.Blue;
                    footnote.Font.Bold = false;

                    builder.MoveTo(footnote.LastParagraph);
                    builder.InsertHtml($"<sup style=\"font-size: 8pt;\">)</sup>&nbsp;<span style=\"font-size: 10pt;\">{footnoteText}</span>");
                    builder.MoveTo(par);
                }
            }
        }

        private string GetInternalVerseRangeText(string input, Translation translation) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\-(?<verseEnd>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var translationName = translation.Name.Replace("+", "");
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = translation.Books.Where(x => x.NumberOfBook == numberOfBook).First().BaseBook.BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();
                var verseEnd = m.Groups["verseEnd"].Value.ToInt();

                var verseText = GetVerseTranslation(translation.Session, numberOfBook, numberOfChapter, verseStart, verseEnd, translationName);

                var link = $"{bookShortcut} {numberOfChapter}:{verseStart}-{verseEnd}";

                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }

        private string GetInternalVerseText(string input, Translation translation) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = translation.Books.Where(x => x.NumberOfBook == numberOfBook).First().BaseBook.BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();

                var verseText = GetVerseTranslation(translation.Session, numberOfBook, numberOfChapter, verseStart, translationName: translation.Name.Replace("+", ""));

                var link = $"{bookShortcut} {numberOfChapter}:{verseStart}";

                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }

        public string GetExternalVerseText(string input, VerseWord word) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<translationName>[a-zA-Z]+)\s(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var translationName = m.Groups["translationName"].Value;
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var translation = new XPQuery<Translation>(word.Session).Where(x => x.Name.Replace("'", "").Replace("+", "") == translationName).FirstOrDefault();
                var baseBook = translation.Books.Where(x => x.NumberOfBook == numberOfBook).First().BaseBook;
                var bookShortcut = baseBook.IsNotNull() ? baseBook.BookShortcut : "";
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();

                var verseText = GetVerseTranslation(translation.Session, numberOfBook, numberOfChapter, verseStart, translationName: translationName);

                var link = $"{bookShortcut} {numberOfChapter}:{verseStart}";

                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }


        private string GetExternalVerseRangeText(string input, VerseWord word) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<translationName>[a-zA-Z]+)\s(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\-(?<verseEnd>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var translationName = m.Groups["translationName"].Value;
                var translation = new XPQuery<Translation>(word.Session).Where(x => x.Name.Replace("'", "").Replace("+", "") == translationName).FirstOrDefault();
                var baseBook = translation.Books.Where(x => x.NumberOfBook == numberOfBook).First().BaseBook;
                var bookShortcut = baseBook.IsNotNull() ? baseBook.BookShortcut : "";
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();
                var verseEnd = m.Groups["verseEnd"].Value.ToInt();

                var verseText = GetVerseTranslation(translation.Session, numberOfBook, numberOfChapter, verseStart, verseEnd, translationName);

                var link = $"{bookShortcut} {numberOfChapter}:{verseStart}-{verseEnd}";

                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }

        private string GetVerseOtherTranslation(Session session, int numberOfBook, int numberOfChapter, int verseStart) {
            var index = String.Empty;
            Verse verse = null;
            if (numberOfBook > 460) {
                index = $"PBPW.{numberOfBook}.{numberOfChapter}.{verseStart}";
                verse = new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
            }
            else {
                index = $"SNP18.{numberOfBook}.{numberOfChapter}.{verseStart}";
                verse = new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
            }
            if (verse.IsNotNull()) {
                var verseText = verse.Text;
                verseText = System.Text.RegularExpressions.Regex.Replace(verseText, @"\[[0-9]+\]", "");

                return verseText;
            }
            return String.Empty;
        }
        private string GetVersesOtherTranslation(Session session, int numberOfBook, int numberOfChapter, int verseStart, int verseEnd) {
            IEnumerable<Verse> verses = null;
            ExpressionStarter<Verse> predicate = null;
            if (numberOfBook > 460) {
                predicate = PredicateBuilder.New<Verse>();
                for (int i = verseStart; i < verseEnd + 1; i++) {
                    var index = $"PBPW.{numberOfBook}.{numberOfChapter}.{i}";
                    predicate = predicate.Or(x => x.Index == index);
                }

                verses = new XPQuery<Verse>(session).Where(predicate);
            }
            else {
                predicate = PredicateBuilder.New<Verse>();
                for (int i = verseStart; i < verseEnd + 1; i++) {
                    var index = $"SNP18.{numberOfBook}.{numberOfChapter}.{i}";
                    predicate = predicate.Or(x => x.Index == index);
                }

                verses = new XPQuery<Verse>(session).Where(predicate);
            }

            if (verses.Count() > 0) {
                var versesText = String.Empty;
                foreach (var item in verses) {
                    versesText += item.Text + " ";
                }
                versesText = System.Text.RegularExpressions.Regex.Replace(versesText, @"\[[0-9]+\]", "");

                return versesText.Trim();
            }
            return String.Empty;
        }

        private string GetVerseTranslation(Session session, int numberOfBook, int numberOfChapter, int verseStart, int verseEnd = 0, string translationName = "NPI") {
            if (verseEnd == 0) {
                var index = $"{translationName}.{numberOfBook}.{numberOfChapter}.{verseStart}";
                var verse = new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
                if (verse.IsNotNull()) {
                    var verseText = verse.GetTranslationText();
                    if (verseText.IsNotNullOrWhiteSpace()) {
                        return verseText;
                    }
                    else {
                        return GetVerseOtherTranslation(session, numberOfBook, numberOfChapter, verseStart);
                    }
                }
                else {
                    return GetVerseOtherTranslation(session, numberOfBook, numberOfChapter, verseStart);
                }
            }
            else {
                var predicate = PredicateBuilder.New<Verse>();
                for (int i = verseStart; i < verseEnd + 1; i++) {
                    var index = $"{translationName}.{numberOfBook}.{numberOfChapter}.{i}";
                    predicate = predicate.Or(x => x.Index == index);
                }
                var verses = new XPQuery<Verse>(session).Where(predicate);

                if (verses.Count() > 0) {
                    if (verses.First().GetTranslationText().IsNotNullOrWhiteSpace()) {
                        var versesText = String.Empty;
                        foreach (var item in verses) {
                            versesText += item.GetTranslationText() + " ";
                        }
                        return versesText.Trim();
                    }
                    else {
                        return GetVersesOtherTranslation(session, numberOfBook, numberOfChapter, verseStart, verseEnd);
                    }
                }
                else {
                    return GetVersesOtherTranslation(session, numberOfBook, numberOfChapter, verseStart, verseEnd);
                }
            }
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

        private void SaveBuilder(ExportSaveFormat saveFormat, string outputPath, DocumentBuilder builder) {
            if (saveFormat == ExportSaveFormat.Docx) {
                builder.Document.Save(outputPath, SaveFormat.Docx);
            }
            else {
                builder.Document.Save(outputPath, new PdfSaveOptions() {
                    SaveFormat = SaveFormat.Pdf,
                    AllowEmbeddingPostScriptFonts = true,
                    ColorMode = ColorMode.Normal,
                    Compliance = PdfCompliance.PdfA1a,
                    CreateNoteHyperlinks = true,
                    DisplayDocTitle = true,
                    ExportDocumentStructure = true,
                    JpegQuality = 100,
                    OutlineOptions = { HeadingsOutlineLevels = 3, ExpandedOutlineLevels = 3, CreateOutlinesForHeadingsInTables = true, CreateMissingOutlineLevels = true },
                    OptimizeOutput = true
                });
            }
        }

        private byte[] SaveBuilder(ExportSaveFormat saveFormat, DocumentBuilder builder) {
            var ms = new MemoryStream();
            if (saveFormat == ExportSaveFormat.Docx) {
                builder.Document.Save(ms, SaveFormat.Docx);
            }
            else {
                builder.Document.Save(ms, new PdfSaveOptions() {
                    SaveFormat = SaveFormat.Pdf,
                    AllowEmbeddingPostScriptFonts = true,
                    ColorMode = ColorMode.Normal,
                    Compliance = PdfCompliance.PdfA1a,
                    CreateNoteHyperlinks = true,
                    DisplayDocTitle = true,
                    ExportDocumentStructure = true,
                    JpegQuality = 100,
                    OutlineOptions = { HeadingsOutlineLevels = 3, ExpandedOutlineLevels = 3, CreateOutlinesForHeadingsInTables = true, CreateMissingOutlineLevels = true },
                    OptimizeOutput = true
                });
            }
            return ms.GetBuffer();
        }

        private DocumentBuilder GetDocumentBuilder() {
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

            var hyperLinkStyle = document.Styles[StyleIdentifier.Hyperlink];
            if (hyperLinkStyle.IsNotNull()) {
                hyperLinkStyle.Font.Color = System.Drawing.Color.Black;
                hyperLinkStyle.Font.Underline = Underline.None;
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

            var h2 = document.Styles.Add(StyleType.Paragraph, "Nagłówek 2");
            h2.BaseStyleName = "Heading 2";
            h2.Font.Size = 14;
            h2.Font.Bold = true;
            h2.Font.Color = Color.Black;
            h2.Font.Italic = false;
            h2.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            h2.ParagraphFormat.KeepWithNext = true;

            var h3 = document.Styles.Add(StyleType.Paragraph, "Nagłówek 3");
            h3.BaseStyleName = "Heading 3";
            h3.Font.Size = 12;
            h3.Font.Bold = true;
            h3.Font.Color = Color.Black;
            h3.Font.Italic = false;
            h3.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            h3.ParagraphFormat.KeepWithNext = true;

            var builder = new DocumentBuilder(document);
            builder.Font.NoProofing = true;
            return builder;
        }
    }

    public enum ExportSaveFormat {
        Docx, Pdf
    }
}
