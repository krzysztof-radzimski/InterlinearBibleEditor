/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Export {
    public abstract class BaseDefaultExporter : BaseExporter {
        protected BaseDefaultExporter() : base() { }
        public BaseDefaultExporter(byte[] asposeLicense, string host) : base(asposeLicense, host) { }

        public virtual void Export(Translation translation, ExportSaveFormat saveFormat, string outputPath, bool addFooter = true) {
            if (translation.IsNull()) { throw new ArgumentNullException("translation"); }
            if (outputPath.IsNullOrEmpty()) { throw new ArgumentNullException("outputPath"); }

            var builder = GetDocumentBuilder();

            foreach (var book in translation.Books.OrderBy(x => x.NumberOfBook)) {
                ExportBookName(book, builder);
                var chapters = book.Chapters.OrderBy(x => x.NumberOfChapter).ToArray();
                foreach (var chapter in chapters) {

                    if (chapter == chapters.First()) { builder.InsertParagraph(); }

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
            }

            builder.MoveToDocumentEnd();

            SaveBuilder(saveFormat, outputPath, builder);
        }
        public virtual void Export(Book book, ExportSaveFormat saveFormat, string outputPath, bool addFooter = true, bool addBookAndHeader = true) {
            if (book.IsNull()) { throw new ArgumentNullException("book"); }
            if (outputPath.IsNullOrEmpty()) { throw new ArgumentNullException("outputPath"); }

            var builder = GetDocumentBuilder();

            ExportBookName(book, builder);

            var chapters = book.Chapters.OrderBy(x => x.NumberOfChapter).ToArray();
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
        public virtual byte[] Export(Book book, ExportSaveFormat saveFormat, bool addFooter = true, bool addBookAndHeader = true) {
            if (book.IsNull()) { throw new ArgumentNullException("book"); }

            var builder = GetDocumentBuilder();

            ExportBookName(book, builder);

            var chapters = book.Chapters.OrderBy(x => x.NumberOfChapter).ToArray();
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
        public virtual void Export(Chapter chapter, ExportSaveFormat saveFormat, string outputPath, bool addFooter = true, bool addChapterHeaderAndFooter = true) {
            if (chapter.IsNull()) { throw new ArgumentNullException("chapter"); }
            if (outputPath.IsNullOrEmpty()) { throw new ArgumentNullException("outputPath"); }

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
        public virtual byte[] Export(Chapter chapter, ExportSaveFormat saveFormat, bool addFooter = true, bool addChapterHeaderAndFooter = true) {
            if (chapter.IsNull()) { throw new ArgumentNullException("chapter"); }

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
        protected abstract void ExportVerse(Verse verse, ref Paragraph par, DocumentBuilder builder);
        protected void AddBookHeaderAndFooter(Book book, DocumentBuilder builder) {
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
        protected void AddChapterHeaderAndFooter(Chapter chapter, DocumentBuilder builder) {
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
        protected void WriteFooter(Translation translation, Paragraph par, DocumentBuilder builder) {
            if (par.IsNotNull()) builder.MoveTo(par);

            builder.InsertParagraph();

            builder.InsertHtml($"<div style=\"font-size: 11; text-align: left;\">{translation.DetailedInfo}</div>");
        }

        protected void ExportChapterNumber(Chapter chapter, DocumentBuilder builder, bool withBookName = false) {
            if (withBookName) {
                ExportBookName(chapter.ParentBook, builder);
            }

            if (chapter.ParentBook.NumberOfChapters == 1) { return; }

            var par = builder.InsertParagraph();
            builder.Font.ClearFormatting();
            par.ParagraphFormat.ClearFormatting();

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
        protected void ExportBookName(Book book, DocumentBuilder builder) {
            builder.CurrentParagraph.ParagraphFormat.Style = builder.Document.Styles["Nagłówek 1"];
            builder.CurrentParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.CurrentParagraph.ParagraphFormat.KeepWithNext = true;
            builder.Write(book.BaseBook.BookTitle);
        }
    }
}
