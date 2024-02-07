using DevExpress.Xpo;

namespace EIB.Data.Model {
    public class Book : XPObject {
        private Translation parentTranslation;
        private BookBase bookBase;

        [Association]
        public BookBase BaseBook {
            get { return bookBase; }
            set { SetPropertyValue(nameof(BaseBook), ref bookBase, value); }
        }

        [Association]
        public Translation ParentTranslation {
            get { return parentTranslation; }
            set { SetPropertyValue(nameof(ParentTranslation), ref parentTranslation, value); }
        }

        [NonPersistent] public int NumberOfBook => BaseBook != null ? BaseBook.NumberOfBook : 0;
        [NonPersistent] public int DefaultNumberOfChapters => BaseBook != null ? BaseBook.NumberOfBook : 0;
        [NonPersistent] public BookShortcut BookShortcut => BaseBook != null && BaseBook.BookShortcuts.Count > 0 ? BaseBook.BookShortcuts.Where(x => x.IsDefault).FirstOrDefault() : null;
        [NonPersistent] public string BookName => BaseBook != null ? BaseBook.BookName : null;
        [NonPersistent] public string AuthorName => BaseBook != null ? BaseBook.AuthorName : null;
        [NonPersistent] public string TimeOfWriting => BaseBook != null ? BaseBook.TimeOfWriting : null;
        [NonPersistent] public string PlaceWhereBookWasWritten => BaseBook != null ? BaseBook.PlaceWhereBookWasWritten : null;
        [NonPersistent] public string Purpose => BaseBook != null ? BaseBook.Purpose : null;
        [NonPersistent] public string Subject => BaseBook != null ? BaseBook.Subject : null;
        [NonPersistent] public string Preface => BaseBook != null ? BaseBook.Preface : null;
        [NonPersistent] public string Color => BaseBook != null ? BaseBook.Color : null;
        [NonPersistent] public bool IsNT => NumberOfBook >= 470 && NumberOfBook <= 730;
        [NonPersistent] public bool IsOT => NumberOfBook < 470 && NumberOfBook >= 10;
        [NonPersistent] public int NumberOfChapters => Chapters != null ? Chapters.Count : 0;
        [Association] public XPCollection<Chapter> Chapters { get { return GetCollection<Chapter>(nameof(Chapters)); } }

        public Book(Session session) : base(session) { }

        public override string ToString() => BookName;
    }
}
