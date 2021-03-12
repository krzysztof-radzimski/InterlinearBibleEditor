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
    public class Book : XPObject {
        private int numberOfBook;
        private string preface;
        private string bookShortcut;
        private string bookName;
        private string authorName;
        private string timeOfWriting;
        private string placeWhereBookWasWritten;
        private string purpose;
        private string subject;
        private int numberOfChapters;        
        private string color;
        private Translation parentTranslation;
        private BookBase bookBase;

        [Association("BaseBooks")]
        public BookBase BaseBook {
            get { return bookBase; }
            set { SetPropertyValue(nameof(BaseBook), ref bookBase, value); }
        }

        [Association("BookTranslations")]
        public Translation ParentTranslation {
            get { return parentTranslation; }
            set { SetPropertyValue(nameof(ParentTranslation), ref parentTranslation, value); }
        }

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
        public string AuthorName {
            get { return authorName; }
            set { SetPropertyValue(nameof(AuthorName), ref authorName, value); }
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
        [Size(200)]
        public string Subject {
            get { return subject; }
            set { SetPropertyValue(nameof(Subject), ref subject, value); }
        }

        /// <summary>
        /// Przedmowa
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public string Preface {
            get { return preface; }
            set { SetPropertyValue(nameof(Preface), ref preface, value); }
        }

        [Association("BookChapters")]
        public XPCollection<Chapter> Chapters {
            get { return GetCollection<Chapter>(nameof(Chapters)); }
        }        

        public string Color {
            get { return color; }
            set { SetPropertyValue(nameof(Color), ref color, value); }
        }

        public Book(Session session) : base(session) { }

        public override string ToString() {
            return BookName;
        }
    }

    public class BookInfo {
        public int Id { get; private set; }
        public string Caption { get; private set; }
        public int NumberOfChapters { get; private set; }

        private BookInfo() { }
        public BookInfo(Book book) : this() {
            this.Id = book.Oid;
            this.Caption = book.ToString();
            this.NumberOfChapters = book.NumberOfChapters;
        }
        public override string ToString() {
            return Caption;
        }
    }
}
