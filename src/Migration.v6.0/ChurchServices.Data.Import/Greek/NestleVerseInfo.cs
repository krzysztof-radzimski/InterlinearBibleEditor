using ChurchServices.Extensions;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ChurchServices.Data.Import.Greek {
    class NestleVerseInfo : IDisposable {
        public int Book { get; set; }
        public int Chapter { get; set; }
        public int Verse { get; set; }
        public List<VerseWordInfo> Words { get; }

        public NestleVerseInfo(int book, int chapter, int verse, string data) {
            Words = new List<VerseWordInfo>();
            Book = book;
            Chapter = chapter;
            Verse = verse;

            var wordIndex = 1;

            data = data.Replace("<t>", "");
            data = data.Replace("</t>", "");

            var xmlText = $"<verse>{data}</verse>";
            var xml = XElement.Parse(xmlText);
            VerseWordInfo word = null;
            foreach (var node in xml.Nodes()) {
                if (node.NodeType == System.Xml.XmlNodeType.Text) {
                    var text = (node as XText).Value;
                    if (text.IsNotNullOrEmpty()) {
                        text = text.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";").Trim();
                        if (text.IsNotNullOrEmpty()) {
                            word = new VerseWordInfo() {
                                Text = text, WordIndex = wordIndex
                            };
                            Words.Add(word);
                            wordIndex++;
                        }
                    }
                }
                else if (node is XElement) {
                    XElement el = node as XElement;
                    if (el.Name.LocalName == "S") {
                        var code = Convert.ToInt32(el.Value.Trim());
                        word.StrongCode = code;
                    }
                    else if (el.Name.LocalName == "m") {
                        word.GrammarCode = el.Value;
                    }
                }
            }
        }

        public void Dispose() { }
    }
}
