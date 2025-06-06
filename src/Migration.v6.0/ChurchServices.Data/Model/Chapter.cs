﻿/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/


namespace ChurchServices.Data.Model {
    public class Chapter : XPObject {
        private int numberOfChapter;
        private int numberOfVerses;
        private Book parentBook;
        private bool isTranslated;
        private string index;

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

        public bool IsTranslated {
            get { return isTranslated; }
            set { SetPropertyValue(nameof(IsTranslated), ref isTranslated, value); }
        }

        public string Index {
            get { return index; }
            set { SetPropertyValue(nameof(Index), ref index, value); }
        }

        public Chapter(Session session) : base(session) { }
    }
}
