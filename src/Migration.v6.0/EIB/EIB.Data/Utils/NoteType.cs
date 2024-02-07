using System.Xml.Serialization;

namespace EIB.Data.Utils {
    public enum NoteType {
        [XmlEnum("")] Default = 0,
        [XmlEnum("cr")] CrossReference = 1
    };
}
