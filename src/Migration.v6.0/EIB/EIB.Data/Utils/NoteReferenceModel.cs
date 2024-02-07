using System.Xml.Serialization;

namespace EIB.Data.Utils {
    public class NoteReferenceModel {
        [XmlAttribute("ref")] public string Ref { get; set; }
        [XmlAttribute("lit")] public bool LiteratureAndNotes { get; set; } = false;
        [XmlText(typeof(string))] public string Text { get; set; }

        [XmlIgnore] public ReferenceIndex Index => Ref != null ? new ReferenceIndex(Ref) : null;
        public NoteReferenceModel() { }
        public NoteReferenceModel(string text, bool literatureAndNotes = false) {
            Text = text;
            LiteratureAndNotes = literatureAndNotes;
        }
        public bool ShouldSerializeLiteratureAndNotes() => LiteratureAndNotes;

        public override string ToString() => Text;
    }
}
