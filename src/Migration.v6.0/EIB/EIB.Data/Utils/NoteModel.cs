using System.Xml.Serialization;

namespace EIB.Data.Utils {
    public class NoteModel {
        [XmlAttribute("n")] public string Number { get; set; }
        [XmlAttribute("t")] public NoteType Type { get; set; } = NoteType.Default;

        [XmlElement("br", typeof(BreakLineModel))]
        [XmlElement("a", typeof(Hyperlink))]
        [XmlElement("ref", typeof(NoteReferenceModel))]
        [XmlElement("span", typeof(SpanModel))]
        [XmlText(typeof(string))]
        public List<object> Items { get; set; }

        public bool ShouldSerializeType() => Type != NoteType.Default;

        public override string ToString() {
            if (Number != null) { return Number; }
            return base.ToString();
        }
    }
}
