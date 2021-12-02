using System.Xml.Serialization;

namespace IBE.ePubConverter.Model.NcxModel {
    public class NcxNavMap {
        [XmlElement("navPoint")]public List<NcxNavPoint> Points { get; set; }
    }
}
