using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Saving;
using IBE.Common.Extensions;
using IBE.Data.Model;
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

        public void Export(Book book, ExportSaveFormat saveFormat, string outputPath) {
            if (book.IsNull()) { throw new ArgumentNullException("book"); }
            if (outputPath.IsNullOrEmpty()) { throw new ArgumentNullException("outputPath"); }

            if (book.ParentTranslation.Type != TranslationType.Interlinear) { throw new Exception("Wrong translation type!"); }

            var builder = GetDocumentBuilder();
            SaveBuilder(saveFormat, outputPath, builder);
        }

        public void Export(Chapter chapter, ExportSaveFormat saveFormat, string outputPath) {
            if (chapter.IsNull()) { throw new ArgumentNullException("chapter"); }
            if (outputPath.IsNullOrEmpty()) { throw new ArgumentNullException("outputPath"); }

            if (chapter.ParentTranslation.Type != TranslationType.Interlinear) { throw new Exception("Wrong translation type!"); }

            var builder = GetDocumentBuilder();
            ExportChapterNumber(chapter, builder, true);
            var par = builder.InsertParagraph();
            par.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            foreach (var item in chapter.Verses) {
                ExportVerse(item, par, builder);
            }

            WriteFooter(chapter.ParentTranslation, par, builder);

            SaveBuilder(saveFormat, outputPath, builder);
        }

        public byte[] Export(Chapter chapter, ExportSaveFormat saveFormat) {
            if (chapter.IsNull()) { throw new ArgumentNullException("chapter"); }
            if (chapter.ParentTranslation.Type != TranslationType.Interlinear) { throw new Exception("Wrong translation type!"); }

            var builder = GetDocumentBuilder();
            ExportChapterNumber(chapter, builder, true);
            var par = builder.InsertParagraph();
            par.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            foreach (var item in chapter.Verses) {
                ExportVerse(item, par, builder);
            }

            WriteFooter(chapter.ParentTranslation, par, builder);

            return SaveBuilder(saveFormat, builder);
        }

        private void WriteFooter(Translation translation, Paragraph par, DocumentBuilder builder) {
            builder.MoveTo(par);
          
            builder.InsertParagraph();

            builder.InsertHtml($"<div style=\"font-size: 11; text-align: left;\">{translation.DetailedInfo}</div>");
        }

        private void ExportChapterNumber(Chapter chapter, DocumentBuilder builder, bool withBookName = false) {
            builder.MoveToDocumentEnd();

            if (withBookName) {
                builder.Font.Size = 16;
                builder.Font.Bold = true;
                builder.Font.Color = Color.Black;
                builder.CurrentParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.CurrentParagraph.ParagraphFormat.LineSpacing = 18;

                builder.Write(chapter.ParentBook.BaseBook.BookTitle);
            }

            builder.InsertParagraph();

            builder.Font.Size = 14;
            builder.Font.Bold = true;
            builder.Font.Color = Color.Black;
            builder.CurrentParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            builder.CurrentParagraph.ParagraphFormat.LineSpacing = 18;

            var chapterString = chapter.ParentBook.NumberOfBook == 230 ? chapter.ParentTranslation.ChapterPsalmString : chapter.ParentTranslation.ChapterString;
            var chapterNumber = chapter.ParentTranslation.ChapterRomanNumbering ? chapter.NumberOfChapter.ArabicToRoman() : chapter.NumberOfChapter.ToString();
            builder.Write($"{chapterString} {chapterNumber}");
        }

        private void ExportVerse(Verse verse, Paragraph par, DocumentBuilder builder) {
            // try GroupShape https://apireference.aspose.com/words/net/aspose.words.drawing/groupshape
            ExportVerseNumber(verse, par, builder);
            foreach (var item in verse.VerseWords) {
                ExportVerseWord(item, par, builder);
            }
        }

        private void ExportVerseNumber(Verse verse, Paragraph par, DocumentBuilder builder) {
            var text = $"{verse.ParentChapter.NumberOfChapter},{verse.NumberOfVerse}";
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

            par.AppendChild(shape);
            shape.Width = GetMaxTextSize(text, font11bold) * 1.2F;
        }

        private void ExportVerseWord(VerseWord word, Paragraph par, DocumentBuilder builder) {
            var shape = new Shape(par.Document, ShapeType.TextBox) {
                Width = 50,
                Height = 80,
                BehindText = false,
                Stroked = false,
                WrapType = WrapType.Inline
            };
            shape.TextBox.FitShapeToText = true;
            shape.TextBox.TextBoxWrapMode = TextBoxWrapMode.Square;

            var shapePar = new Paragraph(par.Document);
            shape.AppendChild(shapePar);

            builder.MoveTo(shape.FirstParagraph);

            if (word.ParentVerse.ParentChapter.ParentBook.ParentTranslation.WithStrongs) {
                builder.Font.Size = 6;
                builder.Font.Bold = false;
                builder.Font.Color = Color.Black;
                if (word.StrongCode.IsNotNull()) { builder.Write(word.StrongCode.Topic); } else { builder.Write("–"); }
                builder.InsertBreak(BreakType.LineBreak);
            }
            if (word.ParentVerse.ParentChapter.ParentBook.ParentTranslation.WithGrammarCodes) {
                builder.Font.Size = 6;
                builder.Font.Bold = false;
                builder.Font.Color = Color.Black;
                if (word.GrammarCode.IsNotNull()) { builder.Write(word.GrammarCode.GrammarCodeVariant1); } else { builder.Write("–"); }
                builder.InsertBreak(BreakType.LineBreak);
            }

            builder.Font.Size = 10;
            builder.Font.Bold = false;
            builder.Font.Color = Color.Black;
            builder.Write(word.SourceWord);
            builder.InsertBreak(BreakType.LineBreak);

            builder.Font.Size = 9;
            builder.Font.Bold = false;
            builder.Font.Color = Color.Black;
            builder.Write(word.Transliteration);
            builder.InsertBreak(BreakType.LineBreak);

            builder.Font.Size = 11;
            builder.Font.Bold = word.Citation;
            builder.Font.Color = word.WordOfJesus ? Color.DarkRed : Color.Black;
            if (word.Translation.IsNotNull()) {

                if (word.Translation.Contains("<n>")) {
                    var translation = word.Translation.Replace("<n>", "<span style=\"color: #6c757d;\">").Replace("</n>", "</span>");
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
                    footnoteText = System.Text.RegularExpressions.Regex.Replace(footnoteText, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\-(?<verseEnd>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                        var numberOfBook = m.Groups["book"].Value.ToInt();
                        var bookShortcut = word.ParentVerse.ParentChapter.ParentBook.ParentTranslation.Books.Where(x => x.NumberOfBook == Convert.ToInt32(m.Groups["book"].Value)).First().BaseBook.BookShortcut;
                        var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                        var verseStart = m.Groups["verseStart"].Value.ToInt();
                        var verseEnd = m.Groups["verseEnd"].Value.ToInt();
                        var versesText = String.Empty;
                        for (int i = verseStart; i <= verseEnd; i++) {
                            versesText += $"{i}";
                            if (i != verseEnd) { versesText += ","; }
                        }

                        return $"{bookShortcut} {numberOfChapter}:{verseStart}-{verseEnd}";
                    });
                    footnoteText = System.Text.RegularExpressions.Regex.Replace(footnoteText, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                        var numberOfBook = m.Groups["book"].Value.ToInt();
                        var bookShortcut = word.ParentVerse.ParentChapter.ParentBook.ParentTranslation.Books.Where(x => x.NumberOfBook == Convert.ToInt32(m.Groups["book"].Value)).First().BaseBook.BookShortcut;
                        var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                        var verseStart = m.Groups["verseStart"].Value.ToInt();


                        return $"{bookShortcut} {numberOfChapter}:{verseStart}";
                    });

                    builder.MoveTo(par);
                    var footnote = builder.InsertFootnote(FootnoteType.Footnote, footnoteText);
                    footnote.Font.Position = 11;
                }
            }
        }

        private float GetMaxTextSize(VerseWord word) {
            if (word.IsNotNull()) {
                var list = new List<float> {
                    g.MeasureString(word.SourceWord, font11).Width,
                    g.MeasureString(word.Transliteration, font10).Width,
                    g.MeasureString(word.Translation.RemoveAny("<n>","</n>"), word.Citation ? font11bold : font11).Width
                };
                if (word.GrammarCode.IsNotNull()) { list.Add(g.MeasureString(word.GrammarCode.GrammarCodeVariant1, font8).Width); }
                if (word.StrongCode.IsNotNull()) { list.Add(g.MeasureString(word.StrongCode.Topic, font8).Width); }

                return list.Max() * 1.1F;
            }
            return default;
        }
        private float GetMaxTextSize(string text, System.Drawing.Font font) {
            return g.MeasureString(text, font).Width;
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
                    JpegQuality = 100
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
                    JpegQuality = 100
                });
            }
            return ms.GetBuffer();
        }

        private DocumentBuilder GetDocumentBuilder() {
            var document = new Document();

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
            var builder = new DocumentBuilder(document);
            builder.Font.NoProofing = true;
            return builder;
        }
    }

    public enum ExportSaveFormat {
        Docx, Pdf
    }
}
