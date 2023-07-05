using System.Text;
using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Bible {
    public class VerseModel {
        [XmlAttribute("nr")] public int NumberOfVerse { get; set; }
        [XmlAttribute("nl")] public bool StartFromNewLine { get; set; } = false;
        [XmlAttribute("title")] public bool IsTitle{ get; set; } = false;
        [XmlAttribute("vs")] public VerseStyle Style { get; set; } = VerseStyle.Default;
      
        [XmlElement("w", typeof(VerseWordModel))]
        [XmlElement("br", typeof(BreakLine))]
        [XmlElement("n", typeof(NoteModel))]
        [XmlElement("gw", typeof(WordOfGod))]        
        [XmlElement("span", typeof(Span))]
        [XmlText(typeof(string))]
        public List<object> Items { get; set; }

        public bool ShouldSerializeStyle() => Style != VerseStyle.Default;
        public bool ShouldSerializeStartFromNewLine() => StartFromNewLine;
        public bool ShouldSerializeIsTitle() => IsTitle;
        public override string ToString() {
            if (Items != null) {
                var sb = new StringBuilder();
                foreach (object item in Items) {
                    if (item is Span || item is NoteModel) {
                        sb.Append(item.ToString());
                    }
                }
                return sb.ToString();
            }
            return base.ToString();
        }
    }

    public enum VerseStyle {
        [XmlEnum("")] Default = 0,
        [XmlEnum("ci")] CenterItalic = 1
    }
}
