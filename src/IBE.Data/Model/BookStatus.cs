/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class BookStatus : XPObject {
        private BiblePart biblePart;
        private CanonType canonType;
        public BiblePart BiblePart {
            get { return biblePart; }
            set { SetPropertyValue(nameof(BiblePart), ref biblePart, value); }
        }
        public CanonType CanonType {
            get { return canonType; }
            set { SetPropertyValue(nameof(CanonType), ref canonType, value); }
        }

        [Association("StatusBooks")]
        public XPCollection<BookBase> Books {
            get { return GetCollection<BookBase>(nameof(Books)); }
        }

        public BookStatus(Session session) : base(session) { }
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
}
