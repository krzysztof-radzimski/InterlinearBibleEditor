using DevExpress.Xpo;

namespace EIB.Data.Model {
    public class Chapter : XPObject {
        private int numberOfChapter;
        private Book parentBook;
        private string index;
        private string text;

        public int NumberOfChapter {
            get { return numberOfChapter; }
            set { SetPropertyValue(nameof(NumberOfChapter), ref numberOfChapter, value); }
        }
        [NonPersistent] public int NumberOfVerses => Verses != null ? Verses.Count : 0;

        [Association]
        public Book ParentBook {
            get { return parentBook; }
            set { SetPropertyValue(nameof(ParentBook), ref parentBook, value); }
        }

        [NonPersistent] public Translation ParentTranslation => ParentBook != null ? ParentBook.ParentTranslation : null;

        [Size(20)]
        public string Index {
            get { return index; }
            set { SetPropertyValue(nameof(Index), ref index, value); }
        }

        // RTF text
        [Size(SizeAttribute.Unlimited)]
        public string Text {
            get { return text; }
            set { SetPropertyValue(nameof(Text), ref text, value); }
        }

        [Association] public XPCollection<Subtitle> Subtitles { get { return GetCollection<Subtitle>(nameof(Subtitles)); } }
        [Association] public XPCollection<Verse> Verses { get { return GetCollection<Verse>(nameof(Verses)); } }

        public Chapter(Session session) : base(session) { }
    }
}
