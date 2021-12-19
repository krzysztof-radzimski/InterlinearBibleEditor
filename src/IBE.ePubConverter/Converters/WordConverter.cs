using IBE.ePubConverter.Model.NcxModel;
using Ionic.Zip;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace IBE.ePubConverter.Converters {
    internal class WordConverter : IConverter {
        public void Execute(string fileName) {
            if (String.IsNullOrWhiteSpace(fileName)) { throw new ArgumentNullException("fileName"); }
            if (!File.Exists(fileName)) { throw new FileNotFoundException(); }
            if (!Path.GetExtension(fileName).ToLower().Contains("epub")) { throw new Exception("Przekazany plik nie jest plikiem .epub!"); }

            var doc = GetDocument(fileName);
            if (doc != null) {
                doc.Save(fileName.Replace(".epub", ".docx"), new Aspose.Words.Saving.OoxmlSaveOptions(Aspose.Words.SaveFormat.Docx) {
                    UseHighQualityRendering = true
                });
            }
        }

        public Aspose.Words.Document GetDocument(string fileName) {
            new Aspose.Words.License().SetLicense(GetLicenseKeyFilePath());
            Aspose.Words.Document document = null;
            var dir = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(fileName));
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            using var zip = new ZipFile(fileName);
            zip.ExtractAll(dir, ExtractExistingFileAction.OverwriteSilently);

            var info = new DirectoryInfo(dir);
            var ncxInfo = info.GetFiles("*.ncx", SearchOption.AllDirectories).FirstOrDefault();
            if (ncxInfo != null && ncxInfo.Exists) {
                using (var stream = new FileStream(ncxInfo.FullName, FileMode.Open)) {
                    var serializer = new XmlSerializer(typeof(NcxDocument));
                    var ncx = serializer.Deserialize(stream) as NcxDocument;
                    if (ncx != null && ncx.Map != null && ncx.Map.Points != null && ncx.Map.Points.Count > 0) {
                        var html = @"<?xml version=""1.0"" encoding=""utf-8""?><html xmlns=""http://www.w3.org/1999/xhtml""><head><title>Tekst</title><style></style></head><body></body></html>";
                        var xhtml = XElement.Parse(html);

                        var baseDirectory = "";
                        var first = true;
                        foreach (var point in ncx.Map.Points.OrderBy(x => x.Order)) {
                            PopulatePoints(ncxInfo, xhtml, ref baseDirectory, ref first, point);
                        }

                        xhtml = RenumerateFootnotes(xhtml);
                        xhtml = RepairLinks(xhtml);
                        xhtml = RepairImages(xhtml);
                        xhtml = RepairChapters(xhtml);
                        xhtml = RepairChapters(xhtml, "numerrozdz", "nanieparzytsa");
                        xhtml = RepairChapters(xhtml, "numerrozdz1", "nanieparzytsa");

                        var xhtmlPath = Path.Combine(ncxInfo.DirectoryName, "TempFile.html");
                        xhtml.Save(xhtmlPath);

                        var options = new Aspose.Words.Loading.LoadOptions {
                            LoadFormat = Aspose.Words.LoadFormat.Html,
                            BaseUri = baseDirectory
                        };
                        document = new Aspose.Words.Document(xhtmlPath, options);
                        document.Styles.DefaultFont.LocaleId = (int)Aspose.Words.Loading.EditingLanguage.Polish;
                        document.Styles.DefaultFont.LocaleIdBi = (int)Aspose.Words.Loading.EditingLanguage.Polish;

                        // footnotes
                        var builder = new Aspose.Words.DocumentBuilder(document);
                        var list = new List<Aspose.Words.Fields.Field>();
                        var list2 = new List<Aspose.Words.Node>();
                        foreach (var field in document.Range.Fields) {
                            if (field.Type == Aspose.Words.Fields.FieldType.FieldHyperlink) {
                                var link = field as Aspose.Words.Fields.FieldHyperlink;
                                if (Regex.IsMatch(link.Result, @"^\[[0-9]+\]")) {
                                    var footnotePar = FindParagraph(document, link.Result);
                                    if (footnotePar != null) {
                                        list2.Add(footnotePar);
                                        var footnoteText = footnotePar.ToString(Aspose.Words.SaveFormat.Text).Replace(link.Result, "");
                                        builder.MoveToField(field, true);
                                        var f = builder.InsertFootnote(Aspose.Words.Notes.FootnoteType.Footnote, footnoteText.Trim());
                                        f.Font.Size = 12;
                                        f.Font.Color = System.Drawing.Color.Black;
                                        f.Font.Bold = true;
                                        f.Font.Underline = Aspose.Words.Underline.None;
                                        list.Add(field);
                                    }
                                }
                            }
                        }

                        if (list.Count > 0) {
                            var _list = list.ToArray();
                            var length = _list.Length;
                            for (int i = 0; i < length; i++) {
                                _list[i].Remove();
                            }
                        }
                        if (list2.Count > 0) {
                            var _list = list2.ToArray();
                            var length = _list.Length;
                            for (int i = 0; i < length; i++) {
                                try {
                                    (_list[i] as Aspose.Words.Paragraph).Remove();
                                }
                                catch { }
                            }
                        }

                        // headings
                        var chapterTitleStyle = document.Styles.Where(x => x.Name == "chapter_title").FirstOrDefault();
                        if (chapterTitleStyle != null) {
                            chapterTitleStyle.BaseStyleName = document.Styles[Aspose.Words.StyleIdentifier.Heading1].Name;
                        }
                        var subTitleStyle = document.Styles.Where(x => x.Name == "subtitle1" || x.Name == "subtitle").FirstOrDefault();
                        if (subTitleStyle != null) {
                            subTitleStyle.BaseStyleName = document.Styles[Aspose.Words.StyleIdentifier.Heading2].Name;
                        }
                        var subTitle2Style = document.Styles.Where(x => x.Name == "subtitle2").FirstOrDefault();
                        if (subTitle2Style != null) {
                            subTitle2Style.BaseStyleName = document.Styles[Aspose.Words.StyleIdentifier.Heading3].Name;
                        }
                    }
                }
            }

            Directory.Delete(dir, true);

            return document;
        }

        private Aspose.Words.Paragraph FindParagraph(Aspose.Words.Document document, string startsWith) {
            foreach (Aspose.Words.Section section in document.Sections) {
                var node = section.Body.Paragraphs.Where(x => x.ToString(Aspose.Words.SaveFormat.Text).Trim().StartsWith(startsWith)).LastOrDefault();
                if (node != null) { return node as Aspose.Words.Paragraph; }
            }
            return default;
        }

        private XElement RenumerateFootnotes(XElement xhtml) {
            try {
                var _footnotesRefs = xhtml.Descendants().Where(x => x.Name.LocalName == "a" && x.HasAttributes && x.Attribute("href") != null && Regex.IsMatch(x.Value, @"^\[[0-9\*]+\]") && x.PreviousNode != null);
                var footnoteRefs = _footnotesRefs.ToList();
                var index = 1;
                foreach (var item in footnoteRefs) {
                    item.Value = $"[{index}]";
                    var idAttr = item.Attribute("href").Value;
                    var id = idAttr.IndexOf("#") > -1 ? idAttr.Substring(idAttr.LastIndexOf("#") + 1) : idAttr;
                    var val = xhtml.Descendants().Where(x => x.Name.LocalName == "a" && x.Attribute("id") != null && x.Attribute("id").Value == id).FirstOrDefault();
                    if (val != null) {
                        if (!val.Value.Contains("[")) {
                            if (val.PreviousNode != null) {
                                if (val.PreviousNode is XText && (val.PreviousNode as XText).Value == "[") { val.PreviousNode.Remove(); }
                                else if (val.PreviousNode is XElement && (val.PreviousNode as XElement).Value == "[") { val.PreviousNode.Remove(); }
                            }
                        }
                        if (!val.Value.Contains("]")) {
                            if (val.NextNode != null) {
                                if (val.NextNode is XText && (val.NextNode as XText).Value.StartsWith("]")) { (val.NextNode as XText).Value = (val.NextNode as XText).Value.Substring(1); }
                                else if (val.NextNode is XElement && (val.NextNode as XElement).Value == "]") { val.NextNode.Remove(); }
                            }
                        }

                        val.Value = $"[{index}]";
                        var valParent = val.Ancestors().Where(y => y.Name.LocalName == "p" || y.Name.LocalName == "div").FirstOrDefault();
                        if (valParent != null) {
                            var nextElements = valParent.ElementsAfterSelf().Take(3);
                            var elementsToRemove = new List<XElement>();
                            foreach (var nextElement in nextElements) {
                                if (Regex.IsMatch(nextElement.Value, @"^Rozdział\s+[0-9\*]+") ||
                                    Regex.IsMatch(nextElement.Value, @"^Dodatek\s+[a-zA-Z0-9\*]+")) {
                                    break;
                                }
                                if (!nextElement.Elements().Where(x => x.Name.LocalName == "a" && Regex.IsMatch(x.Value, @"\[[0-9\*]+\]")).Any()) {
                                    valParent.Add(new XElement(XName.Get("br", valParent.Name.NamespaceName)));
                                    valParent.Add(nextElement.Nodes());
                                    elementsToRemove.Add(nextElement);
                                    continue;
                                }

                                break;
                            }

                            if (elementsToRemove.Count > 0) {
                                var len = elementsToRemove.Count;
                                for (int i = len - 1; i >= 0; i--) {
                                    elementsToRemove[i].Remove();
                                }
                            }
                        }
                    }
                    else {
                    }
                    index++;
                }
            }
            catch { }

            return xhtml;
        }
        private XElement RepairImages(XElement xhtml) {
            try {
                var images = xhtml.Descendants().Where(x => x.Name.LocalName == "img" && x.HasAttributes && x.Attribute("src") != null);
                foreach (var image in images) {
                    if (image.Attribute("style") == null) {
                        image.Add(new XAttribute("style", ""));
                    }
                    if (!string.IsNullOrWhiteSpace(image.Attribute("style").Value) && !image.Attribute("style").Value.Trim().EndsWith(";")) {
                        image.Attribute("style").Value += ";";
                    }
                    image.Attribute("style").Value += " max-width: 100%;";
                }
            }
            catch { }
            return xhtml;
        }
        private XElement RepairLinks(XElement xhtml) {
            try {
                var links = xhtml.Descendants().Where(x => x.Name.LocalName == "a" && x.HasAttributes && x.Attribute("href") != null);
                foreach (var link in links) {
                    var href = link.Attribute("href").Value;
                    if (href.IndexOf("#") > -1) {
                        href = href.Substring(href.LastIndexOf("#"));
                    }
                    if (!String.IsNullOrEmpty(href)) {
                        link.Attribute("href").Value = href;
                    }
                }
            }
            catch { }

            return xhtml;
        }
        private XElement RepairChapters(XElement xhtml, string chapterNumberClassName = "chapter_number", string chapterTitleClassName = "chapter_title") {
            try {
                var chapterNumbers = xhtml.Descendants().Where(x => x.HasAttributes && x.Attribute("class") != null && x.Attribute("class").Value == chapterNumberClassName).ToList();
                if (chapterNumbers.Count > 0) {
                    var elementsToRemove = new List<XElement>();
                    foreach (var chapterNumber in chapterNumbers) {
                        var titleEl = chapterNumber.ElementsAfterSelf().FirstOrDefault();
                        if (titleEl != null && titleEl.HasAttributes && titleEl.Attribute("class") != null && titleEl.Attribute("class").Value == chapterTitleClassName) {
                            var h1 = new XElement(XName.Get("h1", titleEl.Name.NamespaceName), titleEl.Attribute("class"));
                            h1.Add(chapterNumber.Nodes(), new XElement(XName.Get("br", titleEl.Name.NamespaceName)), titleEl.Nodes());

                            if (chapterNumber.Parent != null && chapterNumber.Parent.Name.LocalName == "h1") {
                                chapterNumber.Parent.AddBeforeSelf(h1);
                                elementsToRemove.Add(chapterNumber.Parent);
                            }
                            else {
                                chapterNumber.AddBeforeSelf(h1);

                                elementsToRemove.Add(chapterNumber);
                                elementsToRemove.Add(titleEl);
                            }
                        }
                    }


                    if (elementsToRemove.Count > 0) {
                        var len = elementsToRemove.Count;
                        for (int i = len - 1; i >= 0; i--) {
                            elementsToRemove[i].Remove();
                        }
                    }
                }
            }
            catch { }

            return xhtml;
        }

        private XElement PopulatePoints(FileInfo ncxInfo, XElement xhtml, ref string baseDirectory, ref bool first, NcxNavPoint point) {
            var body = xhtml.Elements().Where(x => x.Name.LocalName == "body").FirstOrDefault();
            var head = xhtml.Elements().Where(x => x.Name.LocalName == "head").FirstOrDefault();
            var style = head.Elements().Where(x => x.Name.LocalName == "style").FirstOrDefault();

            var pointFileName = point.Content.Uri;
            if (pointFileName.Contains("#")) {
                pointFileName = pointFileName.Substring(0, pointFileName.IndexOf("#"));
            }
            var pointFilePath = Path.Combine(ncxInfo.DirectoryName, pointFileName);
            if (File.Exists(pointFilePath)) {
                if (!first) {
                    body.Add(XElement.Parse(@"<p style=""page-break-after: always;""> </p>"));
                }
                first = false;

                if (baseDirectory == "") {
                    baseDirectory = Path.GetDirectoryName(pointFilePath);
                }

                var pointHtmlText = File.ReadAllText(pointFilePath);

                pointHtmlText = pointHtmlText.Replace("&shy;", "&#173;");
                pointHtmlText = pointHtmlText.Replace("&nbsp;", "&#160;");

                XElement pointHtml; try {
                    pointHtml = XElement.Parse(pointHtmlText);
                }
                catch (Exception ex) {
                    throw new Exception($"Błąd podczas ładowania pliku {pointFileName}!", ex);
                }
                var pointBody = pointHtml.Elements().Where(x => x.Name.LocalName == "body").FirstOrDefault();
                body.Add(pointBody.Nodes());

                var pointHead = pointHtml.Elements().Where(x => x.Name.LocalName == "head").FirstOrDefault();

                foreach (var item in pointHead.Elements()) {
                    if (item.Name.LocalName == "link" && item.HasAttributes && item.Attribute("rel").Value == "stylesheet") {
                        var linkHref = item.Attribute("href").Value;
                        if (!head.Elements().Where(x => x.Name.LocalName == "link" && x.HasAttributes && x.Attribute("href").Value == linkHref).Any()) {
                            head.Add(item);
                        }
                    }
                    else if (item.Name.LocalName == "style") {
                        style.Add(item.Nodes());
                    }
                }
            }

            if (point.Points != null && point.Points.Count > 0) {
                foreach (var item in point.Points.OrderBy(x => x.Order)) {
                    var itemFileName = item.Content.Uri;
                    if (itemFileName.Contains("#")) {
                        itemFileName = itemFileName.Substring(0, itemFileName.IndexOf("#"));
                    }

                    if (itemFileName == pointFileName) { continue; }

                    xhtml = PopulatePoints(ncxInfo, xhtml, ref baseDirectory, ref first, item);
                }
            }

            return xhtml;
        }

        private string GetLicenseKeyFilePath() {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile(Path.Combine(dir, "appsettings.json"), optional: false);

            IConfiguration config = builder.Build();
            var settings = config.GetSection("Settings").Get<Settings>();
            if (settings != null) {
                return settings.AsposeLicenseFilePath;
            }
            return "../../../../../db/Aspose.Total.lic";
        }
    }

    public class Settings {
        public string AsposeLicenseFilePath { get; set; }
    }
}
