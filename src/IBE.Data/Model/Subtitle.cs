using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class Subtitle : XPObject {
        private Chapter parentChapter;
        private int beforeVerseNumber;
        private string text;
        private int level; // 1 - title (center, bold, 14) , 2 - subtitle (left, bold, 12)

        [Association("ChapterSubtitles")]
        public Chapter ParentChapter {
            get { return parentChapter; }
            set { SetPropertyValue(nameof(ParentChapter), ref parentChapter, value); }
        }
             
        public int BeforeVerseNumber {
            get { return beforeVerseNumber; }
            set { SetPropertyValue(nameof(BeforeVerseNumber), ref beforeVerseNumber, value); }
        }

        public int Level {
            get { return level; }
            set { SetPropertyValue(nameof(Level), ref level, value); }
        }

        [Size(200)]
        public string Text {
            get { return text; }
            set { SetPropertyValue(nameof(Text), ref text, value); }
        }

        public Subtitle(Session session) : base(session) { }
    }
}
