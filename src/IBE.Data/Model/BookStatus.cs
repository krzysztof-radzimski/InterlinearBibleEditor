using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class BookStatus : XPObject {
        private string statusName;
        private BiblePart biblePart;
        private CanonType canonType;
        public string StatusName {
            get { return statusName; }
            set { SetPropertyValue(nameof(StatusName), ref statusName, value); }
        }
        public BiblePart BiblePart {
            get { return biblePart; }
            set { SetPropertyValue(nameof(BiblePart), ref biblePart, value); }
        }
        public CanonType CanonType {
            get { return canonType; }
            set { SetPropertyValue(nameof(CanonType), ref canonType, value); }
        }

        [Association("StatusBooks")]
        public XPCollection<Book> Books {
            get { return GetCollection<Book>(nameof(Books)); }
        }

        public BookStatus(Session session) : base(session) { }
    }

    public enum BiblePart {
        None = 0,
        OldTestament = 1,
        NewTestament = 2
    }
    public enum CanonType {
        None = 0,
        Canon = 1,
        SecondCanon = 2,
        Apocrypha = 3
    }
}
