using System.Text;
using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Bible {
    public class FormattedText {
        [XmlElement("div", typeof(Div))]
        [XmlElement("p", typeof(Paragraph))]
        [XmlElement("span", typeof(SpanModel))]
        [XmlElement("br", typeof(BreakLineModel))]
        [XmlElement("hr", typeof(HLine))]
        [XmlElement("a", typeof(Hyperlink))]
        [XmlElement("gw", typeof(WordOfGodModel))]
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
                    else if (item is SpanModel) {
                        sb.Append((item as SpanModel).ToString());
                    }
                }
                return sb.ToString();
            }
            return base.ToString();
        }
    } 
}
