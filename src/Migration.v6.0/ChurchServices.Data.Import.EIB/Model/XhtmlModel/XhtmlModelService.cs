using ChurchServices.Data.Import.EIB.Model.Bible;
using ChurchServices.Extensions;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace ChurchServices.Data.Import.EIB.Model {
    public class XhtmlModelService {
        public BookModel GetBook(string xhtmlFilePath) {
            if (File.Exists(xhtmlFilePath)) {
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.Load(xhtmlFilePath);
                var body = doc.DocumentNode.SelectSingleNode("//body");
                if (body != null) {
                    var contentDiv = body.Elements("div").FirstOrDefault();
                    if (contentDiv != null) {

                        var numOfChapters = contentDiv.Descendants("h2").Where(x => x.Attributes["id"] != null && x.Attributes["id"].Value.StartsWith("chapid")).Count();

                        var book = new BookModel() {
                            NumberOfBook = GetBookNumberFromFileNameWithoutExtension(Path.GetFileNameWithoutExtension(xhtmlFilePath)),
                            BookShortcut = GetBookShortcutFromFileNameWithoutExtension(Path.GetFileNameWithoutExtension(xhtmlFilePath)),
                            NumberOfChapters = numOfChapters,
                            Chapters = new List<ChapterModel>()
                        };

                        if (numOfChapters > 0) {
                            for (int i = 1; i <= numOfChapters; i++) {
                                book.Chapters.Add(new ChapterModel() {
                                    NumberOfChapter = i,
                                    Items = new List<object>()
                                });
                            }
                        }
                        else {
                            // for short books and letters
                            book.Chapters.Add(new ChapterModel() {
                                NumberOfChapter = 1,
                                Items = new List<object>()
                            });
                        }


                        ChapterModel currentChapter = null;
                        VerseModel verse = null;
                        if (book.Chapters.Count == 1) { currentChapter = book.Chapters.First(); }
                        var chapterNumber = 0;
                        foreach (var elem in contentDiv.ChildNodes) {
                            if (elem.Name == "div" && elem.HasChildNodes && elem.Elements("h2").Where(x => x.Attributes["id"] != null && x.Attributes["id"].Value.StartsWith("chapid")).Any()) {
                                chapterNumber++;
                                currentChapter = book.Chapters.Where(x => x.NumberOfChapter == chapterNumber).FirstOrDefault();
                            }
                            else if (elem.Name == "h2" && elem.Attributes["id"] != null && elem.Attributes["id"].Value.StartsWith("chapid")) {
                                chapterNumber++;
                                currentChapter = book.Chapters.Where(x => x.NumberOfChapter == chapterNumber).FirstOrDefault();
                            }
                            else if (elem.Name == "h3" && elem.HasClass("sigil_not_in_toc")) {
                                var title = new FormattedText() { Items = new List<object>() };
                                foreach (var item in elem.ChildNodes) {
                                    if (item.NodeType == HtmlAgilityPack.HtmlNodeType.Text) {
                                        title.Items.Add(new SpanModel(GetNodeText(item)));
                                    }
                                    else if (item.NodeType == HtmlAgilityPack.HtmlNodeType.Element) {
                                        if (item.Name == "br") {
                                            // title.Items.Add(new BreakLineModel());
                                            title.Items.Add(new SpanModel(" "));
                                        }
                                    }
                                }
                                currentChapter.Items.Add(title);
                            }
                            else if (elem.Name == "h4" && elem.HasClass("sigil_not_in_toc")) {
                                var title = new FormattedText() { Items = new List<object>() };
                                foreach (var item in elem.ChildNodes) {
                                    if (item.NodeType == HtmlAgilityPack.HtmlNodeType.Text) {
                                        title.Items.Add(new SpanModel(GetNodeText(item)));
                                    }
                                    else if (item.NodeType == HtmlAgilityPack.HtmlNodeType.Element) {
                                        if (item.Name == "br") {
                                            // title.Items.Add(new BreakLineModel());
                                            title.Items.Add(new SpanModel(" "));
                                        }
                                    }
                                }
                                currentChapter.Items.Add(title);
                            }
                            else if (elem.Name == "p" && ((elem.FirstChild.Name == "sup" && elem.FirstChild.HasClass("ver")) || (elem.HasClass("poezja") && verse != null) || (verse != null && elem.PreviousSibling != null && GetPreviousSibilingElement(elem, "poezja") != null))) {
                                foreach (var item in elem.ChildNodes) {
                                    if (item.Name == "sup" && item.HasClass("ver")) {
                                        int verseNum = item.InnerText.Substring(item.InnerText.IndexOf(".")).Replace(".", "").ToInt();
                                        verse = new VerseModel() {
                                            Items = new List<object>(),
                                            NumberOfVerse = verseNum
                                        };
                                        currentChapter.Items.Add(verse);
                                    }
                                    else if (item.NodeType == HtmlAgilityPack.HtmlNodeType.Text) {
                                        verse.Items.Add(new SpanModel(GetNodeText(item).Trim() + " "));
                                    }
                                    else if (item.NodeType == HtmlAgilityPack.HtmlNodeType.Element) {
                                        if (item.Name == "br") {
                                            if (book.NumberOfBook == 230) {
                                                verse.Items.Add(new BreakLineModel());
                                            }
                                            else {
                                                if (verse.Items.Last() != null && verse.Items.Last() is SpanModel && !(verse.Items.Last() as SpanModel).ToString().EndsWith(" ")) {
                                                    verse.Items.Add(new SpanModel("\u00A0"));
                                                }
                                            }
                                        }
                                        else if (item.Name == "i") {
                                            verse.Items.Add(new SpanModel(GetNodeText(item).Trim() + " ") { Italic = true });
                                        }
                                        else if (item.Name == "a" && item.Id != null && item.Id.StartsWith("_ftnref")) {
                                            var note = new NoteModel() {
                                                Number = item.InnerText.Replace("[", "").Replace("]", ""),
                                                Type = NoteType.Default,
                                                Items = new List<object>()
                                            };
                                            var pId = item.Attributes["href"].Value.Replace("#", "").Replace("_", "");
                                            var footNodePar = body.Descendants("div").Where(p => p.Id != null && p.Id == pId).FirstOrDefault();
                                            if (footNodePar != null && footNodePar.HasChildNodes) {

                                                foreach (var par in footNodePar.ChildNodes) {
                                                    if (par.Name != "p") { continue; }
                                                    foreach (var footNodeParItem in par.ChildNodes) {
                                                        if (footNodeParItem.Name == "a") { continue; }

                                                        var textNode = GetNodeText(footNodeParItem);

                                                        if (footNodeParItem.NodeType == HtmlAgilityPack.HtmlNodeType.Text) {
                                                            if (footNodeParItem.InnerText.IsNotNullOrEmpty()) {
                                                                var obj = RecognizeBibleTags(textNode);
                                                                if (obj == null) {
                                                                    note.Items.Add(new SpanModel(textNode.Trim() + " "));
                                                                }
                                                                else {
                                                                    note.Items.AddRange(obj);
                                                                }
                                                            }
                                                        }
                                                        else if (footNodeParItem.NodeType == HtmlAgilityPack.HtmlNodeType.Element) {
                                                            if (footNodeParItem.Name == "i") {
                                                                if (footNodeParItem.InnerText.IsNotNullOrEmpty()) {
                                                                    var obj = RecognizeBibleTags(textNode);
                                                                    if (obj == null) {
                                                                        note.Items.Add(new SpanModel(textNode.Trim() + " ") { Italic = true });
                                                                    }
                                                                    else {
                                                                        note.Items.AddRange(obj);
                                                                    }
                                                                }
                                                            }
                                                            else if (footNodeParItem.Name == "span" && footNodeParItem.InnerText.IsNotNullOrEmpty()) {
                                                                if (footNodeParItem.InnerText.IsNotNullOrEmpty()) {
                                                                    var span = new SpanModel(textNode.Trim() + " ");
                                                                    if (footNodeParItem.Attributes["lang"] != null) {
                                                                        var lang = footNodeParItem.Attributes["lang"].Value.ToLower();
                                                                        if (lang == "el") { lang = "gr"; }
                                                                        span.Language = lang;
                                                                    }
                                                                    if (footNodeParItem.Attributes["dir"] != null) {
                                                                        span.Direction = footNodeParItem.Attributes["dir"].Value;
                                                                    }
                                                                    note.Items.Add(span);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else {
                                                throw new Exception("Footnote not found!");
                                            }

                                            verse.Items.Add(note);
                                        }
                                    }
                                }

                            }
                        }

                        return book;
                    }
                }
            }
            return default;
        }

        private HtmlNode GetPreviousSibilingElement(HtmlNode e, string className = null) {
            if (e != null && e.PreviousSibling != null) {
                if (e.PreviousSibling.NodeType == HtmlNodeType.Element && (className != null ? e.PreviousSibling.HasClass(className) : true)) {
                    return e.PreviousSibling;
                }
                else {
                    return GetPreviousSibilingElement(e.PreviousSibling, className);
                }
            }
            return null;
        }

        private object[] RecognizeBibleTags(string text) {
            const string pattern =
                   @"(?<book>([0-9]\s+)?[0-9\u0041-\u005A\u0061-\u007A\u00AA\u00B5\u00BA\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02C1\u02C6-\u02D1\u02E0-\u02E4\u02EC\u02EE\u0370-\u0374\u0376\u0377\u037A-\u037D\u0386\u0388-\u038A\u038C\u038E-\u03A1\u03A3-\u03F5\u03F7-\u0481\u048A-\u0527\u0531-\u0556\u0559\u0561-\u0587\u05D0-\u05EA\u05F0-\u05F2\u0620-\u064A\u066E\u066F\u0671-\u06D3\u06D5\u06E5\u06E6\u06EE\u06EF\u06FA-\u06FC\u06FF\u0710\u0712-\u072F\u074D-\u07A5\u07B1\u07CA-\u07EA\u07F4\u07F5\u07FA\u0800-\u0815\u081A\u0824\u0828\u0840-\u0858\u08A0\u08A2-\u08AC\u0904-\u0939\u093D\u0950\u0958-\u0961\u0971-\u0977\u0979-\u097F\u0985-\u098C\u098F\u0990\u0993-\u09A8\u09AA-\u09B0\u09B2\u09B6-\u09B9\u09BD\u09CE\u09DC\u09DD\u09DF-\u09E1\u09F0\u09F1\u0A05-\u0A0A\u0A0F\u0A10\u0A13-\u0A28\u0A2A-\u0A30\u0A32\u0A33\u0A35\u0A36\u0A38\u0A39\u0A59-\u0A5C\u0A5E\u0A72-\u0A74\u0A85-\u0A8D\u0A8F-\u0A91\u0A93-\u0AA8\u0AAA-\u0AB0\u0AB2\u0AB3\u0AB5-\u0AB9\u0ABD\u0AD0\u0AE0\u0AE1\u0B05-\u0B0C\u0B0F\u0B10\u0B13-\u0B28\u0B2A-\u0B30\u0B32\u0B33\u0B35-\u0B39\u0B3D\u0B5C\u0B5D\u0B5F-\u0B61\u0B71\u0B83\u0B85-\u0B8A\u0B8E-\u0B90\u0B92-\u0B95\u0B99\u0B9A\u0B9C\u0B9E\u0B9F\u0BA3\u0BA4\u0BA8-\u0BAA\u0BAE-\u0BB9\u0BD0\u0C05-\u0C0C\u0C0E-\u0C10\u0C12-\u0C28\u0C2A-\u0C33\u0C35-\u0C39\u0C3D\u0C58\u0C59\u0C60\u0C61\u0C85-\u0C8C\u0C8E-\u0C90\u0C92-\u0CA8\u0CAA-\u0CB3\u0CB5-\u0CB9\u0CBD\u0CDE\u0CE0\u0CE1\u0CF1\u0CF2\u0D05-\u0D0C\u0D0E-\u0D10\u0D12-\u0D3A\u0D3D\u0D4E\u0D60\u0D61\u0D7A-\u0D7F\u0D85-\u0D96\u0D9A-\u0DB1\u0DB3-\u0DBB\u0DBD\u0DC0-\u0DC6\u0E01-\u0E30\u0E32\u0E33\u0E40-\u0E46\u0E81\u0E82\u0E84\u0E87\u0E88\u0E8A\u0E8D\u0E94-\u0E97\u0E99-\u0E9F\u0EA1-\u0EA3\u0EA5\u0EA7\u0EAA\u0EAB\u0EAD-\u0EB0\u0EB2\u0EB3\u0EBD\u0EC0-\u0EC4\u0EC6\u0EDC-\u0EDF\u0F00\u0F40-\u0F47\u0F49-\u0F6C\u0F88-\u0F8C\u1000-\u102A\u103F\u1050-\u1055\u105A-\u105D\u1061\u1065\u1066\u106E-\u1070\u1075-\u1081\u108E\u10A0-\u10C5\u10C7\u10CD\u10D0-\u10FA\u10FC-\u1248\u124A-\u124D\u1250-\u1256\u1258\u125A-\u125D\u1260-\u1288\u128A-\u128D\u1290-\u12B0\u12B2-\u12B5\u12B8-\u12BE\u12C0\u12C2-\u12C5\u12C8-\u12D6\u12D8-\u1310\u1312-\u1315\u1318-\u135A\u1380-\u138F\u13A0-\u13F4\u1401-\u166C\u166F-\u167F\u1681-\u169A\u16A0-\u16EA\u1700-\u170C\u170E-\u1711\u1720-\u1731\u1740-\u1751\u1760-\u176C\u176E-\u1770\u1780-\u17B3\u17D7\u17DC\u1820-\u1877\u1880-\u18A8\u18AA\u18B0-\u18F5\u1900-\u191C\u1950-\u196D\u1970-\u1974\u1980-\u19AB\u19C1-\u19C7\u1A00-\u1A16\u1A20-\u1A54\u1AA7\u1B05-\u1B33\u1B45-\u1B4B\u1B83-\u1BA0\u1BAE\u1BAF\u1BBA-\u1BE5\u1C00-\u1C23\u1C4D-\u1C4F\u1C5A-\u1C7D\u1CE9-\u1CEC\u1CEE-\u1CF1\u1CF5\u1CF6\u1D00-\u1DBF\u1E00-\u1F15\u1F18-\u1F1D\u1F20-\u1F45\u1F48-\u1F4D\u1F50-\u1F57\u1F59\u1F5B\u1F5D\u1F5F-\u1F7D\u1F80-\u1FB4\u1FB6-\u1FBC\u1FBE\u1FC2-\u1FC4\u1FC6-\u1FCC\u1FD0-\u1FD3\u1FD6-\u1FDB\u1FE0-\u1FEC\u1FF2-\u1FF4\u1FF6-\u1FFC\u2071\u207F\u2090-\u209C\u2102\u2107\u210A-\u2113\u2115\u2119-\u211D\u2124\u2126\u2128\u212A-\u212D\u212F-\u2139\u213C-\u213F\u2145-\u2149\u214E\u2183\u2184\u2C00-\u2C2E\u2C30-\u2C5E\u2C60-\u2CE4\u2CEB-\u2CEE\u2CF2\u2CF3\u2D00-\u2D25\u2D27\u2D2D\u2D30-\u2D67\u2D6F\u2D80-\u2D96\u2DA0-\u2DA6\u2DA8-\u2DAE\u2DB0-\u2DB6\u2DB8-\u2DBE\u2DC0-\u2DC6\u2DC8-\u2DCE\u2DD0-\u2DD6\u2DD8-\u2DDE\u2E2F\u3005\u3006\u3031-\u3035\u303B\u303C\u3041-\u3096\u309D-\u309F\u30A1-\u30FA\u30FC-\u30FF\u3105-\u312D\u3131-\u318E\u31A0-\u31BA\u31F0-\u31FF\u3400-\u4DB5\u4E00-\u9FCC\uA000-\uA48C\uA4D0-\uA4FD\uA500-\uA60C\uA610-\uA61F\uA62A\uA62B\uA640-\uA66E\uA67F-\uA697\uA6A0-\uA6E5\uA717-\uA71F\uA722-\uA788\uA78B-\uA78E\uA790-\uA793\uA7A0-\uA7AA\uA7F8-\uA801\uA803-\uA805\uA807-\uA80A\uA80C-\uA822\uA840-\uA873\uA882-\uA8B3\uA8F2-\uA8F7\uA8FB\uA90A-\uA925\uA930-\uA946\uA960-\uA97C\uA984-\uA9B2\uA9CF\uAA00-\uAA28\uAA40-\uAA42\uAA44-\uAA4B\uAA60-\uAA76\uAA7A\uAA80-\uAAAF\uAAB1\uAAB5\uAAB6\uAAB9-\uAABD\uAAC0\uAAC2\uAADB-\uAADD\uAAE0-\uAAEA\uAAF2-\uAAF4\uAB01-\uAB06\uAB09-\uAB0E\uAB11-\uAB16\uAB20-\uAB26\uAB28-\uAB2E\uABC0-\uABE2\uAC00-\uD7A3\uD7B0-\uD7C6\uD7CB-\uD7FB\uF900-\uFA6D\uFA70-\uFAD9\uFB00-\uFB06\uFB13-\uFB17\uFB1D\uFB1F-\uFB28\uFB2A-\uFB36\uFB38-\uFB3C\uFB3E\uFB40\uFB41\uFB43\uFB44\uFB46-\uFBB1\uFBD3-\uFD3D\uFD50-\uFD8F\uFD92-\uFDC7\uFDF0-\uFDFB\uFE70-\uFE74\uFE76-\uFEFC\uFF21-\uFF3A\uFF41-\uFF5A\uFF66-\uFFBE\uFFC2-\uFFC7\uFFCA-\uFFCF\uFFD2-\uFFD7\uFFDA-\uFFDC]+)?\s+(((?<chapter>[0-9]+)\:(?<verseStart>[0-9]+)(\-(?<chapter2>[0-9]+)\:(?<verseEnd>[0-9]+))(?<lit>\s+L[\.\s])?)|((?<chapter>[0-9]+)\:(?<verseStart>[0-9]+)(\-(?<verseEnd>[0-9]+))?(?<lit>\s+L[\.\s])?))";
            //@"(?<book>([0-9]\s+)?[0-9\u0041-\u005A\u0061-\u007A\u00AA\u00B5\u00BA\u00C0-\u00D6\u00D8-\u00F6\u00F8-\u02C1\u02C6-\u02D1\u02E0-\u02E4\u02EC\u02EE\u0370-\u0374\u0376\u0377\u037A-\u037D\u0386\u0388-\u038A\u038C\u038E-\u03A1\u03A3-\u03F5\u03F7-\u0481\u048A-\u0527\u0531-\u0556\u0559\u0561-\u0587\u05D0-\u05EA\u05F0-\u05F2\u0620-\u064A\u066E\u066F\u0671-\u06D3\u06D5\u06E5\u06E6\u06EE\u06EF\u06FA-\u06FC\u06FF\u0710\u0712-\u072F\u074D-\u07A5\u07B1\u07CA-\u07EA\u07F4\u07F5\u07FA\u0800-\u0815\u081A\u0824\u0828\u0840-\u0858\u08A0\u08A2-\u08AC\u0904-\u0939\u093D\u0950\u0958-\u0961\u0971-\u0977\u0979-\u097F\u0985-\u098C\u098F\u0990\u0993-\u09A8\u09AA-\u09B0\u09B2\u09B6-\u09B9\u09BD\u09CE\u09DC\u09DD\u09DF-\u09E1\u09F0\u09F1\u0A05-\u0A0A\u0A0F\u0A10\u0A13-\u0A28\u0A2A-\u0A30\u0A32\u0A33\u0A35\u0A36\u0A38\u0A39\u0A59-\u0A5C\u0A5E\u0A72-\u0A74\u0A85-\u0A8D\u0A8F-\u0A91\u0A93-\u0AA8\u0AAA-\u0AB0\u0AB2\u0AB3\u0AB5-\u0AB9\u0ABD\u0AD0\u0AE0\u0AE1\u0B05-\u0B0C\u0B0F\u0B10\u0B13-\u0B28\u0B2A-\u0B30\u0B32\u0B33\u0B35-\u0B39\u0B3D\u0B5C\u0B5D\u0B5F-\u0B61\u0B71\u0B83\u0B85-\u0B8A\u0B8E-\u0B90\u0B92-\u0B95\u0B99\u0B9A\u0B9C\u0B9E\u0B9F\u0BA3\u0BA4\u0BA8-\u0BAA\u0BAE-\u0BB9\u0BD0\u0C05-\u0C0C\u0C0E-\u0C10\u0C12-\u0C28\u0C2A-\u0C33\u0C35-\u0C39\u0C3D\u0C58\u0C59\u0C60\u0C61\u0C85-\u0C8C\u0C8E-\u0C90\u0C92-\u0CA8\u0CAA-\u0CB3\u0CB5-\u0CB9\u0CBD\u0CDE\u0CE0\u0CE1\u0CF1\u0CF2\u0D05-\u0D0C\u0D0E-\u0D10\u0D12-\u0D3A\u0D3D\u0D4E\u0D60\u0D61\u0D7A-\u0D7F\u0D85-\u0D96\u0D9A-\u0DB1\u0DB3-\u0DBB\u0DBD\u0DC0-\u0DC6\u0E01-\u0E30\u0E32\u0E33\u0E40-\u0E46\u0E81\u0E82\u0E84\u0E87\u0E88\u0E8A\u0E8D\u0E94-\u0E97\u0E99-\u0E9F\u0EA1-\u0EA3\u0EA5\u0EA7\u0EAA\u0EAB\u0EAD-\u0EB0\u0EB2\u0EB3\u0EBD\u0EC0-\u0EC4\u0EC6\u0EDC-\u0EDF\u0F00\u0F40-\u0F47\u0F49-\u0F6C\u0F88-\u0F8C\u1000-\u102A\u103F\u1050-\u1055\u105A-\u105D\u1061\u1065\u1066\u106E-\u1070\u1075-\u1081\u108E\u10A0-\u10C5\u10C7\u10CD\u10D0-\u10FA\u10FC-\u1248\u124A-\u124D\u1250-\u1256\u1258\u125A-\u125D\u1260-\u1288\u128A-\u128D\u1290-\u12B0\u12B2-\u12B5\u12B8-\u12BE\u12C0\u12C2-\u12C5\u12C8-\u12D6\u12D8-\u1310\u1312-\u1315\u1318-\u135A\u1380-\u138F\u13A0-\u13F4\u1401-\u166C\u166F-\u167F\u1681-\u169A\u16A0-\u16EA\u1700-\u170C\u170E-\u1711\u1720-\u1731\u1740-\u1751\u1760-\u176C\u176E-\u1770\u1780-\u17B3\u17D7\u17DC\u1820-\u1877\u1880-\u18A8\u18AA\u18B0-\u18F5\u1900-\u191C\u1950-\u196D\u1970-\u1974\u1980-\u19AB\u19C1-\u19C7\u1A00-\u1A16\u1A20-\u1A54\u1AA7\u1B05-\u1B33\u1B45-\u1B4B\u1B83-\u1BA0\u1BAE\u1BAF\u1BBA-\u1BE5\u1C00-\u1C23\u1C4D-\u1C4F\u1C5A-\u1C7D\u1CE9-\u1CEC\u1CEE-\u1CF1\u1CF5\u1CF6\u1D00-\u1DBF\u1E00-\u1F15\u1F18-\u1F1D\u1F20-\u1F45\u1F48-\u1F4D\u1F50-\u1F57\u1F59\u1F5B\u1F5D\u1F5F-\u1F7D\u1F80-\u1FB4\u1FB6-\u1FBC\u1FBE\u1FC2-\u1FC4\u1FC6-\u1FCC\u1FD0-\u1FD3\u1FD6-\u1FDB\u1FE0-\u1FEC\u1FF2-\u1FF4\u1FF6-\u1FFC\u2071\u207F\u2090-\u209C\u2102\u2107\u210A-\u2113\u2115\u2119-\u211D\u2124\u2126\u2128\u212A-\u212D\u212F-\u2139\u213C-\u213F\u2145-\u2149\u214E\u2183\u2184\u2C00-\u2C2E\u2C30-\u2C5E\u2C60-\u2CE4\u2CEB-\u2CEE\u2CF2\u2CF3\u2D00-\u2D25\u2D27\u2D2D\u2D30-\u2D67\u2D6F\u2D80-\u2D96\u2DA0-\u2DA6\u2DA8-\u2DAE\u2DB0-\u2DB6\u2DB8-\u2DBE\u2DC0-\u2DC6\u2DC8-\u2DCE\u2DD0-\u2DD6\u2DD8-\u2DDE\u2E2F\u3005\u3006\u3031-\u3035\u303B\u303C\u3041-\u3096\u309D-\u309F\u30A1-\u30FA\u30FC-\u30FF\u3105-\u312D\u3131-\u318E\u31A0-\u31BA\u31F0-\u31FF\u3400-\u4DB5\u4E00-\u9FCC\uA000-\uA48C\uA4D0-\uA4FD\uA500-\uA60C\uA610-\uA61F\uA62A\uA62B\uA640-\uA66E\uA67F-\uA697\uA6A0-\uA6E5\uA717-\uA71F\uA722-\uA788\uA78B-\uA78E\uA790-\uA793\uA7A0-\uA7AA\uA7F8-\uA801\uA803-\uA805\uA807-\uA80A\uA80C-\uA822\uA840-\uA873\uA882-\uA8B3\uA8F2-\uA8F7\uA8FB\uA90A-\uA925\uA930-\uA946\uA960-\uA97C\uA984-\uA9B2\uA9CF\uAA00-\uAA28\uAA40-\uAA42\uAA44-\uAA4B\uAA60-\uAA76\uAA7A\uAA80-\uAAAF\uAAB1\uAAB5\uAAB6\uAAB9-\uAABD\uAAC0\uAAC2\uAADB-\uAADD\uAAE0-\uAAEA\uAAF2-\uAAF4\uAB01-\uAB06\uAB09-\uAB0E\uAB11-\uAB16\uAB20-\uAB26\uAB28-\uAB2E\uABC0-\uABE2\uAC00-\uD7A3\uD7B0-\uD7C6\uD7CB-\uD7FB\uF900-\uFA6D\uFA70-\uFAD9\uFB00-\uFB06\uFB13-\uFB17\uFB1D\uFB1F-\uFB28\uFB2A-\uFB36\uFB38-\uFB3C\uFB3E\uFB40\uFB41\uFB43\uFB44\uFB46-\uFBB1\uFBD3-\uFD3D\uFD50-\uFD8F\uFD92-\uFDC7\uFDF0-\uFDFB\uFE70-\uFE74\uFE76-\uFEFC\uFF21-\uFF3A\uFF41-\uFF5A\uFF66-\uFFBE\uFFC2-\uFFC7\uFFCA-\uFFCF\uFFD2-\uFFD7\uFFDA-\uFFDC]+)?\s+(?<chapter>[0-9]+)\:(?<verseStart>[0-9]+)(\-(?<verseEnd>[0-9]+))?(?<lit>\s+L[\.\s])?";
            if (text != null) {
                if (text.Trim().Length < 2) { return null; }
                if (!text.Any(Char.IsDigit)) { return null; }

                var matches = Regex.Matches(text, pattern);
                if (matches.Count > 0) {
                    var result = new List<object>();
                    var currentBook = "";
                    int currentIndex = 0;

                    foreach (Match match in matches) {
                        if (match.Success) {
                            if (match.Index > 0) {
                                var __text = text.Substring(currentIndex, match.Index - currentIndex);
                                RecognizeSequentialVerseNumbers(ref result, ref __text);
                                if (__text.IsNotNullOrEmpty()) result.Add(__text);
                            }
                            currentIndex = match.Index + match.Length;

                            var lit = match.Groups["lit"] != null && match.Groups["lit"].Success;
                            var book = match.Groups["book"] != null && match.Groups["book"].Success ? match.Groups["book"].Value : currentBook;
                            if (book.IsNotNullOrEmpty() && currentBook != book) { currentBook = book; }
                            var chapter = match.Groups["chapter"] != null && match.Groups["chapter"].Success ? match.Groups["chapter"].Value : null;
                            var chapter2 = match.Groups["chapter2"] != null && match.Groups["chapter2"].Success ? match.Groups["chapter2"].Value : null;
                            var verseStart = match.Groups["verseStart"] != null && match.Groups["verseStart"].Success ? match.Groups["verseStart"].Value : null;
                            var verseEnd = match.Groups["verseEnd"] != null && match.Groups["verseEnd"].Success ? match.Groups["verseEnd"].Value : null;

                            // tu trzeba zwalidować informację ... co udało się rozpoznać

                            NoteReferenceModel refModel = new(match.Value.Trim(), lit);
                            var refText = $"{GetShortcutFromEIBAbbreviation(currentBook)}.{chapter}";
                            if (verseStart != null) {
                                refText += $".{verseStart}";
                            }
                            if (chapter2 != null) {
                                refText += $"-";
                                if (verseEnd != null) {
                                    refText += $"{GetShortcutFromEIBAbbreviation(currentBook)}.{chapter2}.{verseEnd}";
                                }
                            }
                            else if (verseEnd != null) {
                                refText += $"-{GetShortcutFromEIBAbbreviation(currentBook)}.{chapter}.{verseEnd}";
                            }
                            refModel.Ref = refText;
                            result.Add(refModel);
                        }
                    }
                    if (currentIndex < text.Length) {
                        var __text = text.Substring(currentIndex);
                        RecognizeSequentialVerseNumbers(ref result, ref __text);
                        result.Add(__text);
                    }

                    return result.Count > 0 ? result.ToArray() : null;
                }
            }
            return null;
        }

        private void RecognizeSequentialVerseNumbers(ref List<object> result, ref string __text) {
            if (result.Count > 0 && result.Last() is NoteReferenceModel && __text.IsNotNullOrWhiteSpace() && __text.Any(Char.IsDigit)) {
                var versesMatchesAdded = false;
                var versesPattern = @"^((\s+)?(,)?(\s+)?(?<verse>[0-9]+))+";
                var versesMatches = Regex.Matches(__text, versesPattern);
                if (versesMatches.Count > 0) {
                    var __last = result.Last() as NoteReferenceModel;
                    foreach (Match verseMatch in versesMatches) {
                        if (verseMatch.Success) {
                            var verseGroup = verseMatch.Groups["verse"];
                            if (verseGroup != null && verseGroup.Success) {
                                foreach (Capture capture in verseGroup.Captures) {
                                    var verseNumber = capture.Value;
                                    var __ref = new NoteReferenceModel(capture.Value) {
                                        Ref = $"{__last.Index.BookShortcut}.{__last.Index.ChapterNumber}.{verseNumber}"
                                    };
                                    result.Add(", ");
                                    result.Add(__ref);
                                    versesMatchesAdded = true;
                                }
                            }
                        }
                    }
                }
                if (versesMatchesAdded) {
                    __text = Regex.Replace(__text, versesPattern, new MatchEvaluator(delegate (Match m) { return String.Empty; }));
                }
            }
        }

        private string GetShortcutFromEIBAbbreviation(string nameOfBook) {
            // Abbreviations of Old Testament Books of the Bible
            if (nameOfBook == "Rdz") { return "Ge"; }
            if (nameOfBook == "Wj") { return "Ex"; }
            if (nameOfBook == "Kpł") { return "Le"; }
            if (nameOfBook == "Lb") { return "Nu"; }
            if (nameOfBook == "Pwt") { return "Dt"; }

            if (nameOfBook == "Joz") { return "Jos"; }
            if (nameOfBook == "Sdz") { return "Jdg"; }
            if (nameOfBook == "Rt") { return "Ru"; }

            if (nameOfBook == "1Sm") { return "1Sa"; }
            if (nameOfBook == "2Sm") { return "2Sa"; }
            if (nameOfBook == "1Krl") { return "1Ki"; }
            if (nameOfBook == "2Krl") { return "2Ki"; }
            if (nameOfBook == "1Krn") { return "1Ch"; }
            if (nameOfBook == "2Krn") { return "2Ch"; }


            if (nameOfBook == "Ezd") { return "Ezr"; }
            if (nameOfBook == "Ne") { return "Ne"; }
            if (nameOfBook == "Neh") { return "Ne"; }
            if (nameOfBook == "Est") { return "Es"; }

            if (nameOfBook == "Hi") { return "Job"; }
            if (nameOfBook == "Jb") { return "Job"; }
            if (nameOfBook == "Ps") { return "Ps"; }
            if (nameOfBook == "Prz") { return "Pr"; }
            if (nameOfBook == "Kaz") { return "Ec"; }
            if (nameOfBook == "Kzn") { return "Ec"; }
            if (nameOfBook == "Pnp") { return "So"; }

            if (nameOfBook == "Iz") { return "Is"; }
            if (nameOfBook == "Jr") { return "Je"; }
            if (nameOfBook == "Tr") { return "La"; }
            if (nameOfBook == "Lm") { return "La"; }
            if (nameOfBook == "Ez") { return "Eze"; }
            if (nameOfBook == "Dn") { return "Da"; }
            if (nameOfBook == "Oz") { return "Ho"; }
            if (nameOfBook == "Jl") { return "Joe"; }
            if (nameOfBook == "Am") { return "Am"; }
            if (nameOfBook == "Ab") { return "Ob"; }
            if (nameOfBook == "Jo") { return "Jon"; }
            if (nameOfBook == "Jon") { return "Jon"; }
            if (nameOfBook == "Mi") { return "Mic"; }
            if (nameOfBook == "Na") { return "Na"; }
            if (nameOfBook == "Ha") { return "Hab"; }
            if (nameOfBook == "So") { return "Zep"; }
            if (nameOfBook == "Ag") { return "Hag"; }
            if (nameOfBook == "Za") { return "Zec"; }
            if (nameOfBook == "Ml") { return "Mal"; }
            if (nameOfBook == "Ma") { return "Mal"; }

            //Abbreviations of New Testament Books of the Bible
            if (nameOfBook == "Mt") { return "Mt"; }
            if (nameOfBook == "Mk") { return "Mk"; }
            if (nameOfBook == "Łk") { return "Lk"; }
            if (nameOfBook == "J") { return "Jn"; }
            if (nameOfBook == "Dz") { return "Ac"; }
            if (nameOfBook == "Rz") { return "Ro"; }
            if (nameOfBook == "1Kor") { return "1Co"; }
            if (nameOfBook == "2Kor") { return "2Co"; }
            if (nameOfBook == "Ga") { return "Ga"; }
            if (nameOfBook == "Ef") { return "Eph"; }
            if (nameOfBook == "Flp") { return "Php"; }
            if (nameOfBook == "Kol") { return "Col"; }
            if (nameOfBook == "1Tes") { return "1Th"; }
            if (nameOfBook == "1Ts") { return "1Th"; }
            if (nameOfBook == "2Tes") { return "2Th"; }
            if (nameOfBook == "2Ts") { return "2Th"; }
            if (nameOfBook == "1Tm") { return "1Ti"; }
            if (nameOfBook == "2Tm") { return "2Ti"; }
            if (nameOfBook == "Tt") { return "Tt"; }
            if (nameOfBook == "Flm") { return "Phm"; }
            if (nameOfBook == "Hbr") { return "Heb"; }
            if (nameOfBook == "Jk") { return "Jas"; }
            if (nameOfBook == "1P") { return "1Pe"; }
            if (nameOfBook == "2P") { return "2Pe"; }
            if (nameOfBook == "1J") { return "1Jn"; }
            if (nameOfBook == "2J") { return "2Jn"; }
            if (nameOfBook == "3J") { return "3Jn"; }
            if (nameOfBook == "Jud") { return "Jud"; }
            if (nameOfBook == "Jd") { return "Jud"; }
            if (nameOfBook == "Obj") { return "Re"; }

            // Abbreviations of Books in the Apocrypha
            if (nameOfBook == "Tob") { return "Tob"; }
            if (nameOfBook == "Jdt") { return "Jdt"; }
            if (nameOfBook == "1Mch") { return "1Mac"; }
            if (nameOfBook == "2Mch") { return "2Mac"; }
            if (nameOfBook == "Mdr") { return "Wis"; }
            if (nameOfBook == "Syr") { return "Sir"; }
            if (nameOfBook == "Ba") { return "Bar"; }
            if (nameOfBook == "3Mch") { return "3Mac"; }
            if (nameOfBook == "Efe") { return "Efe"; }
            if (nameOfBook == "Mag") { return "Mag"; }
            if (nameOfBook == "Trl") { return "Trl"; }
            if (nameOfBook == "Rzm") { return "Rzm"; }
            if (nameOfBook == "Fld") { return "Fld"; }
            if (nameOfBook == "Smy") { return "Smy"; }
            if (nameOfBook == "Pol") { return "Pol"; }
            if (nameOfBook == "Did") { return "Did"; }
            if (nameOfBook == "2Ezd") { return "2Esd"; }
            if (nameOfBook == "3Ezd") { return "3Esd"; }

            return default;
        }

        private string GetNodeText(HtmlNode node) {
            var s = node.InnerText.Replace("&nbsp;", "\u00A0").Replace("\r\n", " "); //.TrimStart();
            if (s.StartsWith("  ")) {
                s = " " + s.TrimStart();
            }
            return s;
        }

        private int GetBookNumberFromFileNameWithoutExtension(string fileName) {
            if (fileName == "01") { return 10; }
            if (fileName == "02") { return 20; }
            if (fileName == "03") { return 30; }
            if (fileName == "04") { return 40; }
            if (fileName == "05") { return 50; }

            if (fileName == "06") { return 60; }
            if (fileName == "07") { return 70; }
            if (fileName == "08") { return 80; }

            if (fileName == "09") { return 90; }
            if (fileName == "10") { return 100; }
            if (fileName == "11") { return 110; }
            if (fileName == "12") { return 120; }
            if (fileName == "13") { return 130; }
            if (fileName == "14") { return 140; }


            if (fileName == "15") { return 150; }
            if (fileName == "16") { return 160; }
            if (fileName == "17") { return 190; }

            if (fileName == "18") { return 220; }
            if (fileName == "19") { return 230; }
            if (fileName == "20") { return 240; }
            if (fileName == "21") { return 250; }
            if (fileName == "22") { return 260; }

            if (fileName == "23") { return 290; }
            if (fileName == "24") { return 300; }
            if (fileName == "25") { return 310; }
            if (fileName == "26") { return 330; }
            if (fileName == "27") { return 340; }
            if (fileName == "28") { return 350; }
            if (fileName == "29") { return 360; }
            if (fileName == "30") { return 370; }
            if (fileName == "31") { return 380; }
            if (fileName == "32") { return 390; }
            if (fileName == "33") { return 400; }
            if (fileName == "34") { return 410; }
            if (fileName == "35") { return 420; }
            if (fileName == "36") { return 430; }
            if (fileName == "37") { return 440; }
            if (fileName == "38") { return 450; }
            if (fileName == "39") { return 460; }

            if (fileName == "40") { return 470; }
            if (fileName == "41") { return 480; }
            if (fileName == "42") { return 490; }
            if (fileName == "43") { return 500; }
            if (fileName == "44") { return 510; }
            if (fileName == "45") { return 520; }
            if (fileName == "46") { return 530; }
            if (fileName == "47") { return 540; }
            if (fileName == "48") { return 550; }
            if (fileName == "49") { return 560; }
            if (fileName == "50") { return 570; }
            if (fileName == "51") { return 580; }
            if (fileName == "52") { return 590; }
            if (fileName == "53") { return 600; }
            if (fileName == "54") { return 610; }
            if (fileName == "55") { return 620; }
            if (fileName == "56") { return 630; }
            if (fileName == "57") { return 640; }
            if (fileName == "58") { return 650; }
            if (fileName == "59") { return 660; }
            if (fileName == "60") { return 670; }
            if (fileName == "61") { return 680; }
            if (fileName == "62") { return 690; }
            if (fileName == "63") { return 700; }
            if (fileName == "64") { return 710; }
            if (fileName == "65") { return 720; }
            if (fileName == "66") { return 730; }

            return default;
        }
        private string GetBookShortcutFromFileNameWithoutExtension(string fileName) {
            if (fileName == "01") { return "Ge"; }
            if (fileName == "02") { return "Ex"; }
            if (fileName == "03") { return "Le"; }
            if (fileName == "04") { return "Nu"; }
            if (fileName == "05") { return "Dt"; }

            if (fileName == "06") { return "Jos"; }
            if (fileName == "07") { return "Jdg"; }
            if (fileName == "08") { return "Ru"; }

            if (fileName == "09") { return "1Sa"; }
            if (fileName == "10") { return "2Sa"; }
            if (fileName == "11") { return "1Ki"; }
            if (fileName == "12") { return "2Ki"; }
            if (fileName == "13") { return "1Ch"; }
            if (fileName == "14") { return "2Ch"; }


            if (fileName == "15") { return "Ezr"; }
            if (fileName == "16") { return "Ne"; }
            if (fileName == "17") { return "Es"; }

            if (fileName == "18") { return "Job"; }
            if (fileName == "19") { return "Ps"; }
            if (fileName == "20") { return "Pr"; }
            if (fileName == "21") { return "Ec"; }
            if (fileName == "22") { return "So"; }

            if (fileName == "23") { return "Is"; }
            if (fileName == "24") { return "Je"; }
            if (fileName == "25") { return "La"; }
            if (fileName == "26") { return "Eze"; }
            if (fileName == "27") { return "Da"; }
            if (fileName == "28") { return "Ho"; }
            if (fileName == "29") { return "Joe"; }
            if (fileName == "30") { return "Am"; }
            if (fileName == "31") { return "Ob"; }
            if (fileName == "32") { return "Jon"; }
            if (fileName == "33") { return "Mic"; }
            if (fileName == "34") { return "Na"; }
            if (fileName == "35") { return "Hab"; }
            if (fileName == "36") { return "Zep"; }
            if (fileName == "37") { return "Hag"; }
            if (fileName == "38") { return "Zec"; }
            if (fileName == "39") { return "Mal"; }

            //Abbreviations of New Testament Books of the Bible
            if (fileName == "40") { return "Mt"; }
            if (fileName == "41") { return "Mk"; }
            if (fileName == "42") { return "Lk"; }
            if (fileName == "43") { return "Jn"; }
            if (fileName == "44") { return "Ac"; }
            if (fileName == "45") { return "Ro"; }
            if (fileName == "46") { return "1Co"; }
            if (fileName == "47") { return "2Co"; }
            if (fileName == "48") { return "Ga"; }
            if (fileName == "49") { return "Eph"; }
            if (fileName == "50") { return "Php"; }
            if (fileName == "51") { return "Col"; }
            if (fileName == "52") { return "1Th"; }
            if (fileName == "53") { return "2Th"; }
            if (fileName == "54") { return "1Ti"; }
            if (fileName == "55") { return "2Ti"; }
            if (fileName == "56") { return "Tt"; }
            if (fileName == "57") { return "Phm"; }
            if (fileName == "58") { return "Heb"; }
            if (fileName == "59") { return "Jas"; }
            if (fileName == "60") { return "1Pe"; }
            if (fileName == "61") { return "2Pe"; }
            if (fileName == "62") { return "1Jn"; }
            if (fileName == "63") { return "2Jn"; }
            if (fileName == "64") { return "3Jn"; }
            if (fileName == "65") { return "Jud"; }
            if (fileName == "66") { return "Re"; }

            return default;
        }
    }
}
