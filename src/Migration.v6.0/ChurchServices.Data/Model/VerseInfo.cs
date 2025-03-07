/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Model {
    public class VerseInfo : XPObject {
        private BookBase bookBase;
        private int numberOfBook;
        private int numberOfChapter;
        private int numberOfVerse;       
        public int NumberOfBook {
            get { return numberOfBook; }
            set { SetPropertyValue(nameof(NumberOfBook), ref numberOfBook, value); }
        }
        public int NumberOfChapter {
            get { return numberOfChapter; }
            set { SetPropertyValue(nameof(NumberOfChapter), ref numberOfChapter, value); }
        }
        public int NumberOfVerse {
            get { return numberOfVerse; }
            set { SetPropertyValue(nameof(NumberOfVerse), ref numberOfVerse, value); }
        }
        
        public BookBase BaseBook {
            get { return bookBase; }
            set { SetPropertyValue(nameof(BaseBook), ref bookBase, value); }
        }

        [Association("DictionaryItemsVerses", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<AncientDictionaryItem> DictionaryItems {
            get { return GetCollection<AncientDictionaryItem>(nameof(DictionaryItems)); }
        }

        public VerseInfo(Session session) : base(session) { }
    }
}
