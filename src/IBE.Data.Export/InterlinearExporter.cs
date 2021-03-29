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

        private InterlinearExporter() { }
        public InterlinearExporter(byte[] asposeLicense) : this() {
            if (asposeLicense.IsNotNull()) {
                new License().SetLicense(new MemoryStream(asposeLicense));
            }
        }

        public void Test(string outputPath) {
            // Create and empty document and DocumentBuilder.

            Document doc = new Document();

            DocumentBuilder builder = new DocumentBuilder(doc);


            // Create textbox.

            Shape shape = new Shape(doc, ShapeType.TextBox) {
                Width = 100,
                Height = 100,
                BehindText = false,
                Stroked = false,
                
            };
            shape.TextBox.FitShapeToText = true;

            // Make textbox TopBottom, so if we insert another textbox

            // into the next paragpaph, it will be always below this textbox.

            shape.WrapType = WrapType.Inline;


            // Create paragraph and insert it into the textbox.

            Paragraph par = new Paragraph(doc);
            shape.AppendChild(par);


            // Create another paragraph and insert textbox into it.

            //Paragraph mainPar = new Paragraph(doc);

            //mainPar.AppendChild(shape);


            Paragraph mainPar = builder.CurrentParagraph;
            mainPar.AppendChild(shape);

            for (int i = 0; i < 5; i++) { 
            
            }


                //for (int i = 0; i < 5; i++) {

                //    // Insert paragraph with textbox into the document.

                //    Paragraph mainPar1 = (Paragraph)mainPar.Clone(true);

                //    refParagraph.ParentNode.InsertAfter(mainPar1, refParagraph);

                //    refParagraph = mainPar1;


                //    // Insert some text into the textbox.

                //    builder.MoveTo(((Shape)mainPar1.FirstChild).FirstParagraph);

                //    builder.Writeln(i.ToString());

                //    builder.Writeln("This is some text in this textbox");

                //    builder.Write("This is some more text");

                //}


                doc.Save(outputPath);
        }

        public void Export(Translation translation, ExportSaveFormat saveFormat, string outputPath) {
            if (translation.IsNull()) { throw new ArgumentNullException("translation"); }
            if (outputPath.IsNullOrEmpty()) { throw new ArgumentNullException("outputPath"); }

            var builder = GetDocumentBuilder();
            SaveBuilder(saveFormat, outputPath, builder);
        }

        public void Export(Book book, ExportSaveFormat saveFormat, string outputPath) {
            if (book.IsNull()) { throw new ArgumentNullException("book"); }
            if (outputPath.IsNullOrEmpty()) { throw new ArgumentNullException("outputPath"); }

            var builder = GetDocumentBuilder();
            SaveBuilder(saveFormat, outputPath, builder);
        }

        public void Export(Chapter chapter, ExportSaveFormat saveFormat, string outputPath) {
            if (chapter.IsNull()) { throw new ArgumentNullException("chapter"); }
            if (outputPath.IsNullOrEmpty()) { throw new ArgumentNullException("outputPath"); }       

            var builder = GetDocumentBuilder();
            ExportChapterNumber(chapter, builder, true);
            foreach (var item in chapter.Verses) {
                ExportVerse(item, builder);
                break;
            }

            SaveBuilder(saveFormat, outputPath, builder);
        }

        private void ExportChapterNumber(Chapter chapter, DocumentBuilder builder, bool withBookName = false) {
            builder.MoveToDocumentEnd();

            if (withBookName) {                
                builder.Font.Size = 16;
                builder.Font.Bold = true;
                builder.Font.Color = Color.Black;
                builder.CurrentParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Center;

                builder.Write(chapter.ParentBook.BaseBook.BookTitle);
            }

            builder.InsertParagraph();

            builder.Font.Size = 14;
            builder.Font.Bold = true;
            builder.Font.Color = Color.Black;
            builder.CurrentParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            var chapterString = chapter.ParentBook.NumberOfBook == 230 ? chapter.ParentTranslation.ChapterPsalmString : chapter.ParentTranslation.ChapterString;
            var chapterNumber = chapter.ParentTranslation.ChapterRomanNumbering ? chapter.NumberOfChapter.ArabicToRoman() : chapter.NumberOfChapter.ToString();
            builder.Writeln($"{chapterString} {chapterNumber}");

            builder.CurrentParagraph.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
        }

        private void ExportVerse(Verse verse, DocumentBuilder builder) {
            ExportVerseNumber(verse, builder);
            foreach (var item in verse.VerseWords) {
                ExportVerseWord(item, builder);
                break;
            }
        }

        private void ExportVerseNumber(Verse verse, DocumentBuilder builder) {
            builder.MoveToDocumentEnd();
            var shape = builder.InsertShape(ShapeType.TextBox, 50, 80);
            //shape.TextBox.FitShapeToText = true;
            shape.TextBox.TextBoxWrapMode = TextBoxWrapMode.None;
            //shape.TextBox.LayoutFlow = LayoutFlow.Horizontal;
            shape.Stroked = false;
            shape.BehindText = false;
            builder.MoveTo(shape.LastParagraph);
            builder.Font.Size = 12;
            builder.Font.Bold = true;
            builder.Font.Color = Color.Black;
            builder.Write($"{verse.ParentChapter.NumberOfChapter},{verse.NumberOfVerse}");            
        }

        private void ExportVerseWord(VerseWord word, DocumentBuilder builder) {
            builder.MoveToDocumentEnd();
            //var doc = builder.Document;
            //var size = GetMaxTextSize(word);
            var shape = builder.InsertShape(ShapeType.TextBox, 150, 80);
            //shape.TextBox.FitShapeToText = true;
            shape.TextBox.TextBoxWrapMode = TextBoxWrapMode.None;
            //shape.TextBox.LayoutFlow = LayoutFlow.Horizontal;
            shape.Stroked = false;
            shape.BehindText = false;
            builder.MoveTo(shape.LastParagraph);

            if (word.ParentVerse.ParentChapter.ParentBook.ParentTranslation.WithStrongs) {
                builder.Font.Size = 8;
                builder.Font.Bold = false;
                builder.Font.Color = Color.Black;
                if (word.StrongCode.IsNotNull()) { builder.Write(word.StrongCode.Topic); } else { builder.Write("–"); }
                builder.InsertBreak(BreakType.LineBreak);
            }
            if (word.ParentVerse.ParentChapter.ParentBook.ParentTranslation.WithGrammarCodes) {
                builder.Font.Size = 8;
                builder.Font.Bold = false;
                builder.Font.Color = Color.Black;
                if (word.GrammarCode.IsNotNull()) { builder.Write(word.GrammarCode.GrammarCodeVariant1); } else { builder.Write("–"); }
                builder.InsertBreak(BreakType.LineBreak);
            }

            builder.Font.Size = 12;
            builder.Font.Bold = false;
            builder.Font.Color = Color.Black;
            builder.Write(word.SourceWord);
            builder.InsertBreak(BreakType.LineBreak);

            builder.Font.Size = 10;
            builder.Font.Bold = false;
            builder.Font.Color = Color.Black;
            builder.Write(word.Transliteration);
            builder.InsertBreak(BreakType.LineBreak);

            builder.Font.Size = 12;
            builder.Font.Bold = word.Citation;
            builder.Font.Color = Color.Black;
            builder.Font.Color = word.WordOfJesus ? Color.DarkRed : Color.Black;
            builder.InsertHtml(word.Translation.Replace("<n>", "<span style=\"color: #6c757d;\">").Replace("</n>", "</span>"));

            if (word.FootnoteText.IsNotNullOrEmpty()) {
                var footnoteText = word.FootnoteText;
                if (footnoteText.IsNotNullOrEmpty() && footnoteText.Contains("<x>")) {
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
                    builder.InsertFootnote(FootnoteType.Footnote, footnoteText);
                }
            }            
        }

        private SizeF GetMaxTextSize(VerseWord word) {
            if (word.IsNotNull()) {
                var img = new Bitmap(1, 1);
                var g = System.Drawing.Graphics.FromImage(img);
                var list = new List<SizeF> {
                    g.MeasureString(word.SourceWord, SystemFonts.GetFontByName("Times New Roman")),
                    g.MeasureString(word.Transliteration, SystemFonts.GetFontByName("Times New Roman")),
                    g.MeasureString(word.Translation, SystemFonts.GetFontByName("Times New Roman"))
                };

                return list.Max();
            }

            return default;
        }

        private void SaveBuilder(ExportSaveFormat saveFormat, string outputPath, DocumentBuilder builder) {
            if (saveFormat == ExportSaveFormat.Docx) {
                builder.Document.Save(outputPath, SaveFormat.Docx);
            }
            else {
                builder.Document.Save(outputPath, new PdfSaveOptions() {
                    SaveFormat = SaveFormat.Pdf,
                    AllowEmbeddingPostScriptFonts = true,
                    ColorMode = ColorMode.Grayscale,
                    Compliance = PdfCompliance.PdfA1a,
                    CreateNoteHyperlinks = true,
                    DisplayDocTitle = true,
                    ExportDocumentStructure = true,
                    JpegQuality = 100
                });
            }
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
                normalStyle.Font.Size = 12;
                normalStyle.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
                normalStyle.Font.LocaleId = (int)EditingLanguage.Polish;
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
