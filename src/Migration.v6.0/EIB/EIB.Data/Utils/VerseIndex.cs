using ChurchServices.Extensions;
using System.Text.RegularExpressions;

namespace EIB.Data.Utils {
    public class VerseIndex {
        public string TranslationName { get; }
        public int NumberOfBook { get; }
        public int NumberOfChapter { get; }
        public int NumberOfVerse { get; }
        public string Index { get; }
        private VerseIndex() { }
        public VerseIndex(string index) {
            if (index != null) {
                Index = index;
                var regex = new Regex(@"(?<translation>[A-Z0-9]+)\.(?<book>[0-9]+)\.(?<chapter>[0-9]+)\.(?<verse>[0-9]+)");
                var m = regex.Match(index);
                if (m != null && m.Success) {
                    TranslationName = m.Groups["translation"] != null && m.Groups["translation"].Success ? m.Groups["translation"].Value : null;
                    NumberOfBook = m.Groups["book"] != null && m.Groups["book"].Success ? m.Groups["book"].Value.ToInt() : 0;
                    NumberOfChapter = m.Groups["chapter"] != null && m.Groups["chapter"].Success ? m.Groups["chapter"].Value.ToInt() : 0;
                    NumberOfVerse = m.Groups["verse"] != null && m.Groups["verse"].Success ? m.Groups["verse"].Value.ToInt() : 0;
                }
            }
        }
        public bool IsEmpty => TranslationName.IsNullOrEmpty() && NumberOfBook == 0 && NumberOfChapter == 0 && NumberOfVerse == 0;
    }
}
