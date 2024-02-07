using System.Text;
using System.Xml.Serialization;

namespace EIB.Data.Utils {
    [XmlRoot("v")]
    public class VerseModel {
        [XmlAttribute("style")] public VerseStyle Style { get; set; } = VerseStyle.Default;

        [XmlElement("w", typeof(VerseWordModel))]
        [XmlElement("br", typeof(BreakLineModel))]
        [XmlElement("n", typeof(NoteModel))]
        [XmlElement("gw", typeof(WordOfGodModel))]
        [XmlElement("span", typeof(SpanModel))]
        [XmlText(typeof(string))]
        public List<object> Items { get; set; }

        public override string ToString() {
            if (Items != null) {
                var sb = new StringBuilder();
                foreach (object item in Items) {
                    if (item is SpanModel || item is NoteModel) {
                        sb.Append(item.ToString());
                    }
                }
                return sb.ToString();
            }
            return base.ToString();
        }
        public bool ShouldSerializeStyle() => Style != VerseStyle.Default;        
    }
}
