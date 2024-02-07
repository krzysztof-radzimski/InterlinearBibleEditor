using System.Xml.Serialization;

namespace EIB.Data.Utils {
    public enum VerseStyle {
        [XmlEnum("")] Default = 0,
        [XmlEnum("ci")] CenterItalic = 1
    }
}
