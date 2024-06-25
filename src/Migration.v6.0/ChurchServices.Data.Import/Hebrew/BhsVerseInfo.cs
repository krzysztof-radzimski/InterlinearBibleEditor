using ChurchServices.Data.Import.Greek;
using ChurchServices.Extensions;
using System.Xml.Linq;

namespace ChurchServices.Data.Import.Hebrew {
    class BhsVerseInfo {
        public List<VerseWordInfo> Words { get; private set; }
        public int Book { get; private set; }
        public int Chapter { get; private set; }
        public int Verse { get; private set; }

        /*
בְּ<S>9001</S><m>prep</m>
רֵאשִׁ֖ית<S>7225</S><m>n.fs.a</m>
בָּרָ֣א<S>1254</S><m>v.qal.pf.3ms</m>
אֱלֹהִ֑ים<S>430</S><m>n.mp.a</m>
אֵ֥ת<S>853</S><m>prep</m>
הַ<S>9006</S><m>art</m>שָּׁמַ֖יִם
<S>8064</S><m>n.mp.a</m>
וְ<S>9005</S><m>conj</m>אֵ֥ת
<S>853</S><m>prep</m>
הָ<S>9006</S><m>art</m>
אָֽרֶץ<S>776</S><m>n.us.a</m>
׃
*/
        public BhsVerseInfo(int book, int chapter, int verse, string data) {
            Words = new List<VerseWordInfo>();
            Book = book;
            Chapter = chapter;
            Verse = verse;

            if (data.IsNotNullOrEmpty()) {
                var wordIndex = 1;
                var xmlText = $"<verse>{data}</verse>";
                var xml = XElement.Parse(xmlText);

                VerseWordInfo word = null;
                foreach (var node in xml.Nodes()) {
                    if (node.NodeType == System.Xml.XmlNodeType.Text) {
                        var text = (node as XText).Value;
                        if (text.IsNotNullOrWhiteSpace()) {
                            word = new VerseWordInfo() {
                                WordIndex = wordIndex,
                                Text = text
                            };
                            Words.Add(word);
                            wordIndex++;
                        }
                    }
                    else if (node.NodeType == System.Xml.XmlNodeType.Element) {
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
        }
    }   
}
