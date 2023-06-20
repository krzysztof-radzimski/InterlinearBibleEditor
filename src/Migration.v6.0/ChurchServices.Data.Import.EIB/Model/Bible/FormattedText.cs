using System.Text;
using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Bible {
    public class FormattedText {
        [XmlElement("div", typeof(Div))]
        [XmlElement("p", typeof(Paragraph))]
        [XmlElement("span", typeof(Span))]
        [XmlElement("i", typeof(Italic))]
        [XmlElement("u", typeof(Underline))]
        [XmlElement("b", typeof(Bold))]
        [XmlElement("br", typeof(BreakLine))]
        [XmlElement("hr", typeof(HLine))]
        [XmlElement("a", typeof(Hyperlink))]
        [XmlElement("gw", typeof(WordOfGod))]
        [XmlText(typeof(string))]
        public List<object> Items { get; set; }

        public FormattedText() { }
        public FormattedText(string text) {
            if (Items == null) { Items = new List<object>(); }
            Items.Add(text);
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
