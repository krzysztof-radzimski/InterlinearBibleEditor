/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Export {
    public abstract class BaseExporter {
        protected System.Drawing.Graphics g;
        protected System.Drawing.Font font11;
        protected System.Drawing.Font font11bold;
        protected System.Drawing.Font font10;
        protected System.Drawing.Font font9;
        protected System.Drawing.Font font8;

        protected string Host { get; }

        protected BaseExporter() { }
        public BaseExporter(byte[] asposeLicense, string host) : this() {
            if (asposeLicense.IsNotNull()) {
                new Aspose.Words.License().SetLicense(new MemoryStream(asposeLicense));
            }
            Host = host;
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
            //document.FirstSection.PageSetup.TextColumns.SetCount(1);
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
            h1.Font.Name = "Times New Roman";
            h1.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            //h1.ParagraphFormat.LineSpacing = 18;
            h1.ParagraphFormat.KeepWithNext = true;

            var h2 = document.Styles.Add(StyleType.Paragraph, "Nagłówek 2");
            h2.BaseStyleName = "Heading 2";
            h2.Font.Size = 14;
            h2.Font.Bold = true;
            h2.Font.Color = Color.Black;
            h2.Font.Italic = false;
            h2.Font.Name = "Times New Roman";
            h2.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            h2.ParagraphFormat.KeepWithNext = true;

            var h3 = document.Styles.Add(StyleType.Paragraph, "Nagłówek 3");
            h3.BaseStyleName = "Heading 3";
            h3.Font.Size = 13;
            h3.Font.Bold = true;
            h3.Font.Color = Color.Black;
            h3.Font.Italic = false;
            h3.Font.Name = "Times New Roman";
            h3.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            h3.ParagraphFormat.KeepWithNext = true;

            var h4 = document.Styles.Add(StyleType.Paragraph, "Nagłówek 4");
            h4.BaseStyleName = "Heading 4";
            h4.Font.Size = 12;
            h4.Font.Bold = true;
            h4.Font.Color = Color.Black;
            h4.Font.Italic = false;
            h4.Font.Name = "Times New Roman";
            h4.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            h4.ParagraphFormat.KeepWithNext = true;

            var builder = new DocumentBuilder(document);
            builder.Font.NoProofing = true;
            return builder;
        }

        protected byte[] SaveBuilder(ExportSaveFormat saveFormat, DocumentBuilder builder) {
            ResizeShapes(builder);

            var ms = new MemoryStream();
            if (saveFormat == ExportSaveFormat.Docx) {
                var saveOptions = new Aspose.Words.Saving.OoxmlSaveOptions(SaveFormat.Docx) {
                    Compliance = OoxmlCompliance.Iso29500_2008_Transitional,
                    AllowEmbeddingPostScriptFonts = true,
                    CompressionLevel = 0,
                    MemoryOptimization = false,
                    PrettyFormat = true,
                    UpdateLastSavedTimeProperty = true,
                    UseHighQualityRendering = true,
                    UseAntiAliasing = true,
                    ExportGeneratorName = false
                };
                builder.Document.Save(ms, saveOptions);
            }
            else {
                builder.Document.Save(ms, new PdfSaveOptions() {
                    SaveFormat = SaveFormat.Pdf,
                    AllowEmbeddingPostScriptFonts = true,
                    ColorMode = ColorMode.Normal,
                    Compliance = PdfCompliance.PdfA2a,
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
            ResizeShapes(builder);

            if (saveFormat == ExportSaveFormat.Docx) {
                builder.Document.Save(outputPath, SaveFormat.Docx);
            }
            else {
                builder.Document.Save(outputPath, new PdfSaveOptions() {
                    SaveFormat = SaveFormat.Pdf,
                    AllowEmbeddingPostScriptFonts = true,
                    ColorMode = ColorMode.Normal,
                    Compliance = PdfCompliance.PdfA2a,
                    CreateNoteHyperlinks = true,
                    DisplayDocTitle = true,
                    ExportDocumentStructure = true,
                    JpegQuality = 100,
                    OutlineOptions = { HeadingsOutlineLevels = 3, ExpandedOutlineLevels = 3, CreateOutlinesForHeadingsInTables = true, CreateMissingOutlineLevels = true },
                    OptimizeOutput = true
                });
            }
        }
        protected List<BookBaseInfo> GetBookBases(UnitOfWork uow = null) {
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

        private void ResizeShapes(DocumentBuilder builder) {
            var _shapes = builder.Document.GetChildNodes(NodeType.Shape, true);
            if (_shapes.Count > 0) {
                IEnumerable<Shape> shapes = _shapes.OfType<Shape>().Where(s => s.HasImage);
                foreach (Shape shape in shapes) {
                    double targetHeight = 0;
                    double targetWidth = 0;
                    CalculateImageSize(builder, shape, out targetHeight, out targetWidth);
                    if (targetWidth > 0 && targetHeight > 0) {
                        shape.Width = targetWidth;
                        shape.Height = targetHeight;
                    }
                }
            }
        }
        private void CalculateImageSize(DocumentBuilder builder, Shape shape, out double targetHeight, out double targetWidth) {

            //Calculate width and height of the page
            PageSetup ps = builder.CurrentSection.PageSetup;
            targetHeight = ps.PageHeight - ps.TopMargin - ps.BottomMargin;
            targetWidth = ps.PageWidth - ps.LeftMargin - ps.RightMargin;


            //Get size of an image
            double imgHeight = ConvertUtil.PixelToPoint(shape.Height);
            double imgWidth = ConvertUtil.PixelToPoint(shape.Width);

            if (targetHeight > imgHeight && targetWidth > imgWidth) { return; }

            if (imgHeight < targetHeight && imgWidth < targetWidth) {
                targetHeight = imgHeight;
                targetWidth = imgWidth;
            }
            else {
                //Calculate size of an image in the document
                double ratioWidth = imgWidth / targetWidth;
                double ratioHeight = imgHeight / targetHeight;

                if (ratioWidth > ratioHeight)
                    targetHeight = (targetHeight * (ratioHeight / ratioWidth));
                else
                    targetHeight = (targetWidth * (ratioWidth / ratioHeight));
            }
        }

        protected string GetFootnotesPattern() {
            var footNoteTextPatternFragment = @"\w\s\.\=\""\,\;\:\-\(\)\<\>\„\”\/\!\·\…\d\–\?\־\’\’\‘\#\᾽\…\—\‎\𝔓\´";
            var f1 = $@"\[\*\s?(?<f1>[{footNoteTextPatternFragment}]+)\]";
            var f2 = $@"\[\*\*\s?(?<f2>[{footNoteTextPatternFragment}]+)\]";
            var f3 = $@"\[\*\*\*\s?(?<f3>[{footNoteTextPatternFragment}]+)\]";
            var f4 = $@"\[\*\*\*\*\s?(?<f4>[{footNoteTextPatternFragment}]+)\]";
            var f5 = $@"\[\*\*\*\*\*\s?(?<f5>[{footNoteTextPatternFragment}]+)\]";
            var f6 = $@"\[\*\*\*\*\*\*\s?(?<f6>[{footNoteTextPatternFragment}]+)\]";
            var f7 = $@"\[\*\*\*\*\*\*\*\s?(?<f7>[{footNoteTextPatternFragment}]+)\]";
            var footNoteTextPattern = $@"\<n\>(\s+)?{f1}(\s+)?({f2})?(\s+)?({f3})?(\s+)?({f4})?(\s+)?({f5})?(\s+)?({f6})?(\s+)?({f7})?(\s+)?\</n\>";
            return footNoteTextPattern;
        }

        protected string FormatVerseText(string text, BiblePart biblePart) {
            // God's words
            text = text.Replace("<J>", @"<span style=""color: darkred;"">").Replace("</J>", "</span>");
            // Added elements
            text = text.Replace("<n>", @"<span style=""color: darkgray;"">").Replace("</n>", "</span>");
            // Other formatting
            text = text.Replace("<pb/>", "").Replace("<t>", "").Replace("</t>", "").Replace("<e>", "").Replace("</e>", "");

            // change Lord and Jehova names to Jahwe.
            if (biblePart == BiblePart.OldTestament) {
                text = Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>PAN(A)?(EM)?(U)?(IE)?)[\s\,\.\:\""\'\”ʼ]", delegate (Match m) {
                    var prefix = m.Groups["prefix"].Value;
                    return $"{prefix}JAHWE{m.Value.Last()}";
                });
            }
            if (biblePart == BiblePart.OldTestament) {
                text = Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>JHWH)[\s\,\.\:\""\'\”ʼ]", delegate (Match m) {
                    var prefix = m.Groups["prefix"].Value;
                    return $"{prefix}JAHWE{m.Value.Last()}";
                });
            }
            if (biblePart == BiblePart.OldTestament) {
                text = Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>Jehow(a)?(y)?(ie)?(ę)?(o)?)[\s\,\.\:\""\'\”ʼ]", delegate (Match m) {
                    var prefix = m.Groups["prefix"].Value;
                    return $"{prefix}JAHWE{m.Value.Last()}";
                });
            }
            if (biblePart == BiblePart.NewTestament) {
                text = Regex.Replace(text, @"(?<prefix>[\s\”\""\„ʼ])(?<name>Jehow(?<ending>(a)?(y)?(ie)?(ę)?(o)?))[\s\,\.\:\""\'\”ʼ]", delegate (Match m) {
                    var prefix = m.Groups["prefix"].Value;
                    var ending = m.Groups["ending"].Value;
                    var root = "Pan";
                    if (ending == "ie") { root += "u"; }
                    if (ending == "o") { root += "ie"; }
                    if (ending == "y" || ending == "ę") { root += "a"; }
                    return $"{prefix}{root}{m.Value.Last()}";
                });
            }

            // remove orphans
            text = Regex.Replace(text, @"[\s\(\,\;][a,i,o,w,z]\s", delegate (Match m) {
                return " " + m.Value.Trim() + "&nbsp;";
            }, RegexOptions.IgnoreCase);

            // remove empty footnotes
            text = Regex.Replace(text, @"\[[0-9]+\]", delegate (Match m) {
                return String.Empty;
            }, RegexOptions.IgnoreCase);

            return text;
        }
    }
}
