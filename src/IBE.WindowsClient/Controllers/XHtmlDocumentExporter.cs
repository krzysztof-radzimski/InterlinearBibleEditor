using DevExpress.Office;
using DevExpress.Office.Export;
using DevExpress.Office.Internal;
using DevExpress.Utils;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Export;
using DevExpress.XtraRichEdit.Internal;
using DevExpress.XtraRichEdit.Model;
using IBE.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IBE.WindowsClient.Controllers {
    public class XHtmlDocumentExporter : IExporter<DocumentFormat, bool> {
        internal static readonly FileDialogFilter filter = new FileDialogFilter("Simple HTML document", "xhtml");
        public FileDialogFilter Filter => filter;
        public DocumentFormat Format => XHtmlDocumentFormat.Id;

        public bool SaveDocument(IDocumentModel documentModel, Stream stream, IExporterOptions options) {
            XHtmlExporter exporter = new XHtmlExporter((DocumentModel)documentModel, (XHtmlDocumentExporterOptions)options);
            exporter.Export(stream);
            return true;
        }

        public IExporterOptions SetupSaving() {
            return new XHtmlDocumentExporterOptions();
        }

        public static void Register(IServiceProvider provider) {
            if (provider == null)
                return;
            IDocumentExportManagerService service = provider.GetService(typeof(IDocumentExportManagerService)) as IDocumentExportManagerService;
            if (service != null)
                service.RegisterExporter(new XHtmlDocumentExporter());
        }
    }

    public class XHtmlExporter : DocumentModelExporter {
        readonly XHtmlDocumentExporterOptions options;
        Stream outputStream;
        StreamWriter documentContentWriter;
        bool hyperlinkExporting;
        bool inList = false;
        List<string> footnotes = new List<string>();
        List<string> endnotes = new List<string>();

        protected XHtmlDocumentExporterOptions Options { get { return options; } }
        protected internal StreamWriter DocumentContentWriter { get { return documentContentWriter; } set { documentContentWriter = value; } }

        public XHtmlExporter(DocumentModel documentModel, XHtmlDocumentExporterOptions options)
           : base(documentModel) {
            Guard.ArgumentNotNull(options, "options");
            this.options = options;
        }

        public virtual void Export(Stream outputStream) {
            this.outputStream = outputStream;
            using (StreamWriter streamWriter = new StreamWriter(outputStream, Encoding.UTF8)) {
                DocumentContentWriter = streamWriter;

                DocumentContentWriter.Write("<div>");
                Export();
                DocumentContentWriter.Write("</div>");

                if (footnotes.Count > 0) {
                    DocumentContentWriter.Write("<div>");
                    DocumentContentWriter.Write("<hr style=\"width: 20%; margin: initial;\" />");
                    foreach (var item in footnotes) {
                        DocumentContentWriter.Write(item);
                    }
                    DocumentContentWriter.Write("</div>");
                }

                if (endnotes.Count > 0) {
                    DocumentContentWriter.Write("<div>");
                    DocumentContentWriter.Write("<hr style=\"width: 20%; margin: initial;\" />");
                    foreach (var item in endnotes) {
                        DocumentContentWriter.Write(item);
                    }
                    DocumentContentWriter.Write("</div>");
                }

                streamWriter.Flush();
            }
        }

        protected override void ExportTextRun(TextRun run) {
            string text = run.GetPlainText(PieceTable.TextBuffer);
            text = text.Replace("\v", "<br/>");

            if (!hyperlinkExporting) {
                var span = @"<span style=""";
                if (run.FontBold)
                    span += "font-weight: bold; ";
                if (run.FontItalic)
                    span += "font-style: italic; ";
                if (run.FontUnderlineType != UnderlineType.None)
                    span += "text-decoration: underline; ";
                if (run.FontStrikeoutType != StrikeoutType.None)
                    span += "text-decoration: line-through; ";
                if (run.ForeColorIndex != DevExpress.Office.Model.ColorModelInfoCache.EmptyColorIndex)
                    span += $"color: #{ColorTranslator.ToHtml(DocumentModel.GetColor(run.ForeColorIndex))}; ";
                if (run.DoubleFontSize != DocumentModel.DefaultCharacterProperties.DoubleFontSize)
                    span += $"font-size: {Math.Min(run.DoubleFontSize, 39)}px; ";

                span += $"\">{text}</span>";
                DocumentContentWriter.Write(span);
            }
            else {
                DocumentContentWriter.Write(text);
            }

            base.ExportTextRun(run);
        }

        protected override void ExportFieldCodeStartRun(FieldCodeStartRun run) {
            base.ExportFieldCodeStartRun(run);

            HyperlinkInfo hyperlinkInfo = GetHyperlinkInfo(run);
            if (hyperlinkInfo == null)
                return;

            DocumentContentWriter.Write($"<a href=\"{hyperlinkInfo.NavigateUri}\">");
            hyperlinkExporting = true;
        }
        protected override void ExportFieldResultEndRun(FieldResultEndRun run) {
            base.ExportFieldResultEndRun(run);
            if (GetHyperlinkInfo(run) == null)
                return;

            DocumentContentWriter.Write("</a>");
            hyperlinkExporting = false;
        }
        protected override void ExportInlinePictureRun(InlinePictureRun run) {
            var mem = new MemoryStream();
            run.Image.NativeImage.Save(mem, ImageFormat.Jpeg);
            var data = mem.GetBuffer();

            DocumentContentWriter.Write($@"<img  src=""data:image/png;base64, {Convert.ToBase64String(data)}"" />");

            base.ExportInlinePictureRun(run);
        }
        protected override void ExportDrawingObjectRun(DrawingObjectRun run) {
            var mem = new MemoryStream();
            run.DrawingObject.GetCachedImage(new Rectangle(run.DrawingObject.ExtendedBounds.X.ToInt(), run.DrawingObject.ExtendedBounds.Y.ToInt(), run.DrawingObject.ExtendedBounds.Width.ToInt(), run.DrawingObject.ExtendedBounds.Height.ToInt())).NativeImage.Save(mem, ImageFormat.Jpeg);
            var data = mem.GetBuffer();

            DocumentContentWriter.Write($@"<img  src=""data:image/png;base64, {Convert.ToBase64String(data)}"" />");

            base.ExportDrawingObjectRun(run);
        }
        protected override void ExportNonBreakingHyphenRun(NonBreakingHyphenRun run) {
            base.ExportNonBreakingHyphenRun(run);
            DocumentContentWriter.Write("&nbsp;");
        }
        protected override void ExportCustomRun(CustomRun run) {
            base.ExportCustomRun(run);
        }
        protected override void ExportFootNoteRun(FootnoteRun run) {
            base.ExportFootNoteRun(run);

            var methods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance); // "CreateFootNoteExportInfo"
            var dynMethod = methods.Where(x => x.Name == "CreateFootNoteExportInfo").FirstOrDefault();
            if (dynMethod.IsNotNull()) {
                var info = dynMethod.Invoke(this, new object[] { run }) as FootNoteExportInfo;
                if (info.IsNotNull()) {
                    DocumentContentWriter.Write($"<a href=\"#footnote{info.Number}\" style=\"vertical-align: super; font-size: smaller;\">{info.NumberText})</a>");

                    var text = info.Note.TextBuffer.ToString().Replace("#", "").Trim();
                    footnotes.Add($"<a id=\"footnote{info.Number}\" name=\"footnote{info.Number}\"></a><span style=\"vertical-align: super; font-size: smaller;\">{info.NumberText}</span>&nbsp;{text}<br/>");
                }
            }
        }
        protected override void ExportEndNoteRun(EndnoteRun run) {
            base.ExportEndNoteRun(run);

            var methods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance); // "CreateFootNoteExportInfo"
            var dynMethod = methods.Where(x => x.Name == "CreateFootNoteExportInfo").LastOrDefault();
            if (dynMethod.IsNotNull()) {
                var info = dynMethod.Invoke(this, new object[] { run }) as FootNoteExportInfo;
                if (info.IsNotNull()) {
                    DocumentContentWriter.Write($"<a href=\"#endnote{info.Number}\">{info.NumberText}</a>");

                    var text = info.Note.TextBuffer.ToString().Replace("#", "").Trim();
                    endnotes.Add($"<a id=\"endnote{info.Number}\" name=\"endnote{info.Number}\"></a><span style=\"vertical-align: super; font-size: smaller;\">{info.NumberText}</span>&nbsp;{text}<br/>");
                }
            }
        }

        protected override ParagraphIndex ExportParagraph(Paragraph paragraph) {
            var number = string.Empty;
            var isFirstNumber = false;
            try {
                number = GetNumberingListText(paragraph);
                if (number.IsNotNull()) {
                    isFirstNumber = number.Trim().StartsWith("1.");
                }
            }
            catch { }

            if (paragraph.ParagraphStyle.IsNotNull()) {
                if (number.IsNotNullOrEmpty()) {
                    inList = true;
                    if (isFirstNumber) {
                        DocumentContentWriter.Write("<ol>");
                    }
                    DocumentContentWriter.Write("<li>");
                }
                else if (paragraph.ParagraphStyle.StyleName.ToLower().Replace(" ", "") == "heading1") {
                    if (inList) {
                        inList = false;
                        DocumentContentWriter.Write("</ol>");
                    }
                    DocumentContentWriter.Write("<h1>");

                }
                else if (paragraph.ParagraphStyle.StyleName.ToLower().Replace(" ", "") == "heading2") {
                    if (inList) {
                        inList = false;
                        DocumentContentWriter.Write("</ol>");
                    }
                    DocumentContentWriter.Write("<h2>");
                }
                else if (paragraph.ParagraphStyle.StyleName.ToLower().Replace(" ", "") == "heading3") {
                    if (inList) {
                        inList = false;
                        DocumentContentWriter.Write("</ol>");
                    }
                    DocumentContentWriter.Write("<h3>");
                }
                else if (paragraph.ParagraphStyle.StyleName.ToLower().Replace(" ", "") == "heading4") {
                    if (inList) {
                        inList = false;
                        DocumentContentWriter.Write("</ol>");
                    }
                    DocumentContentWriter.Write("<h4>");
                }
                else if (paragraph.ParagraphStyle.StyleName.ToLower().Replace(" ", "") == "heading5") {
                    if (inList) {
                        inList = false;
                        DocumentContentWriter.Write("</ol>");
                    }
                    DocumentContentWriter.Write("<h5>");
                }
                else if (paragraph.ParagraphStyle.StyleName.ToLower().Replace(" ", "") == "quote") {
                    if (inList) {
                        inList = false;
                        DocumentContentWriter.Write("</ol>");
                    }
                    DocumentContentWriter.Write(@"<p class=""mt-3 quote"">");
                }
                else if (paragraph.ParagraphStyle.StyleName.ToLower().Replace(" ", "") == "normal") {
                    if (inList) {
                        inList = false;
                        DocumentContentWriter.Write("</ol>");
                    }

                    if (paragraph.Alignment == ParagraphAlignment.Center) {
                        DocumentContentWriter.Write(@"<p class=""fs-5 mt-3"" style=""text-align: center"">");
                    }
                    else {
                        DocumentContentWriter.Write(@"<p class=""fs-5 mt-3"">");
                    }
                }
            }
            else {
                if (inList) {
                    inList = false;
                    DocumentContentWriter.Write("</ol>");
                }
                DocumentContentWriter.Write(@"<p class=""fs-5 mt-3"">");
            }

            var pi = base.ExportParagraph(paragraph);

            if (paragraph.ParagraphStyle.IsNotNull()) {
                if (number.IsNotNullOrEmpty()) {
                    DocumentContentWriter.Write("</li>");
                }
                else if (paragraph.ParagraphStyle.StyleName.ToLower().Replace(" ", "") == "heading1") {
                    DocumentContentWriter.Write("</h1>");
                }
                else if (paragraph.ParagraphStyle.StyleName.ToLower().Replace(" ", "") == "heading2") {
                    DocumentContentWriter.Write("</h2>");
                }
                else if (paragraph.ParagraphStyle.StyleName.ToLower().Replace(" ", "") == "heading3") {
                    DocumentContentWriter.Write("</h3>");
                }
                else if (paragraph.ParagraphStyle.StyleName.ToLower().Replace(" ", "") == "heading4") {
                    DocumentContentWriter.Write("</h4>");
                }
                else if (paragraph.ParagraphStyle.StyleName.ToLower().Replace(" ", "") == "heading5") {
                    DocumentContentWriter.Write("</h5>");
                }
                else if (paragraph.ParagraphStyle.StyleName.ToLower().Replace(" ", "") == "normal") {
                    DocumentContentWriter.Write(@"</p>");
                }
                else if (paragraph.ParagraphStyle.StyleName.ToLower().Replace(" ", "") == "quote") {
                    DocumentContentWriter.Write(@"</p>");
                }
            }
            else {
                DocumentContentWriter.Write("</p>");
            }

            return pi;
        }

        HyperlinkInfo GetHyperlinkInfo(TextRun run) {
            RunIndex runIndex = run.GetRunIndex();
            Field field = PieceTable.FindFieldByRunIndex(runIndex);
            System.Diagnostics.Debug.Assert(field != null);
            HyperlinkInfo hyperlinkInfo = null;
            if (PieceTable.HyperlinkInfos.TryGetHyperlinkInfo(field.Index, out hyperlinkInfo))
                return hyperlinkInfo;
            return null;
        }
    }
    public static class XHtmlDocumentFormat {
        public static readonly DocumentFormat Id = new DocumentFormat(431);
    }
    public class XHtmlDocumentExporterOptions : DocumentExporterOptions {
        protected override DocumentFormat Format { get { return XHtmlDocumentFormat.Id; } }
    }
}
