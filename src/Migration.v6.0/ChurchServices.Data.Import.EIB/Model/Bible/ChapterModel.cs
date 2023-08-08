using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Bible {
    public class ChapterModel {
        [XmlAttribute("nr")] public int NumberOfChapter { get; set; }
        [XmlAttribute("vn")] public int NumberOfVerses { get; set; } = 0;

        [XmlElement("title", typeof(FormattedText))]
        [XmlElement("span", typeof(SpanModel))]
        [XmlElement("br", typeof(BreakLineModel))]
        [XmlElement("verse", typeof(VerseModel))]
        public List<object> Items { get; set; }

        public IEnumerable<VerseModel> Verses() => Items != null ? Items.Where(x => x is VerseModel).Cast<VerseModel>().OrderBy(x => x.NumberOfVerse) : null;
        public bool ShouldSerializeNumberOfVerses() => NumberOfVerses != 0;        
    }
}
