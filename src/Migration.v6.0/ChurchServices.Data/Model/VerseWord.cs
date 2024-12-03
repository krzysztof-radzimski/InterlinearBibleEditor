/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Model {
    public class VerseWord : XPObject {
        private int numberOfVerseWord;
        private Verse parentVerse;
        private StrongCode strongCode;
        private string sourceWord;
        private string transliteration;
        private GrammarCode grammarCode;
        private string footnoteText;
        private string translation;
        private bool citation;
        private bool wordOfJesus;

        public int NumberOfVerseWord {
            get { return numberOfVerseWord; }
            set { SetPropertyValue(nameof(NumberOfVerseWord), ref numberOfVerseWord, value); }
        }

        [Association("StrongsVerseWords")]
        public StrongCode StrongCode {
            get { return strongCode; }
            set { SetPropertyValue(nameof(StrongCode), ref strongCode, value); }
        }

        public string SourceWord {
            get { return sourceWord; }
            set { SetPropertyValue(nameof(SourceWord), ref sourceWord, value); }
        }

        public string Transliteration {
            get { return transliteration; }
            set { SetPropertyValue(nameof(Transliteration), ref transliteration, value); }
        }

        [Association("VerseWordGrammarCodes")]
        public GrammarCode GrammarCode {
            get { return grammarCode; }
            set { SetPropertyValue(nameof(GrammarCode), ref grammarCode, value); }
        }

        public string Translation {
            get { return translation; }
            set { SetPropertyValue(nameof(Translation), ref translation, value); }
        }

        public bool Citation {
            get { return citation; }
            set { SetPropertyValue(nameof(Citation), ref citation, value); }
        }

        public bool WordOfJesus {
            get { return wordOfJesus; }
            set { SetPropertyValue(nameof(WordOfJesus), ref wordOfJesus, value); }
        }

        [Association("VerseWords")]
        public Verse ParentVerse {
            get { return parentVerse; }
            set { SetPropertyValue(nameof(ParentVerse), ref parentVerse, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string FootnoteText {
            get { return footnoteText; }
            set { SetPropertyValue(nameof(FootnoteText), ref footnoteText, value); }
        }

        public VerseWord(Session session) : base(session) { }

        [NonPersistent] public int StrongCodeValue { get { return StrongCode != null ? StrongCode.Code : 0; } }
    }
}
