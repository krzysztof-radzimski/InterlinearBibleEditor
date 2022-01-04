using System.Xml.Serialization;

namespace IBE.ePubConverter.Common.Model.NcxModel {
    public class NcxNavMap {
        [XmlElement("navPoint")]public List<NcxNavPoint> Points { get; set; }
    }
}
