using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Osis {
    public class OsisTextHeader {
        [XmlElement("work")] public OsisTextHeaderWork Work { get; set; }
    }

    public class OsisTextHeaderWork {
        [XmlAttribute("osisWork")] public string Work { get; set; }
        [XmlElement("title")] public string Title { get; set; }
        [XmlElement("scope")] public string Scope { get; set; } = "Gen-Rev";
    }
}
