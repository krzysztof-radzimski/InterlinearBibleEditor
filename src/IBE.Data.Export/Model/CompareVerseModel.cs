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
}
