using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Osis {
    public class OsisNote {
        [XmlAttribute("n")] public string Number { get; set; }
        [XmlAttribute("osisID")] public string Id { get; set; }
        [XmlAttribute("osisRef")] public string Ref { get; set; }
        [XmlAttribute("oldID")] public string OldId { get; set; }
        [XmlAttribute("type")] public string NoteType { get; set; }

        [XmlText(typeof(string))]
        [XmlElement("reference", typeof(OsisNoteReference))]
        public List<object> Items { get; set; }

        [XmlIgnore] public bool IsCrossReference => NoteType == "crossReference";

        public bool ShouldSerializeOldId() => !String.IsNullOrEmpty(OldId);
        public bool ShouldSerializeNoteType()=> !String.IsNullOrEmpty(NoteType);
    }
    public class OsisNoteReference {
        [XmlAttribute("osisRef")] public string Ref { get; set; }
        [XmlText(typeof(string))] public string Text { get; set; }
    }
}
