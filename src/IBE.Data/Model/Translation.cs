using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class Translation : XPObject {
        private string name;
        public string Name {
            get { return name; }
            set { SetPropertyValue(nameof(Name), ref name, value); }
        }

        [Association("VerseTranslations")]
        public XPCollection<VerseVersion> VerseVersions {
            get { return GetCollection<VerseVersion>(nameof(VerseVersions)); }
        }

        public Translation(Session session) : base(session) { }
    }
}
