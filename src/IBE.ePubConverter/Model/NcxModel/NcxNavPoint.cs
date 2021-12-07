using System.Xml.Serialization;

namespace IBE.ePubConverter.Model.NcxModel {
    public class NcxNavPoint {
        [XmlAttribute("id")] public string Id { get; set; }
        [XmlAttribute("playOrder")] public int Order { get; set; }
        [XmlElement("navLabel")] public NcxNavLabel Label { get; set; }
        [XmlElement("content")] public NcxNavContent Content { get; set; }
        [XmlElement("navPoint")] public List<NcxNavPoint> Points { get; set; }
        public bool IsFootnotesPoint() => (Id != null && Id.Contains("footnotes")) || (Content != null && Content.Uri.Contains("footnotes"));

    }

    public class NcxNavLabel {
        [XmlElement("text")] public string Text { get; set; }
    }
    public class NcxNavContent {
        [XmlAttribute("src")] public string Uri { get; set; }
    }
}
