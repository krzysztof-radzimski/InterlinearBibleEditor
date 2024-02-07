using DevExpress.Xpo;
using EIB.Data.Enumeration;

namespace EIB.Data.Model {
    public class BookBase : XPObject {
        private int numberOfBook;
        private string bookShortcut;
        private string bookName;
        private string bookTitle;
        private string color;
        private string preface;
        private string timeOfWriting;
        private string placeWhereBookWasWritten;
        private string purpose;
        private string subject;
        private string authorName;
        private CanonType status;

        public int NumberOfBook {
            get { return numberOfBook; }
            set { SetPropertyValue(nameof(NumberOfBook), ref numberOfBook, value); }
        }

        [Size(100)]
        public string BookName {
            get { return bookName; }
            set { SetPropertyValue(nameof(BookName), ref bookName, value); }
        }

        [Size(100)]
        public string BookTitle {
            get { return bookTitle; }
            set { SetPropertyValue(nameof(BookTitle), ref bookTitle, value); }
        }

        public string Color {
            get { return color; }
            set { SetPropertyValue(nameof(Color), ref color, value); }
        }

        public CanonType Status {
            get { return status; }
            set { SetPropertyValue(nameof(Status), ref status, value); }
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

        [Size(500)]
        public string Subject {
            get { return subject; }
            set { SetPropertyValue(nameof(Subject), ref subject, value); }
        }

        [Size(500)]
        public string AuthorName {
            get { return authorName; }
            set { SetPropertyValue(nameof(AuthorName), ref authorName, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Preface {
            get { return preface; }
            set { SetPropertyValue(nameof(Preface), ref preface, value); }
        }

        [Association] public XPCollection<Book> TranslationBooks { get { return GetCollection<Book>(nameof(TranslationBooks)); } }
        [Association] public XPCollection<BookShortcut> BookShortcuts { get { return GetCollection<BookShortcut>(nameof(BookShortcuts)); } }

        public BookBase() : base(new UnitOfWork()) { }
        public BookBase(Session session) : base(session) { }

        public override string ToString() => BookName;
    }
}
