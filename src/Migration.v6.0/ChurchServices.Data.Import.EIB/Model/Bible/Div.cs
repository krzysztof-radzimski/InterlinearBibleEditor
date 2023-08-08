using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Bible {
    public class Div : HtmlElement {
        [XmlElement("span", typeof(SpanModel))] 
        [XmlElement("br", typeof(BreakLineModel))]
        [XmlElement("hr", typeof(HLine))]
        [XmlElement("a", typeof(Hyperlink))]
        [XmlElement("gw", typeof(WordOfGodModel))]
        [XmlText(typeof(string))]
        public List<object> Items { get; set; }
        public Div() { }
        public Div(string text) {
            if (Items == null) { Items = new List<object>(); }
            Items.Add(text);
        }
    }
}
