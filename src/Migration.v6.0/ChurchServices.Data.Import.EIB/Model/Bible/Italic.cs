using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Bible {
    public class Italic : HtmlElement {
        [XmlElement("u", typeof(Underline))]
        [XmlElement("b", typeof(Bold))]
        [XmlElement("br", typeof(BreakLine))]
        [XmlElement("hr", typeof(HLine))]
        [XmlElement("a", typeof(Hyperlink))]
        [XmlText(typeof(string))]
        public List<object> Items { get; set; }
        public Italic() { }
        public Italic(string text) {
            if (Items == null) { Items = new List<object>(); }
            Items.Add(text);
        }
    }
}
