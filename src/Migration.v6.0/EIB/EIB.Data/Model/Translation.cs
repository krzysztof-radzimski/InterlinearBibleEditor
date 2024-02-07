using DevExpress.Xpo;
using EIB.Data.Enumeration;

namespace EIB.Data.Model {
    public class Translation : XPObject {
        private string name;
        private string shortcut;
        private string description;
        private string introduction;
        private string summary;
        private string chapterString;
        private string chapterPsalmString;
        private Language language;
        private TranslationType type;
        private TheBookType bookType;
        private bool chapterRomanNumbering;

        [Size(150)]
        public string Name {
            get { return name; }
            set { SetPropertyValue(nameof(Name), ref name, value); }
        }
        [Size(10)]
        public string Shortcut {
            get { return shortcut; }
            set { SetPropertyValue(nameof(Shortcut), ref shortcut, value); }
        }
        [Size(1000)]
        public string Description {
            get { return description; }
            set { SetPropertyValue(nameof(Description), ref description, value); }
        }
        [Size(50)]
        public string ChapterString {
            get { return chapterString; }
            set { SetPropertyValue(nameof(ChapterString), ref chapterString, value); }
        }
        [Size(50)]
        public string ChapterPsalmString {
            get { return chapterPsalmString; }
            set { SetPropertyValue(nameof(ChapterPsalmString), ref chapterPsalmString, value); }
        }
        public Language Language {
            get { return language; }
            set { SetPropertyValue(nameof(Language), ref language, value); }
        }

        public TranslationType Type {
            get { return type; }
            set { SetPropertyValue(nameof(Type), ref type, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Introduction {
            get { return introduction; }
            set { SetPropertyValue(nameof(Introduction), ref introduction, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Summary {
            get { return summary; }
            set { SetPropertyValue(nameof(Summary), ref summary, value); }
        }

        public TheBookType BookType {
            get { return bookType; }
            set { SetPropertyValue(nameof(BookType), ref bookType, value); }
        }

        public bool ChapterRomanNumbering {
            get { return chapterRomanNumbering; }
            set { SetPropertyValue(nameof(ChapterRomanNumbering), ref chapterRomanNumbering, value); }
        }

        [Association] public XPCollection<Book> Books { get { return GetCollection<Book>(nameof(Books)); } }

        public Translation() : base(new UnitOfWork()) { }
        public Translation(Session session) : base(session) { }

        public override string ToString() => Name;
    }
}
