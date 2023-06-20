using System.Xml.Serialization;

namespace ChurchServices.Data.Import.EIB.Model.Bible {
    [XmlRoot("bible", Namespace = "http://www.feib.pl/2023/06/bible")]
    public class BibleModel {
        [XmlAttribute("sc")] public string Shortcut { get; set; }
        [XmlAttribute("name")] public string Name { get; set; }
        [XmlAttribute("desc")] public string Description { get; set; } = null;
        [XmlAttribute("cn")] public string ChapterName { get; set; } = "Rozdział";
        [XmlAttribute("pn")] public string PsalmName { get; set; } = "Psalm";
        [XmlAttribute("xml:lang")] public string Language { get; set; } = "pl";
        [XmlAttribute("cr")] public bool ChapterRomanNumbering { get; set; } = false;
        [XmlAttribute("s")] public bool WithStrongs { get; set; } = false;
        [XmlAttribute("g")] public bool WithGrammarCodes { get; set; } = false;
        [XmlAttribute("t")] public TranslationType Type { get; set; } = TranslationType.None;

        [XmlElement("intro")] public FormattedText Introduction { get; set; } = null;
        [XmlElement("info")] public FormattedText DetailedInfo { get; set; } = null;

        [XmlElement("book")] public List<BookModel> Books { get; set; }

        public bool ShouldSerializeDetailedInfo() => DetailedInfo != null;
        public bool ShouldSerializeIntroduction() => Introduction != null;
        public bool ShouldSerializeDescription() => Description != null;
        public bool ShouldSerializeType() => Type != TranslationType.None;
        public bool ShouldSerializeWithGrammarCodes() => WithGrammarCodes;
        public bool ShouldSerializeWithStrongs() => WithStrongs;
        public bool ShouldSerializeChapterRomanNumbering() => ChapterRomanNumbering;
        public bool ShouldSerializeLanguage() => Language != "pl";
        public bool ShouldSerializePsalmName() => PsalmName != "Psalm";
        public bool ShouldSerializeChapterName() => ChapterName != "Rozdział";
    }

    public enum TranslationType {
        [XmlEnum("")]
        None = 0,
        [XmlEnum("i")]
        Interlinear = 1,
        [XmlEnum("n")]
        Default = 2,
        [XmlEnum("d")]
        Dynamic = 3,
        [XmlEnum("l")]
        Literal = 4
    }
}
