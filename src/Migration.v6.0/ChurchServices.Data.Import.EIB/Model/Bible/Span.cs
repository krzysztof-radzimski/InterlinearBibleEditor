using System.Text;
using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Bible {
    public class Span : HtmlElement {
        [XmlAttribute("lang")] public string Language { get; set; } = null;
        [XmlAttribute("dir")] public string Direction { get; set; } = null;
        [XmlIgnore][XmlAttribute("html")] public string Html { get; set; } = null;
        [XmlIgnore] public bool RTL => Direction == "rtl";

        [XmlAttribute("i")] public bool Italic { get; set; }
        [XmlAttribute("b")] public bool Bold { get; set; }
        [XmlAttribute("u")] public bool Underline { get; set; }

        [XmlElement("br", typeof(BreakLine))]
        [XmlElement("hr", typeof(HLine))]
        [XmlElement("a", typeof(Hyperlink))]
        [XmlText(typeof(string))]
        public List<object> Items { get; set; }
        public Span() { }
        public Span(string text) {
            if (Items == null) { Items = new List<object>(); }
            Items.Add(text);
        }

        public bool ShouldSerializeLanguage() => Language != null;
        public bool ShouldSerializeDirection() => Direction != null;
        public bool ShouldSerializeHtml() => Html != null;
        public bool ShouldSerializeItalic() => Italic;
        public bool ShouldSerializeBold() => Bold;
        public bool ShouldSerializeUnderline() => Underline;

        public void MarkAsHebrew() {
            Direction = "rtl";
            Language = "he";
        }
        public void MarkAsGreek() {
            Direction = "ltr";
            Language = "gr";
        }
        public override string ToString() {
            if (Items != null) {
                var sb = new StringBuilder();
                foreach (object item in Items) {
                    if (item is string) {
                        sb.Append(item as string);
                    }
                }
                return sb.ToString();
            }
            return base.ToString();
        }
    }
}
