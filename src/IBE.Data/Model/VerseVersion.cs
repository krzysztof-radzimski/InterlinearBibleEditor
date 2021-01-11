using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class VerseVersion : XPObject {
        private string text;
        private Translation translationName;
        private Verse parentVerse;

        public string Text {
            get { return text; }
            set { SetPropertyValue(nameof(Text), ref text, value); }
        }

        [Association("VerseTranslations")]
        public Translation TranslationName {
            get { return translationName; }
            set { SetPropertyValue(nameof(TranslationName), ref translationName, value); }
        }

        [Association("VerseVersions")]
        public Verse ParentVerse {
            get { return parentVerse; }
            set { SetPropertyValue(nameof(TranslationName), ref parentVerse, value); }
        }

        public VerseVersion(Session session) : base(session) { }
    }
}
