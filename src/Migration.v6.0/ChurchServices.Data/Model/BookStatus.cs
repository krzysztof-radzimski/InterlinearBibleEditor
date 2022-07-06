/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Model {
    public class BookStatus : XPObject {
        private BiblePart biblePart;
        private CanonType canonType;
        private TheBookType bookType;
        public BiblePart BiblePart {
            get { return biblePart; }
            set { SetPropertyValue(nameof(BiblePart), ref biblePart, value); }
        }
        public CanonType CanonType {
            get { return canonType; }
            set { SetPropertyValue(nameof(CanonType), ref canonType, value); }
        }
        public TheBookType BookType {
            get { return bookType; }
            set { SetPropertyValue(nameof(BookType), ref bookType, value); }
        }

        [Association("StatusBooks")]
        public XPCollection<BookBase> Books {
            get { return GetCollection<BookBase>(nameof(Books)); }
        }

        public BookStatus(Session session) : base(session) { }

        public override string ToString() {
            return $"{BookType} {BiblePart} {CanonType}";
        }
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
    public enum TheBookType {
        None = 0,
        Bible = 1,
        ChurchFathersLetter = 2,
        Catechism = 3,
        Other = 4
    }
}
