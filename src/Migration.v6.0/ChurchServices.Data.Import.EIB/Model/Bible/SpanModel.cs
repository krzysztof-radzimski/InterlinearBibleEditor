using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Bible {
    public class SpanModel : HtmlElement {
        [XmlAttribute("lang")] public string Language { get; set; } = null;
        [XmlAttribute("dir")] public string Direction { get; set; } = null;
        [XmlIgnore][XmlAttribute("html")] public string Html { get; set; } = null;
        [XmlIgnore] public bool RTL => Direction == "rtl";

        [XmlAttribute("i")] public bool Italic { get; set; }
        [XmlAttribute("b")] public bool Bold { get; set; }
        [XmlAttribute("u")] public bool Underline { get; set; }
        [XmlAttribute("sup")] public bool Sup { get; set; }

        [XmlElement("br", typeof(BreakLineModel))]
        [XmlElement("hr", typeof(HLine))]
        [XmlElement("a", typeof(Hyperlink))]
        [XmlText(typeof(string))]
        public List<object> Items { get; set; }
        public SpanModel() { if (Items == null) { Items = new List<object>(); } }
        public SpanModel(string text) : this() {
            Items.Add(text);
        }
        public SpanModel Parse(string text) {
            try {
                var e = XElement.Parse($"<root>{text}</root>");
                if (e != null) {
                    foreach (var item in e.Nodes()) {
                        if (item.NodeType == System.Xml.XmlNodeType.Text) {
                            Items.Add((item as XText).Value);
                        }
                        else if (item.NodeType == System.Xml.XmlNodeType.Element) { 
                        var _e = item as XElement;
                            if (_e != null) {
                                if (_e.Name.LocalName == "br") {
                                    Items.Add(new BreakLineModel());
                                }
                            }
                        }
                    }
                }
            }
            catch {
                Items.Add(text                   
                    .Replace("<br/>", ""));
            }
            return this;
        }

        public bool ShouldSerializeLanguage() => Language != null;
        public bool ShouldSerializeDirection() => Direction != null;
        public bool ShouldSerializeHtml() => Html != null;
        public bool ShouldSerializeItalic() => Italic;
        public bool ShouldSerializeBold() => Bold;
        public bool ShouldSerializeUnderline() => Underline;
        public bool ShouldSerializeSup() => Sup;
        public void MarkAsHebrew() {
            Direction = "rtl";
            Language = "he";
        }
        public void MarkAsGreek() {
            Direction = "ltr";
            Language = "gr";
        }
        public void MarkAsLatin() {
            Direction = "ltr";
            Language = "la";
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
