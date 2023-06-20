using ChurchServices.Extensions;
using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Osis {
    public class OsisChapter {
        [XmlAttribute("osisID")] public string Id { get; set; }
        [XmlAttribute("sID")] public string StartId { get; set; }
        [XmlAttribute("eID")] public string EndId { get; set; }
        [XmlAttribute("oldID")] public string OldId { get; set; }
        [XmlIgnore] public OsisElementType ElementType => !String.IsNullOrEmpty(StartId) ? OsisElementType.Start : OsisElementType.End;
        [XmlIgnore] public int Number => Id != null ? Id.Substring(Id.LastIndexOf('.') + 1).ToInt() : 0;

        public bool ShouldSerializeOldId() => !String.IsNullOrEmpty(OldId);
        public bool ShouldSerializeEndId() => !String.IsNullOrEmpty(EndId);
        public bool ShouldSerializeStartId() => !String.IsNullOrEmpty(StartId);
    }

    public enum OsisElementType {
        Start,
        End
    }
}
