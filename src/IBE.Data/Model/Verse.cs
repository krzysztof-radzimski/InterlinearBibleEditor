/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class Verse : XPObject {
        private int numberOfVerse;
        private Chapter parentChapter;
        private string text;
        private string index;
        private bool startFromNewLine;

        public int NumberOfVerse {
            get { return numberOfVerse; }
            set { SetPropertyValue(nameof(NumberOfVerse), ref numberOfVerse, value); }
        }

        [Association("ChapterVerses")]
        public Chapter ParentChapter {
            get { return parentChapter; }
            set { SetPropertyValue(nameof(ParentChapter), ref parentChapter, value); }
        }

        [Association("VerseWords")]
        public XPCollection<VerseWord> VerseWords {
            get { return GetCollection<VerseWord>(nameof(VerseWords)); }
        }

        /// <summary>
        /// Treść wersetu jeżeli nie jest to przekład interlinearny. Treść zapisywana jest w pseudo-HTML
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public string Text {
            get { return text; }
            set { SetPropertyValue(nameof(Text), ref text, value); }
        }

        public bool StartFromNewLine {
            get { return startFromNewLine; }
            set { SetPropertyValue(nameof(StartFromNewLine), ref startFromNewLine, value); }
        }

        public string Index {
            get { return index; }
            set { SetPropertyValue(nameof(Index), ref index, value); }
        }


        [NonPersistent]
        public Translation ParentTranslation {
            get {
                if (ParentChapter != null) {
                    return ParentChapter.ParentTranslation;
                }
                return default;
            }
        }

        public Verse(Session session) : base(session) { }

        public string GetLink() {
            var result = string.Empty;
            if (ParentChapter != null && parentChapter.ParentBook != null) {
                result = $@"<a class=""verse-reference"" href=""B:{ParentChapter.ParentBook.NumberOfBook} {parentChapter.NumberOfChapter}:{NumberOfVerse}"">{GetShortcut()}</a>";
            }
            return result;
        }

        public string GetShortcut() {
            var result = string.Empty;
            if (ParentChapter != null && parentChapter.ParentBook != null) {
                result = $"{parentChapter.ParentBook.BookShortcut} {parentChapter.NumberOfChapter}:{NumberOfVerse}";
            }
            return result;
        }

        public string GetSourceText() {
            var text = string.Empty;
            foreach (var item in VerseWords) {
                text += item.SourceWord + " ";
            }
            return text.Trim();
        }

        public string GetTranslationText() {
            var text = string.Empty;
            foreach (var item in VerseWords) {
                text += item.Translation + " ";
            }
            return text.Trim();
        }

        public string GetTransliterationText() {
            var text = string.Empty;
            foreach (var item in VerseWords) {
                text += item.Transliteration + " ";
            }
            return text.Trim();
        }
    }
}
