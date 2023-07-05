using ChurchServices.Extensions;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Bible {
    public class NoteModel {
        [XmlAttribute("n")] public string Number { get; set; }
        [XmlAttribute("t")] public NoteType Type { get; set; } = NoteType.Default;

        [XmlElement("br", typeof(BreakLine))]
        [XmlElement("a", typeof(Hyperlink))]
        [XmlElement("ref", typeof(NoteReferenceModel))]
        [XmlElement("span", typeof(Span))]
        [XmlText(typeof(string))]
        public List<object> Items { get; set; }

        public bool ShouldSerializeType() => Type != NoteType.Default;

        public override string ToString() {
            if (Number != null) { return Number; }
            return base.ToString();
        }
    }

    public enum NoteType {
        [XmlEnum("")] Default = 0,
        [XmlEnum("cr")] CrossReference = 1
    };

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
    }

    public class ReferenceIndex {
        public string BookShortcut { get; set; }
        public int ChapterNumber { get; set; }
        public int SecondChapterNumber { get; set; }
        public int VerseStartNumber { get; set; }
        public int VerseEndNumber { get; set; }
        public ReferenceIndex() { }
        public ReferenceIndex(string index) : this() {
            if (index != null && index.Contains(".")) {
                var pattern = @"(?<book>[0-9a-zA-Z]+)\.(?<chapter>[0-9]+)\.(?<verse>[0-9]+)(\-(?<book2>[a-zA-Z]+)\.(?<chapter2>[0-9]+)\.(?<verse2>[0-9]+))?";
                var match = Regex.Match(index, pattern);
                if (match.Success) {
                    if (match.Groups["book"] != null && match.Groups["book"].Success) { BookShortcut = match.Groups["book"].Value; }
                    if (match.Groups["chapter"] != null && match.Groups["chapter"].Success) { ChapterNumber = match.Groups["chapter"].Value.ToInt(); }
                    if (match.Groups["chapter2"] != null && match.Groups["chapter2"].Success) { SecondChapterNumber = match.Groups["chapter2"].Value.ToInt(); }
                    if (match.Groups["verse"] != null && match.Groups["verse"].Success) { VerseStartNumber = match.Groups["verse"].Value.ToInt(); }
                    if (match.Groups["verse2"] != null && match.Groups["verse2"].Success) { VerseEndNumber = match.Groups["verse2"].Value.ToInt(); }
                }
            }
        }
    }
}
