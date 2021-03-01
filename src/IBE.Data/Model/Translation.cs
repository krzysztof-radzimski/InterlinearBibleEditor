using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class Translation : XPObject {
        private string name;
        private string description;
        private string introduction;
        private string chapterString;
        private string chapterPsalmString;
        private string language;
        private string detailedInfo;

        public string Name {
            get { return name; }
            set { SetPropertyValue(nameof(Name), ref name, value); }
        }
        public string Description {
            get { return description; }
            set { SetPropertyValue(nameof(Description), ref description, value); }
        }
        public string ChapterString {
            get { return chapterString; }
            set { SetPropertyValue(nameof(ChapterString), ref chapterString, value); }
        }
        public string ChapterPsalmString {
            get { return chapterPsalmString; }
            set { SetPropertyValue(nameof(ChapterPsalmString), ref chapterPsalmString, value); }
        }
        public string Language {
            get { return language; }
            set { SetPropertyValue(nameof(Language), ref language, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Introduction {
            get { return introduction; }
            set { SetPropertyValue(nameof(Introduction), ref introduction, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string DetailedInfo {
            get { return detailedInfo; }
            set { SetPropertyValue(nameof(DetailedInfo), ref detailedInfo, value); }
        }

        [Association("VerseTranslations")]
        public XPCollection<VerseVersion> VerseVersions {
            get { return GetCollection<VerseVersion>(nameof(VerseVersions)); }
        }

        public Translation(Session session) : base(session) { }
    }
}
