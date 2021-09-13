/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using IBE.Common.Extensions;
using System.Text.RegularExpressions;

namespace IBE.Data.Model {
    public class VerseIndex {
        public string TranslationName { get; }
        public int NumberOfBook { get; }
        public int NumberOfChapter { get; }
        public int NumberOfVerse { get; }
        public string Index { get; }
        private VerseIndex() { }
        public VerseIndex(string index) {
            try {
                Index = index;
                var regex = new Regex(@"(?<translation>[A-Z0-9]+)\.(?<book>[0-9]+)\.(?<chapter>[0-9]+)\.(?<verse>[0-9]+)");
                var m = regex.Match(index);
                TranslationName = m.Groups["translation"].Value;
                NumberOfBook = m.Groups["book"].Value.ToInt();
                NumberOfChapter = m.Groups["chapter"].Value.ToInt();
                NumberOfVerse = m.Groups["verse"].Value.ToInt();
            }
            catch { }
        }
        public bool IsEmpty { get => TranslationName.IsNullOrEmpty() && NumberOfBook == 0 && NumberOfChapter == 0 && NumberOfVerse == 0; }
        public int NTBookNumber {
            get {
                if (TranslationName == "IPD") { return 67; }
                var r = 1;
                for (int i = 470; i <= 730; i += 10) {
                    if (i == NumberOfBook) {
                        return r;
                    }
                    r++;
                }
                return r;
            }
        }
        public int OTBookNumber {
            get {
                if (NumberOfBook > 460) { return 0; }
                var r = 1;
                for (int i = 10; i <= 470; i += 10) {
                    if (i == NumberOfBook) {
                        return r;
                    }
                    r++;
                }
                return r;
            }
        }
    }
}
