using ChurchServices.Extensions;
using DevExpress.CodeParser;
using DevExpress.Office;
using DevExpress.Office.Export;
using DevExpress.Office.Internal;
using DevExpress.Utils;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Export;
using DevExpress.XtraRichEdit.Internal;
using DevExpress.XtraRichEdit.Model;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace ChurchServices.WinApp.Controllers {
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
            using (var streamWriter = new StreamWriter(outputStream, Encoding.UTF8)) {
                DocumentContentWriter = streamWriter;

                DocumentContentWriter.Write("<div>");
                Export();
                DocumentContentWriter.Write("</div>");

                if (footnotes.Count > 0) {
                    DocumentContentWriter.Write("<div style=\"text-align: left;\">");
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

        private string GetTextRunText(TextRun run, string text) {
            text = text.Replace("\u00A0", " ");
            text = text.Replace("\v", " ");
            if (!hyperlinkExporting) {
                return text;
            }
            else {
                if (text.StartsWith("HYPERLINK")) { return String.Empty; }
                return text;
            }
        }
        private string GetTextRunText(TextRun run, PieceTable pieceTable) {
            string text = run.GetPlainText(pieceTable.TextBuffer);
            return GetTextRunText(run, text);         
        }

        private string GetTextRunHtml(TextRun run, string text) {
            text = text.Replace("\u00A0", "&nbsp;");
            text = text.Replace("\v", "<br/>");
            text = text.Replace("\t", "&emsp;");

            if (!hyperlinkExporting) {
                var span = @"<span";
                var _class = @" class=""";
                var style = @" style=""";
                if (run.Script == CharacterFormattingScript.Superscript)
                    style += "vertical-align: super; ";
                if (run.Script == CharacterFormattingScript.Subscript)
                    style += "vertical-align: sub; ";
                if (run.FontBold)
                    style += "font-weight: bold; ";
                if (run.FontItalic)
                    style += "font-style: italic; ";
                if (run.FontUnderlineType != UnderlineType.None)
                    style += "text-decoration: underline; ";
                if (run.FontStrikeoutType != StrikeoutType.None)
                    style += "text-decoration: line-through; ";
                if (run.ForeColorIndex != DevExpress.Office.Model.ColorModelInfoCache.EmptyColorIndex) {
                    var color = ColorTranslator.ToHtml(DocumentModel.GetColor(run.ForeColorIndex));
                    if (color == "#FF0000") {
                        _class += "text-danger";
                    }
                    else {
                        style += $"color: {ColorTranslator.ToHtml(DocumentModel.GetColor(run.ForeColorIndex)).ToLower()}; ";
                    }
                }

                style += @"""";
                _class += @"""";

                if (_class.Trim().Replace(" ", "") != @"class=""""") { span += _class; }
                if (_class.Trim().Replace(" ", "") != @"style=""""") { span += style; }
                span += $">{text}</span>";
                return span;
            }
            else {
                if (text.StartsWith("HYPERLINK")) { return String.Empty; }
                return text;
            }
        }

        private string GetTextRunHtml(TextRun run, PieceTable pieceTable) {
            string text = run.GetPlainText(pieceTable.TextBuffer);
            return GetTextRunHtml(run, text);
        }
        protected override void ExportTextRun(TextRun run) {
            string text = run.GetPlainText(PieceTable.TextBuffer);
            text = text.Replace("\u00A0", "&nbsp;");
            text = text.Replace("\v", "<br/>");
            text = text.Replace("\t", "&emsp;");

            if (!hyperlinkExporting) {
                var span = @"<span";
                var _class = @" class=""";
                var style = @" style=""";
                if (run.Script == CharacterFormattingScript.Superscript)
                    style += "vertical-align: super; ";
                if (run.Script == CharacterFormattingScript.Subscript)
                    style += "vertical-align: sub; ";
                if (run.FontBold)
                    style += "font-weight: bold; ";
                if (run.FontItalic)
                    style += "font-style: italic; ";
                if (run.FontUnderlineType != UnderlineType.None)
                    style += "text-decoration: underline; ";
                if (run.FontStrikeoutType != StrikeoutType.None)
                    style += "text-decoration: line-through; ";
                if (run.ForeColorIndex != DevExpress.Office.Model.ColorModelInfoCache.EmptyColorIndex) {
                    var color = ColorTranslator.ToHtml(DocumentModel.GetColor(run.ForeColorIndex));
                    if (color == "#FF0000") {
                        _class += "text-danger";
                    }
                    else {
                        style += $"color: {ColorTranslator.ToHtml(DocumentModel.GetColor(run.ForeColorIndex))}; ";
                    }
                }

                style += @"""";
                _class += @"""";

                if (_class.Trim() != @"class=""""") { span += _class; }
                if (_class.Trim() != @"style=""""") { span += style; }
                span += $">{text}</span>";
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
            var target = hyperlinkInfo.Target != null ? $" target=\"{hyperlinkInfo.Target}\"" : "";

            DocumentContentWriter.Write($"<a href=\"{hyperlinkInfo.NavigateUri}\" {target}>");
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
                    var titleText = "";
                    var hiperlinkText = "";

                    var text = $"<a id=\"footnote{info.Number}\" class=\"footnote-tag text-decoration-none\" name=\"footnote{info.Number}\" onclick=\"scrollToFootnoteRef('ref-fn-{info.Number}')\"><span style=\"vertical-align: super; font-size: smaller;\">{info.NumberText})</span></a>"; ;
                    var flag = false;
                    foreach (var item in info.Note.Runs) {
                        if (item.Type == RunType.FieldCodeStartRun) {
                            HyperlinkInfo hyperlinkInfo = GetHyperlinkInfo(item as FieldCodeStartRun, info.Note);
                            if (hyperlinkInfo == null) {
                                flag = true;
                            }
                            else {
                                hiperlinkText = "";
                                var target = hyperlinkInfo.Target != null ? $" target=\"{hyperlinkInfo.Target}\"" : "";
                                text += $"<a href=\"{hyperlinkInfo.NavigateUri}\" {target}>";
                                hyperlinkExporting = true;
                            }
                            continue;
                        }
                        else if (item.Type == RunType.FieldCodeEndRun) {
                            HyperlinkInfo hyperlinkInfo = GetHyperlinkInfo(item as FieldCodeEndRun, info.Note);
                            if (hyperlinkInfo == null) {
                                flag = false;
                            }
                            else {
                                if (hiperlinkText.IsNotNullOrEmpty()) {
                                    text += "</a>";
                                }
                                hyperlinkExporting = false;
                            }
                            continue;
                        }
                        else if (item.Type == RunType.TextRun) {
                            if (flag) { continue; }
                            var _run = item as TextRun;

                            var _text = _run.GetPlainText(info.Note.TextBuffer);
                            titleText += GetTextRunText(_run, _text);

                            if (_run.CharacterStyle != null && _run.CharacterStyle.LocalizedStyleName == "Hyperlink") {
                                hiperlinkText += GetTextRunText(_run, _text);
                            }
                            else {
                                if (hiperlinkText.IsNotNullOrEmpty()) {
                                    text += $"{hiperlinkText}</a>";
                                    hiperlinkText = "";
                                }
                                text += GetTextRunHtml(_run, _text);
                            }
                        }
                    }

                    if (hiperlinkText.IsNotNullOrEmpty()) {
                        text += $"{hiperlinkText}</a>";
                        hiperlinkText = "";
                    }

                    var footnoteRef = $"<a id=\"ref-fn-{info.Number}\" href=\"#footnote{info.Number}\" style=\"vertical-align: super; font-size: smaller;\" title=\"{titleText.Trim()}\">{info.NumberText})</a>";
                    DocumentContentWriter.Write(footnoteRef);

                    try {
                        if (text.Contains("<a href")) {
                            var xml = XElement.Parse($"<span>{text.Replace("&nbsp;", "<nbsp/>")}</span>", LoadOptions.PreserveWhitespace);
                            if (xml.Elements().Where(x => x.Name.LocalName == "a" && x.Value.IsNullOrEmpty()).Any()) {
                                var _start = false;
                                XElement _a = null; ;
                                foreach (var node in xml.Nodes()) {
                                    if (node.NodeType == System.Xml.XmlNodeType.Element) {
                                        var elem = node as XElement;
                                        if (elem.Name.LocalName == "a" && elem.Value.IsNullOrEmpty()) {
                                            _start = true;
                                            _a = elem;
                                            continue;
                                        }
                                        if (_start) {
                                            if (elem.Name.LocalName == "span" && elem.Attribute("style") != null && elem.Attribute("style").Value.Contains("color: blue")) {
                                                _a.Value += elem.Value;
                                                elem.Add(new XAttribute("remove", "true"));
                                            }
                                            else {
                                                _start = false;
                                            }
                                        }
                                    }
                                }
                                xml.Elements().Where(x => x.Attribute("remove").IsNotNull()).Remove();
                                text = "";
                                foreach (var node in xml.Nodes()) {
                                    if (node.NodeType == System.Xml.XmlNodeType.Element && (node as XElement).Name.LocalName == "nbsp") {
                                        text += "&nbsp;";
                                        continue;
                                    }
                                    text += node.ToString();
                                }
                            }
                        }
                    }
                    catch { }

                    footnotes.Add(text);
                    footnotes.Add("<br/>");
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
                else if (IsStyle(paragraph.ParagraphStyle, "heading1")) {
                    if (inList) {
                        inList = false;
                        DocumentContentWriter.Write("</ol>");
                    }
                    DocumentContentWriter.Write($@"<h1 id=""{Guid.NewGuid()}"">");

                }
                else if (IsStyle(paragraph.ParagraphStyle, "heading2")) {
                    if (inList) {
                        inList = false;
                        DocumentContentWriter.Write("</ol>");
                    }
                    DocumentContentWriter.Write($@"<h2 id=""{Guid.NewGuid()}"">");
                }
                else if (IsStyle(paragraph.ParagraphStyle, "heading3")) {
                    if (inList) {
                        inList = false;
                        DocumentContentWriter.Write("</ol>");
                    }
                    DocumentContentWriter.Write($@"<h3 id=""{Guid.NewGuid()}"">");
                }
                else if (IsStyle(paragraph.ParagraphStyle, "heading4")) {
                    if (inList) {
                        inList = false;
                        DocumentContentWriter.Write("</ol>");
                    }
                    DocumentContentWriter.Write($@"<h4 id=""{Guid.NewGuid()}"">");
                }
                else if (IsStyle(paragraph.ParagraphStyle, "heading5")) {
                    if (inList) {
                        inList = false;
                        DocumentContentWriter.Write("</ol>");
                    }
                    DocumentContentWriter.Write($@"<h5 id=""{Guid.NewGuid()}"">");
                }
                else if (IsStyle(paragraph.ParagraphStyle, "quote")) {
                    if (inList) {
                        inList = false;
                        DocumentContentWriter.Write("</ol>");
                    }
                    DocumentContentWriter.Write(@"<p class=""mt-3 quote"">");
                }
                else if (IsStyle(paragraph.ParagraphStyle, "normal")) {
                    if (inList) {
                        inList = false;
                        DocumentContentWriter.Write("</ol>");
                    }
                    var pclass = @" class=""fs-5 mt-3""";
                    var tstyle = "";
                    if (inTable) {
                        pclass = "";
                        tstyle = " margin-bottom: 0;";
                    }
                    if (paragraph.Alignment == ParagraphAlignment.Center) {
                        DocumentContentWriter.Write(@$"<p{pclass} style=""text-align: center;{tstyle}"">");
                    }
                    else if (paragraph.Alignment == ParagraphAlignment.Right) {
                        DocumentContentWriter.Write(@$"<p{pclass} style=""text-align: right;{tstyle}"">");
                    }
                    else {
                        DocumentContentWriter.Write(@$"<p{pclass} style=""{tstyle}"">");
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
                else if (IsStyle(paragraph.ParagraphStyle, "heading1")) {
                    DocumentContentWriter.Write("</h1>");
                }
                else if (IsStyle(paragraph.ParagraphStyle, "heading2")) {
                    DocumentContentWriter.Write("</h2>");
                }
                else if (IsStyle(paragraph.ParagraphStyle, "heading3")) {
                    DocumentContentWriter.Write("</h3>");
                }
                else if (IsStyle(paragraph.ParagraphStyle, "heading4")) {
                    DocumentContentWriter.Write("</h4>");
                }
                else if (IsStyle(paragraph.ParagraphStyle, "heading5")) {
                    DocumentContentWriter.Write("</h5>");
                }
                else if (IsStyle(paragraph.ParagraphStyle, "normal")) {
                    DocumentContentWriter.Write(@"</p>");
                }
                else if (IsStyle(paragraph.ParagraphStyle, "quote")) {
                    DocumentContentWriter.Write(@"</p>");
                }
            }
            else {
                DocumentContentWriter.Write("</p>");
            }

            return pi;
        }

        bool IsStyle(ParagraphStyle style, string styleName) {
            if (style.IsNotNull()) {
                if (style.StyleName.ToLower().Replace(" ", "") == styleName) { return true; }
                return IsStyle(style.Parent, styleName);
            }
            return default;
        }

        HyperlinkInfo GetHyperlinkInfo(TextRun run, PieceTable pieceTable = null) {
            RunIndex runIndex = run.GetRunIndex();
            if (pieceTable == null) { pieceTable = PieceTable; }
            Field field = pieceTable.FindFieldByRunIndex(runIndex);
            if (field != null) {
                HyperlinkInfo hyperlinkInfo = null;
                if (pieceTable.HyperlinkInfos.TryGetHyperlinkInfo(field.Index, out hyperlinkInfo))
                    return hyperlinkInfo;
            }
            return default;
        }

        private bool inTable = false;
        protected override ParagraphIndex ExportTable(TableInfo tableInfo) {
            inTable = true;
            DocumentContentWriter.Write(@"<table class=""table table-striped""");
            var idx = base.ExportTable(tableInfo);
            DocumentContentWriter.Write("</table>");
            inTable = false;
            return idx;
        }
        protected override void ExportRow(TableRow row, TableInfo tableInfo) {
            DocumentContentWriter.Write("<tr>");
            base.ExportRow(row, tableInfo);
            DocumentContentWriter.Write("</tr>");
        }
        protected override void ExportCell(TableCell cell, TableInfo tableInfo) {
            var s = "";
            if (cell.ColumnSpan > 1) {
                s += @$" colspan=""{cell.ColumnSpan}""";
            }
            DocumentContentWriter.Write($"<td{s}>");
            base.ExportCell(cell, tableInfo);
            DocumentContentWriter.Write("</td>");
        }
    }
    public static class XHtmlDocumentFormat {
        public static readonly DocumentFormat Id = new DocumentFormat(431);
    }
    public class XHtmlDocumentExporterOptions : DocumentExporterOptions {
        protected override DocumentFormat Format { get { return XHtmlDocumentFormat.Id; } }
    }
}
