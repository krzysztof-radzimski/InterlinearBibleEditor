using System.Xml.Serialization;

namespace EIB.Data.Utils {
    public abstract class HtmlElement {
        [XmlAttribute("id")] public string Id { get; set; }
        [XmlAttribute("style")] public string Style { get; set; }
        [XmlAttribute("class")] public string Class { get; set; }

        public bool ShouldSerializeId() => Id != null;
        public bool ShouldSerializeStyle() => Style != null;
        public bool ShouldSerializeClass() => Class != null;
    }
}
