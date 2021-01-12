using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class VerseWordReference : XPObject {
        private string bookShortcut;
        private int numberOfChapter;
        private int numberOfVerse;
        [Size(10)]
        public string BookShortcut {
            get { return bookShortcut; }
            set { SetPropertyValue(nameof(BookShortcut), ref bookShortcut, value); }
        }
        public int NumberOfChapter {
            get { return numberOfChapter; }
            set { SetPropertyValue(nameof(NumberOfChapter), ref numberOfChapter, value); }
        }
        public int NumberOfVerse {
            get { return numberOfVerse; }
            set { SetPropertyValue(nameof(NumberOfVerse), ref numberOfVerse, value); }
        }
        [Association("VerseWordReferences", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<VerseWord> VerseWords {
            get { return GetCollection<VerseWord>(nameof(VerseWords)); }
        }
        public VerseWordReference(Session session) : base(session) { }

        public string GetShortcut() {
            return $"{bookShortcut} {numberOfChapter}:{numberOfVerse}";
        }
    }
}
