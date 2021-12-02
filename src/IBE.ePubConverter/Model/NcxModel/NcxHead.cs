using System.Xml.Serialization;

namespace IBE.ePubConverter.Model.NcxModel {
    public class NcxHead {
        [XmlElement("meta")]public List<NcxHeadMeta> Metas { get; set; }
    }
    public class NcxHeadMeta {
        [XmlAttribute("name")] public string Name { get; set; }
        [XmlAttribute("content")] public string Content { get; set; }
    }
}
