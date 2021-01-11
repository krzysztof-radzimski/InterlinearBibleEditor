using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class BookStatus : XPObject {
        private string statusName;
        public string StatusName {
            get { return statusName; }
            set { SetPropertyValue(nameof(StatusName), ref statusName, value); }
        }

        [Association("StatusBooks")]
        public XPCollection<Book> Books {
            get { return GetCollection<Book>(nameof(Books)); }
        }

        public BookStatus(Session session) : base(session) { }
    }
}
