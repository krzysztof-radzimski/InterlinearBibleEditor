using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Osis {
    [XmlRoot("osis", Namespace = "http://www.bibletechnologies.net/2003/OSIS/namespace")]
    public class OsisModel {
        [XmlElement(ElementName = "osisText")] public OsisText Text { get; set; }
    }
}
