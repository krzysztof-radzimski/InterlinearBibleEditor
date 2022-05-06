using IBE.Data.Model;

namespace IBE.Data.Export {
    public class CompareVerseInfo {
        public VerseIndex Index { get; set; }
        public string TranslationName { get; set; }
        public string TranslationDescription { get; set; }
        public TranslationType TranslationType { get; set; }
        public string Text { get; set; }
        public string HtmlText { get; set; }
        public string SimpleText { get; set; }
        public int SortIndex { get; set; }
    }
}
