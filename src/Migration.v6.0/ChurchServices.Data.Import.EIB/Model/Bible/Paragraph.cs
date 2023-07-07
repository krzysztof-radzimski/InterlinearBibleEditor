using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Bible {
    public class Paragraph : HtmlElement {
        [XmlElement("div", typeof(Div))]
        [XmlElement("span", typeof(SpanModel))]    
        [XmlElement("br", typeof(BreakLine))]
        [XmlElement("hr", typeof(HLine))]
        [XmlElement("a", typeof(Hyperlink))]
        [XmlElement("gw", typeof(WordOfGod))]
        [XmlText(typeof(string))]
        public List<object> Items { get; set; }
        public Paragraph() { }
        public Paragraph(string text) {
            if (Items == null) { Items = new List<object>(); }
            Items.Add(text);
        }
    }
}
