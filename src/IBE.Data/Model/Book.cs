using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class Book : XPObject {
        private int numberOfBook;
        private string preface;
        private string bookShortcut;
        private string bookName;
        private string greekName;
        private string latinName;
        private string hebrewName;
        private string authorName;
        private string timeOfWriting;
        private string placeWhereBookWasWritten;
        private string purpose;
        private string subject;
        private int numberOfChapters;
        private BookStatus status;

        public int NumberOfBook {
            get { return numberOfBook; }
            set { SetPropertyValue(nameof(NumberOfBook), ref numberOfBook, value); }
        }

        public int NumberOfChapters {
            get { return numberOfChapters; }
            set { SetPropertyValue(nameof(NumberOfChapters), ref numberOfChapters, value); }
        }

        [Size(10)]
        public string BookShortcut {
            get { return bookShortcut; }
            set { SetPropertyValue(nameof(BookShortcut), ref bookShortcut, value); }
        }

        [Size(100)]
        public string BookName {
            get { return bookName; }
            set { SetPropertyValue(nameof(BookName), ref bookName, value); }
        }

        [Size(100)]
        public string GreekName {
            get { return greekName; }
            set { SetPropertyValue(nameof(GreekName), ref greekName, value); }
        }

        [Size(100)]
        public string LatinName {
            get { return latinName; }
            set { SetPropertyValue(nameof(LatinName), ref latinName, value); }
        }

        [Size(100)]
        public string HebrewName {
            get { return hebrewName; }
            set { SetPropertyValue(nameof(HebrewName), ref hebrewName, value); }
        }

        [Size(100)]
        public string AuthorName {
            get { return authorName; }
            set { SetPropertyValue(nameof(AuthorName), ref authorName, value); }
        }

        [Size(100)]
        public string TimeOfWriting {
            get { return timeOfWriting; }
            set { SetPropertyValue(nameof(TimeOfWriting), ref timeOfWriting, value); }
        }

        [Size(100)]
        public string PlaceWhereBookWasWritten {
            get { return placeWhereBookWasWritten; }
            set { SetPropertyValue(nameof(PlaceWhereBookWasWritten), ref placeWhereBookWasWritten, value); }
        }

        [Size(500)]
        public string Purpose {
            get { return purpose; }
            set { SetPropertyValue(nameof(Purpose), ref purpose, value); }
        }

        [Size(200)]
        public string Subject {
            get { return subject; }
            set { SetPropertyValue(nameof(Subject), ref subject, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Preface {
            get { return preface; }
            set { SetPropertyValue(nameof(Preface), ref preface, value); }
        }

        [Association("BookChapters")]
        public XPCollection<Chapter> Chapters {
            get { return GetCollection<Chapter>(nameof(Chapters)); }
        }

        [Association("StatusBooks")]
        public BookStatus Status {
            get { return status; }
            set { SetPropertyValue(nameof(Status), ref status, value); }
        }

        public Book(Session session) : base(session) { }
    }
}
