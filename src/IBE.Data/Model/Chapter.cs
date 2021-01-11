using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class Chapter : XPObject {
        private int numberOfChapter;
        private int numberOfVerses;
        private Book parentBook;

        public int NumberOfChapter {
            get { return numberOfChapter; }
            set { SetPropertyValue(nameof(NumberOfChapter), ref numberOfChapter, value); }
        }

        public int NumberOfVerses {
            get { return numberOfVerses; }
            set { SetPropertyValue(nameof(NumberOfVerses), ref numberOfVerses, value); }
        }

        [Association("ChapterSubtitles")]
        public XPCollection<Subtitle> Subtitles {
            get { return GetCollection<Subtitle>(nameof(Subtitles)); }
        }

        [Association("BookChapters")]
        public Book ParentBook {
            get { return parentBook; }
            set { SetPropertyValue(nameof(ParentBook), ref parentBook, value); }
        }


        public Chapter(Session session) : base(session) { }
    }
}
