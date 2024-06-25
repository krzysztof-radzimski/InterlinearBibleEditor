using ChurchServices.Extensions;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ChurchServices.Data.Import.Greek {
    class ByzVerseInfo : IDisposable {
        public List<ByzVerseWordInfo> Words { get; private set; }
        public int Book { get; set; }
        public int Chapter { get; set; }
        public int Verse { get; set; }
        /*
        Ἀζὼρ<S>107</S> <m>N-PRI</m> 
        δὲ<S>1161</S> <m>CONJ</m> 
        ἐγέννησεν<S>1080</S> <m>V-AAI-3S</m> 
        τὸν<S>3588</S> <m>T-ASM</m> 
        Σαδώκ<S>4524</S> <m>N-PRI</m>· 
        Σαδὼκ<S>4524</S> <m>N-PRI</m> 
        δὲ<S>1161</S> <m>CONJ</m> 
        ἐγέννησεν<S>1080</S> <m>V-AAI-3S</m> 
        τὸν<S>3588</S> <m>T-ASM</m> 
        Ἀχείμ<S>885</S> <m>N-PRI</m> <n>NA27/UBS4 variant:</n> { Ἀχείμ Ἀχεὶμ | Ἀχίμ Ἀχὶμ }· 
        Ἀχεὶμ<S>885</S> <m>N-PRI</m> 
        δὲ<S>1161</S> <m>CONJ</m> 
        ἐγέννησεν<S>1080</S> <m>V-AAI-3S</m> 
        τὸν<S>3588</S> <m>T-ASM</m>
        Ἐλιούδ<S>1664</S> <m>N-PRI</m>·
        */

        public ByzVerseInfo(int book, int chapter, int verse, string data) {
            Words = new List<ByzVerseWordInfo>();
            Book = book;
            Chapter = chapter;
            Verse = verse;

            var wordIndex = 1;
            var xmlText = $"<verse>{data}</verse>";
            var xml = XElement.Parse(xmlText);
            ByzVerseWordInfo word = null;
            foreach (var node in xml.Nodes()) {
                if (node.NodeType == System.Xml.XmlNodeType.Text) {
                    var text = (node as XText).Value;
                    if (text.Trim().StartsWith("{")) {
                        var pattern = @"\{(?<variant>.+)\}(?<nextWord>.+)";
                        var regex = new Regex(pattern);
                        var match = regex.Match(text.Trim());
                        if (match.Success) {
                            word.Variant = match.Groups["variant"].Value.Trim();

                            var _text = match.Groups["nextWord"].Value.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "|").Trim();
                            if (_text.IsNotNullOrEmpty()) {
                                word = new ByzVerseWordInfo() {
                                    WordIndex = wordIndex,
                                    Text = _text
                                };
                                Words.Add(word);
                                wordIndex++;
                            }
                        }
                    }
                    else {
                        var _text = text.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "|").Trim();
                        if (_text.IsNotNullOrEmpty()) {
                            word = new ByzVerseWordInfo() {
                                WordIndex = wordIndex,
                                Text = _text
                            };
                            Words.Add(word);
                            wordIndex++;
                        }
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
    class ByzVerseWordInfo : VerseWordInfo {
        public const string VARIANT_LABEL = "NA27/UBS4 variant:";
        public string Variant { get; set; }
    }
}
