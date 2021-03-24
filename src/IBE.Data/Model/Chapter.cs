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
    public class Chapter : XPObject {
        private int numberOfChapter;
        private int numberOfVerses;
        private Book parentBook;
       
        public int NumberOfChapter {
            get { return numberOfChapter; }
            set { SetPropertyValue(nameof(NumberOfChapter), ref numberOfChapter, value); }
        }

        public int NumberOfVerses {
            get { return numberOfVerses; }
            set { SetPropertyValue(nameof(NumberOfVerses), ref numberOfVerses, value); }
        }

        [Association("ChapterSubtitles")]
        public XPCollection<Subtitle> Subtitles {
            get { return GetCollection<Subtitle>(nameof(Subtitles)); }
        }

        [Association("BookChapters")]
        public Book ParentBook {
            get { return parentBook; }
            set { SetPropertyValue(nameof(ParentBook), ref parentBook, value); }
        }

        [Association("ChapterVerses")]
        public XPCollection<Verse> Verses {
            get { return GetCollection<Verse>(nameof(Verses)); }
        }

        [NonPersistent]
        public Translation ParentTranslation {
            get {
                if (ParentBook != null) {
                    return ParentBook.ParentTranslation;
                }
                return default;
            }
        }
        
        public Chapter(Session session) : base(session) { }
    }
}
