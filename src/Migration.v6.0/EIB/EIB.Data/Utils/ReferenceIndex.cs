using ChurchServices.Extensions;
using System.Text.RegularExpressions;

namespace EIB.Data.Utils {
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
