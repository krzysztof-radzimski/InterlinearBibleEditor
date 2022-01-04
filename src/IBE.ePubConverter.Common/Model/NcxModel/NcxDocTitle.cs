using System.Xml.Serialization;

namespace IBE.ePubConverter.Common.Model.NcxModel {
    public class NcxDocTitle {
        [XmlElement("text")] public string Text { get; set; }
    }
}
