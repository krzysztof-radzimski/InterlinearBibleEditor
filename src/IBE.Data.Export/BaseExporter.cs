using Aspose.Words;
using Aspose.Words.Loading;
using Aspose.Words.Saving;
using IBE.Common.Extensions;
using System.Drawing;
using System.IO;

namespace IBE.Data.Export {
    public abstract class BaseExporter {
        protected System.Drawing.Graphics g;
        protected System.Drawing.Font font11;
        protected System.Drawing.Font font11bold;
        protected System.Drawing.Font font10;
        protected System.Drawing.Font font9;
        protected System.Drawing.Font font8;

        protected BaseExporter() { }
        public BaseExporter(byte[] asposeLicense) : this() {
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

        protected DocumentBuilder GetDocumentBuilder() {
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

        protected byte[] SaveBuilder(ExportSaveFormat saveFormat, DocumentBuilder builder) {
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
        protected void SaveBuilder(ExportSaveFormat saveFormat, string outputPath, DocumentBuilder builder) {
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
    }   
}
