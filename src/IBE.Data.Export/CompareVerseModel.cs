using IBE.Data.Model;
using System.Collections.Generic;

namespace IBE.Data.Export {
    public class CompareVerseModel {
        public string BookName { get; set; }
        public string BookShortcut { get; set; }
        public BiblePart Part { get; set; }
        public VerseIndex Index { get; set; }
        public List<CompareVerseInfo> Verses { get; set; }
        public bool LiteralOnly { get; set; }

        public string GetSiglum() {
            return $"{BookShortcut} {Index.NumberOfChapter}:{Index.NumberOfVerse}";
        }
    }

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
