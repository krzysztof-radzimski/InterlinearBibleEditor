using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Osis {
    public class OsisVerse : OsisChapter {
        [XmlAttribute("isTitle")] public bool IsTitle => Number == 0;
        public bool ShouldSerializeIsTitle() => IsTitle;
    }
}
