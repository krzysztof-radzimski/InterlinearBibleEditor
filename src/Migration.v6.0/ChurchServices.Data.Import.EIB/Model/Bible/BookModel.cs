using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Bible {
    public class BookModel {
        [XmlAttribute("nr")] public int NumberOfBook { get; set; }
        [XmlAttribute("nc")] public int NumberOfChapters { get; set; } = 0;
        [XmlAttribute("bs")] public string BookShortcut { get; set; }
        [XmlAttribute("n")] public string BookName { get; set; }
        [XmlAttribute("c")] public string Color { get; set; } = null;

        [XmlElement("author")] public FormattedText AuthorName { get; set; } = null;
        [XmlElement("time")] public FormattedText TimeOfWriting { get; set; } = null;
        [XmlElement("place")] public FormattedText PlaceWhereBookWasWritten { get; set; } = null;
        [XmlElement("purpose")] public FormattedText Purpose { get; set; } = null;
        [XmlElement("subject")] public FormattedText Subject { get; set; } = null;
        [XmlElement("preface")] public FormattedText Preface { get; set; } = null;

        [XmlElement("chapter")] public List<ChapterModel> Chapters { get; set; }

        public bool ShouldSerializeNumberOfChapters() => NumberOfChapters != 0;
        public bool ShouldSerializeColor() => Color != null;
        public bool ShouldSerializeAuthorName() => AuthorName != null;
        public bool ShouldSerializeTimeOfWriting() => TimeOfWriting != null;
        public bool ShouldSerializePlaceWhereBookWasWritten() => PlaceWhereBookWasWritten != null;
        public bool ShouldSerializePurpose() => Purpose != null;
        public bool ShouldSerializeSubject() => Subject != null;
        public bool ShouldSerializePreface() => Preface != null;
    }
}
