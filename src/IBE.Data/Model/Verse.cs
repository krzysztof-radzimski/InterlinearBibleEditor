using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class Verse : XPObject {
        private int numberOfVerse;
        private string commentary;

        public int NumberOfVerse {
            get { return numberOfVerse; }
            set { SetPropertyValue(nameof(NumberOfVerse), ref numberOfVerse, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Commentary {
            get { return commentary; }
            set { SetPropertyValue(nameof(Commentary), ref commentary, value); }
        }

        [Association("VerseWords")]
        public XPCollection<VerseWord> VerseWords {
            get { return GetCollection<VerseWord>(nameof(VerseWords)); }
        }

        [Association("VerseVersions")]
        public XPCollection<VerseVersion> VerseVersions {
            get { return GetCollection<VerseVersion>(nameof(VerseVersions)); }
        }

        [Association("VerseWordReferences")]
        public XPCollection<VerseWord> References {
            get { return GetCollection<VerseWord>(nameof(References)); }
        }

        public Verse(Session session) : base(session) { }
    }
}
