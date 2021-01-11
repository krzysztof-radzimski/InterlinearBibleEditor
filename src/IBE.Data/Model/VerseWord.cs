using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class VerseWord : XPObject {
        private int numberOfVerseWord;
        private Verse parentVerse;
        private string strongCode;
        private string sourceWord;
        private string transliteration;
        private string grammarCode;
        private string footnoteText;
        private string translation;
        private bool citation;
        private bool wordOfJesus;

        public int NumberOfVerseWord {
            get { return numberOfVerseWord; }
            set { SetPropertyValue(nameof(NumberOfVerseWord), ref numberOfVerseWord, value); }
        }
               
        public string StrongCode {
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
                
        public string GrammarCode {
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

        [Association("VerseWordReferences")]
        public XPCollection<Verse> References {
            get { return GetCollection<Verse>(nameof(References)); }
        }

        public VerseWord(Session session) : base(session) { }
    }
}
