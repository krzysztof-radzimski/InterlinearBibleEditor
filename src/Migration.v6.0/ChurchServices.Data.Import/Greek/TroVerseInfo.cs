using ChurchServices.Extensions;
using System.Xml.Linq;

namespace ChurchServices.Data.Import.Greek {
    class TroVerseInfo : IDisposable {
        public int Book { get; set; }
        public int Chapter { get; set; }
        public int Verse { get; set; }
        public List<TroVerseWordInfo> Words { get; }
        public TroVerseInfo(int book, int chapter, int verse, string data) {
            Words = new List<TroVerseWordInfo>();
            Book = book;
            Chapter = chapter;
            Verse = verse;
            var xmlText = $"<verse>{data}</verse>";
            var xml = XElement.Parse(xmlText);
            TroVerseWordInfo word = null;

            var wordIndex = 1;

            foreach (var node in xml.Nodes()) {
                if (node.NodeType == System.Xml.XmlNodeType.Text) {
                    if (!String.IsNullOrEmpty((node as XText).Value)) {
                        var text = (node as XText).Value;
                        if (text.Contains("–")) {
                            text = text.Substring(0, text.IndexOf('–') - 1).Trim();
                        }
                        word.Translation = text.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";").Trim();
                    }
                }
                else if (node is XElement) {
                    XElement el = node as XElement;
                    if (el.Name.LocalName == "e") {
                        word = new TroVerseWordInfo() {
                            Text = el.Value.Trim(),
                            WordIndex = wordIndex,
                            Transliterit2 = el.Value.Trim().TransliterateAncientGreek().ToLower()
                        };
                        Words.Add(word);
                        wordIndex++;
                    }
                    else if (el.Name.LocalName == "n") {
                        word.Transliterit = el.Value.Trim();
                    }
                    else if (el.Name.LocalName == "S") {
                        var code = Convert.ToInt32(el.Value.Trim());
                        word.StrongCode = code;
                    }
                    else if (el.Name.LocalName == "m") {
                        word.GrammarCode = el.Value;
                    }
                }
            }
        }

        public override string ToString() {
            if (Words.IsNotNull()) {
                var text = String.Empty;
                foreach (var item in Words) {
                    text += item.Text + " ";
                }
                return text.Trim();
            }
            return base.ToString();
        }

        public void Dispose() {

        }
    }

    public class TroBookInfo {
        public int NumberOfBook { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string BookColor { get; set; }
    }
}
