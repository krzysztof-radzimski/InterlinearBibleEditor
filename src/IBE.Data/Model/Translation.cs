/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using System.ComponentModel;

namespace IBE.Data.Model {
    public class Translation : XPObject {
        private string name;
        private string description;
        private string introduction;
        private string chapterString;
        private string chapterPsalmString;
        private Language language;
        private TranslationType type;
        private string detailedInfo;
        private bool catolic;
        private bool recommended;
        private bool openAccess;
        private TheBookType bookType;
        private bool chapterRomanNumbering;

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
        public Language Language {
            get { return language; }
            set { SetPropertyValue(nameof(Language), ref language, value); }
        }

        public TranslationType Type {
            get { return type; }
            set { SetPropertyValue(nameof(Type), ref type, value); }
        }

        public bool Catolic {
            get { return catolic; }
            set { SetPropertyValue(nameof(Catolic), ref catolic, value); }
        }

        public bool OpenAccess {
            get { return openAccess; }
            set { SetPropertyValue(nameof(OpenAccess), ref openAccess, value); }
        }

        public bool Recommended {
            get { return recommended; }
            set { SetPropertyValue(nameof(Recommended), ref recommended, value); }
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

        [Association("BookTranslations")]
        public XPCollection<Book> Books {
            get { return GetCollection<Book>(nameof(Books)); }
        }

        public TheBookType BookType {
            get { return bookType; }
            set { SetPropertyValue(nameof(BookType), ref bookType, value); }
        }

        public bool ChapterRomanNumbering {
            get { return chapterRomanNumbering; }
            set { SetPropertyValue(nameof(ChapterRomanNumbering), ref chapterRomanNumbering, value); }
        }

        public Translation() : base(new UnitOfWork()) { }
        public Translation(Session session) : base(session) { }
    }

    public enum TranslationType {
        [Description("")]
        None = 0,
        [Description("Przekład interlinearny")]
        Interlinear = 1,
        [Description("Przekład literacki")]
        Default = 2,
        [Description("Przekład dynamiczny")]
        Dynamic = 3,
        [Description("Przekład dosłowny")]
        Literal = 4
    }
}
