using System.Xml.Serialization;

namespace EIB.Data.Utils {
    public class WordOfGodModel : HtmlElement {
        [XmlElement("br", typeof(BreakLineModel))]
        [XmlElement("hr", typeof(HLine))]
        [XmlElement("a", typeof(Hyperlink))]
        [XmlText(typeof(string))]
        public List<object> Items { get; set; }
        public WordOfGodModel() { }
        public WordOfGodModel(string text) {
            if (Items == null) { Items = new List<object>(); }
            Items.Add(text);
        }
    }
}
