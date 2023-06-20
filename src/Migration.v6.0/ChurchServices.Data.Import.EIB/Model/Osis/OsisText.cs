using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Osis {
    public class OsisText {
        [XmlAttribute("osisIDWork")] public string WorkId { get; set; }
        [XmlAttribute("osisRefWork")] public string WorkRef { get; set; } = "Bible";
        [XmlAttribute("xml:lang")] public string Language { get; set; } = "pl";

        [XmlElement("header")] public OsisTextHeader Header { get; set; }
        [XmlElement("div")] public List<OsisDivision> Divisions { get; set; }
    }
}
