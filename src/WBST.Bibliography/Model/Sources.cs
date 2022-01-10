using System.Collections.Generic;
using System.Xml.Serialization;

namespace WBST.Bibliography.Model {
    [XmlRoot(ElementName = "Sources", Namespace = "http://schemas.openxmlformats.org/officeDocument/2006/bibliography")]
    public class BibliographySources {
        [XmlAttribute] public int Version { get; set; }
        [XmlAttribute] public string StyleName { get; set; }
        [XmlAttribute] public string SelectedStyle { get; set; }
        [XmlElement(ElementName = "Source")] public List<BibliographySource> Sources { get; set; }
    }
}
