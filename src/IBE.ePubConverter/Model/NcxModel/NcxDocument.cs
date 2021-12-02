using System.Xml.Serialization;

namespace IBE.ePubConverter.Model.NcxModel {
    [XmlRoot(ElementName = "ncx", Namespace = "http://www.daisy.org/z3986/2005/ncx/")]
    public class NcxDocument {
        [XmlAttribute("version")] public string Version { get; set; }
        [XmlElement("head")] public NcxHead Head { get; set; }
        [XmlElement("docTitle")] public NcxDocTitle Title { get; set; }
        [XmlElement("navMap")] public NcxNavMap Map { get; set; }
    }
}
