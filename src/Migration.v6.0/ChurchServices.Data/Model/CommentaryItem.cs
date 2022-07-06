/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Model {
    public class CommentaryItem : XPObject {
        private int bookNumber;
        private int chapterNumberFrom;
        private int chapterNumberTo;
        private int verseNumberFrom;
        private int verseNumberTo;
        private string text;
        private Commentary parentCommentary;

        public int BookNumber {
            get { return bookNumber; }
            set { SetPropertyValue(nameof(BookNumber), ref bookNumber, value); }
        }
        public int ChapterNumberFrom {
            get { return chapterNumberFrom; }
            set { SetPropertyValue(nameof(ChapterNumberFrom), ref chapterNumberFrom, value); }
        }
        public int ChapterNumberTo {
            get { return chapterNumberTo; }
            set { SetPropertyValue(nameof(ChapterNumberTo), ref chapterNumberTo, value); }
        }
        public int VerseNumberFrom {
            get { return verseNumberFrom; }
            set { SetPropertyValue(nameof(VerseNumberFrom), ref verseNumberFrom, value); }
        }
        public int VerseNumberTo {
            get { return verseNumberTo; }
            set { SetPropertyValue(nameof(VerseNumberTo), ref verseNumberTo, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Text {
            get { return text; }
            set { SetPropertyValue(nameof(Text), ref text, value); }
        }

        [Association("CommentaryItems")]
        public Commentary ParentCommentary {
            get { return parentCommentary; }
            set { SetPropertyValue(nameof(ParentCommentary), ref parentCommentary, value); }
        }

        public CommentaryItem(Session session) : base(session) { }


    }
}
