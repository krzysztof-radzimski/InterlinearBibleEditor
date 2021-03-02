using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class Commentary : XPObject {
        private string name;
        private Language language;
        private Translation parentTranslation;
        public string Name {
            get { return name; }
            set { SetPropertyValue(nameof(Name), ref name, value); }
        }
        public Language Language {
            get { return language; }
            set { SetPropertyValue(nameof(Language), ref language, value); }
        }

        [Association("CommentaryOfTranslation")]
        public Translation ParentTranslation {
            get { return parentTranslation; }
            set { SetPropertyValue(nameof(ParentTranslation), ref parentTranslation, value); }
        }

        [Association("CommentaryItems")]
        public XPCollection<CommentaryItem> Items {
            get { return GetCollection<CommentaryItem>(nameof(Items)); }
        }
        public Commentary(Session session) : base(session) { }
    }   
}
