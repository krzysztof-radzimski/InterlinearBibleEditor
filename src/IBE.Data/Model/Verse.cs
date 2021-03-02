using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class Verse : XPObject {
        private int numberOfVerse;
        private string commentary;
        private Chapter parentChapter;

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

        [Association("VerseVersions")]
        public XPCollection<VerseVersion> VerseVersions {
            get { return GetCollection<VerseVersion>(nameof(VerseVersions)); }
        }

        public Verse(Session session) : base(session) { }

        public string GetShortcut() {
            string result = "";
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
