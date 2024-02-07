using System.Xml.Serialization;

namespace EIB.Data.Utils {
    public class VerseWordModel {
        [XmlAttribute("nr")] public int NumberOfVerseWord { get; set; }
        [XmlAttribute("sc")] public string StrongCode { get; set; } = null;
        [XmlAttribute("gc")] public string GrammarCode { get; set; } = null;
        [XmlAttribute("s")] public string SourceWord { get; set; } = null;
        [XmlAttribute("l")] public string Transliteration { get; set; } = null;
        [XmlAttribute("cit")] public bool IsCitation { get; set; } = false;
        [XmlAttribute("gw")] public bool IsWordOfGod { get; set; } = false;

        [XmlText(typeof(string))] public string Translation { get; set; }

        public bool ShouldSerializeStrongCode() => StrongCode != null;
        public bool ShouldSerializeGrammarCode() => GrammarCode != null;
        public bool ShouldSerializeSourceWord() => SourceWord != null;
        public bool ShouldSerializeTransliteration() => Transliteration != null;
        public bool ShouldSerializeIsCitation() => IsCitation;
        public bool ShouldSerializeIsWordOfGod() => IsWordOfGod;
    }
}
