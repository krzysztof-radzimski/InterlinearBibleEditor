using IBE.ePubConverter.Model.NcxModel;
using Ionic.Zip;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace IBE.ePubConverter.Converters {
    internal class WordConverter : IConverter {
        public void Execute(string fileName) {
            var doc = GetDocument(fileName);
            if (doc != null) {
                doc.Save(fileName.Replace(".epub", ".docx"), new Aspose.Words.Saving.OoxmlSaveOptions(Aspose.Words.SaveFormat.Docx)
                {
                    UseHighQualityRendering = true
                });
            }
        }

        public Aspose.Words.Document GetDocument(string fileName) {
            new Aspose.Words.License().SetLicense("../../../../../db/Aspose.Total.lic");
            Aspose.Words.Document document = null;
            var dir = Path.Combine(Path.GetTempPath(), Path.GetFileName(fileName));
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
                        var body = xhtml.Elements().Where(x => x.Name.LocalName == "body").FirstOrDefault();
                        var head = xhtml.Elements().Where(x => x.Name.LocalName == "head").FirstOrDefault();
                        var style = head.Elements().Where(x => x.Name.LocalName == "style").FirstOrDefault();
                        var baseDirectory = "";
                        var first = true;
                        foreach (var point in ncx.Map.Points.OrderBy(x => x.Order)) {
                            var pointFileName = point.Content.Uri;
                            if (pointFileName.Contains("#")) {
                                pointFileName = pointFileName.Substring(0, pointFileName.IndexOf("#"));
                            }
                            var pointFilePath = Path.Combine(ncxInfo.DirectoryName, pointFileName);
                            if (File.Exists(pointFilePath)) {
                                if (!first) {
                                    xhtml.Add(XElement.Parse(@"<p style=""page-break-after: always;""> </p>"));
                                }
                                first = false;

                                if (baseDirectory == "") {
                                    baseDirectory = Path.GetDirectoryName(pointFilePath);
                                }

                                var pointHtmlText = File.ReadAllText(pointFilePath);
                                var pointHtml = XElement.Parse(pointHtmlText);
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
                        }

                        // renumerate footnotes
                        try {
                            var footnoteRefs = xhtml.Descendants().Where(x => x.Name.LocalName == "a" && Regex.IsMatch(x.Value, @"\[[0-9\*]+\]") && x.Ancestors().Where(y => y.Name.LocalName == "p" || y.Name.LocalName == "div").First().Elements().First() != x);
                            var index = 1;
                            foreach (var item in footnoteRefs) {
                                item.Value = $"[{index}]";
                                var idAttr = item.Attribute("href").Value;
                                var id = idAttr.Substring(idAttr.LastIndexOf("#") + 1);
                                var val = xhtml.Descendants().Where(x => x.Name.LocalName == "a" && x.Attribute("id") != null && x.Attribute("id").Value == id).FirstOrDefault();
                                if (val != null) {
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
                                            for (int i = len-1; i >=0; i--) {
                                                elementsToRemove[i].Remove();
                                            }
                                        }
                                    }
                                }
                                index++;
                            }
                        }
                        catch { }

                        var xhtmlPath = Path.Combine(ncxInfo.DirectoryName, "TempFile.html");
                        xhtml.Save(xhtmlPath);

                        var options = new Aspose.Words.Loading.LoadOptions
                        {
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
                                if (Regex.IsMatch(link.Result, @"\[[0-9]+\]")) {
                                    var footnotePar = document.LastSection.Body.Paragraphs.Where(x => x.ToString(Aspose.Words.SaveFormat.Text).Trim().StartsWith(link.Result)).LastOrDefault();
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
                    }
                }
            }

            Directory.Delete(dir, true);

            return document;
        }
    }
}
