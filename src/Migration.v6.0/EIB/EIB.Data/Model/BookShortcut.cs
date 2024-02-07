using DevExpress.Xpo;

namespace EIB.Data.Model {
    public class BookShortcut : XPObject {
        private string shortcut;
        private bool isDefault;
        private BookBase parentBook;

        [Size(10)]
        public string Shortcut {
            get { return shortcut; }
            set { SetPropertyValue(nameof(Shortcut), ref shortcut, value); }
        }

        public bool IsDefault {
            get { return isDefault; }
            set { SetPropertyValue(nameof(IsDefault), ref isDefault, value); }
        }

        [Association]
        public BookBase ParentBook {
            get { return parentBook; }
            set { SetPropertyValue(nameof(ParentBook), ref parentBook, value); }
        }

        public BookShortcut() : base(new UnitOfWork()) { }
        public BookShortcut(Session session) : base(session) { }

    }
}
