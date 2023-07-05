using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Osis {
    public class OsisSpan {
        [XmlAttribute("lang")] public string Language { get; set; } = null;
        [XmlAttribute("dir")] public string Direction { get; set; } = null;
        [XmlAttribute("i")] public bool Italic { get; set; }
        [XmlAttribute("b")] public bool Bold { get; set; }
        [XmlText(typeof(string))] public string Value { get; set; }
        [XmlIgnore] public bool RTL => Direction == "rtl";
        public bool ShouldSerializeItalic() => Italic;
        public bool ShouldSerializeBold() => Bold;
        public bool ShouldSerializeLanguage() => Language != null;
        public bool ShouldSerializeDirection() => Direction != null;

    }
}
