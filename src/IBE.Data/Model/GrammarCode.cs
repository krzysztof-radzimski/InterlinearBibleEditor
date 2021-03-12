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
    public class GrammarCode : XPObject {
        private string grammarCodeVariant1;
        private string grammarCodeVariant2;
        private string grammarCodeVariant3;
        private string grammarCodeDescription;
        public string GrammarCodeVariant1 {
            get { return grammarCodeVariant1; }
            set { SetPropertyValue(nameof(GrammarCodeVariant1), ref grammarCodeVariant1, value); }
        }
        public string GrammarCodeVariant2 {
            get { return grammarCodeVariant2; }
            set { SetPropertyValue(nameof(GrammarCodeVariant2), ref grammarCodeVariant2, value); }
        }
        public string GrammarCodeVariant3 {
            get { return grammarCodeVariant3; }
            set { SetPropertyValue(nameof(GrammarCodeVariant3), ref grammarCodeVariant3, value); }
        }
        public string GrammarCodeDescription {
            get { return grammarCodeDescription; }
            set { SetPropertyValue(nameof(GrammarCodeDescription), ref grammarCodeDescription, value); }
        }

        [Association("VerseWordGrammarCodes")]
        public XPCollection<VerseWord> VerseWords {
            get { return GetCollection<VerseWord>(nameof(VerseWords)); }
        }

        [Association("GrammarCodesDictionaryItems")]
        public XPCollection<AncientDictionaryItem> DictionaryItems {
            get { return GetCollection<AncientDictionaryItem>(nameof(DictionaryItems)); }
        }

        public GrammarCode(Session session) : base(session) { }
    }
}
