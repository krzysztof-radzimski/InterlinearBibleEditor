/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Model {
    public class BookBase : XPObject {
        private int numberOfBook;
        private string bookShortcut;
        private string bookName;
        private string bookTitle;
        private string color;
        private string preface;
        private byte[] prefaceData;
        private string timeOfWriting;
        private string placeWhereBookWasWritten;
        private string purpose;
        private string subject;
        private string authorName;
        private BookStatus status;

        public int NumberOfBook {
            get { return numberOfBook; }
            set { SetPropertyValue(nameof(NumberOfBook), ref numberOfBook, value); }
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
        public string BookTitle {
            get { return bookTitle; }
            set { SetPropertyValue(nameof(BookTitle), ref bookTitle, value); }
        }


        public string Color {
            get { return color; }
            set { SetPropertyValue(nameof(Color), ref color, value); }
        }

        [Association("StatusBooks")]
        public BookStatus Status {
            get { return status; }
            set { SetPropertyValue(nameof(Status), ref status, value); }
        }

        [Association("BaseBooks")]
        public XPCollection<Book> TranslationBooks {
            get { return GetCollection<Book>(nameof(TranslationBooks)); }
        }

        /// <summary>
        /// Czas spisania
        /// </summary>
        [Size(100)]
        public string TimeOfWriting {
            get { return timeOfWriting; }
            set { SetPropertyValue(nameof(TimeOfWriting), ref timeOfWriting, value); }
        }

        /// <summary>
        /// Miejsce spisania
        /// </summary>
        [Size(100)]
        public string PlaceWhereBookWasWritten {
            get { return placeWhereBookWasWritten; }
            set { SetPropertyValue(nameof(PlaceWhereBookWasWritten), ref placeWhereBookWasWritten, value); }
        }

        /// <summary>
        /// Cel napisania księgi
        /// </summary>
        [Size(500)]
        public string Purpose {
            get { return purpose; }
            set { SetPropertyValue(nameof(Purpose), ref purpose, value); }
        }

        /// <summary>
        /// Temat księgi
        /// </summary>
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

        /// <summary>
        /// Przedmowa
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public string Preface {
            get { return preface; }
            set { SetPropertyValue(nameof(Preface), ref preface, value); }
        }
        
        [NonPersistent] public BiblePart StatusBiblePart { get { return Status.IsNotNull() ? Status.BiblePart : BiblePart.None; } }
        [NonPersistent] public CanonType StatusCanonType { get { return Status.IsNotNull() ? Status.CanonType : CanonType.None; } }
        [NonPersistent] public TheBookType StatusBookType { get { return Status.IsNotNull() ? Status.BookType : TheBookType.None; } }

        public byte[] PrefaceData {
            get { return prefaceData; }
            set { SetPropertyValue(nameof(PrefaceData), ref prefaceData, value); }
        }

        public BookBase() : base(new UnitOfWork()) { }
        public BookBase(Session session) : base(session) { }

        public override string ToString() {
            return BookName;
        }
    }
}
