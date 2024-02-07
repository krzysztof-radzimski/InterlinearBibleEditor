using System.Xml.Serialization;

namespace EIB.Data.Utils {
    public class Hyperlink : HtmlElement {
        [XmlAttribute("href")] public string Href { get; set; }
        [XmlAttribute("target")] public string Target { get; set; } = null;
        [XmlText(typeof(string))] public string Text { get; set; }
        public bool ShouldSerializeTarget() => Target != null;
    }
}
