using ChurchServices.Extensions;
using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Osis {
    public class OsisDivision {
        [XmlAttribute("osisID")] public string Id { get; set; }
        [XmlAttribute("type")] public string TypeName { get; set; }

        [XmlElement("div", typeof(OsisDivision))]
        [XmlElement("chapter", typeof(OsisChapter))]
        [XmlElement("title", typeof(OsisTitle))]
        [XmlElement("verse", typeof(OsisVerse))]
        [XmlElement("note", typeof(OsisNote))]
        [XmlElement("span", typeof(OsisSpan))]
        [XmlElement("br", typeof(OsisBreakLine))]
        [XmlText(typeof(string))]
        public List<object> Items { get; set; }

        public OsisDivisionType GetDivType() => TypeName.GetEnumByXmlEnum<OsisDivisionType>();

        public void SetDivType(OsisDivisionType divType) { TypeName = divType.GetXmlEnum(); }
    }

 

  

    public enum OsisDivisionType {
        [XmlEnum("")] None,
        [XmlEnum("book")] Book,
        [XmlEnum("section")] Section
    }
}
