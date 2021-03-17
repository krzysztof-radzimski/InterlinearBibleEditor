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
    public class BookBase : XPObject {
        private int numberOfBook;
        private string bookShortcut;
        private string bookName;
        private string bookTitle;
        private string color;

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

        public BookBase() : base(new UnitOfWork()) { }
        public BookBase(Session session) : base(session) { }

        public override string ToString() {
            return BookName;
        }
    }
}
