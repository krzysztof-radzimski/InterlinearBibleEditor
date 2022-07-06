/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Model {
    public class AncientDictionaryItem : XPObject {
        private AncientDictionary dictionary;
        private string translation;
        private string transliteration;
        private StrongCode strongCode;
        private GrammarCode grammarCode;
        private string word;

        [Browsable(false)]
        [Association("DictionaryItems")]
        public AncientDictionary Dictionary {
            get { return dictionary; }
            set { SetPropertyValue(nameof(Dictionary), ref dictionary, value); }
        }

        public string Word {
            get { return word; }
            set { SetPropertyValue(nameof(Word), ref word, value); }
        }

        public string Transliteration {
            get { return transliteration; }
            set { SetPropertyValue(nameof(Transliteration), ref transliteration, value); }
        }

        public string Translation {
            get { return translation; }
            set { SetPropertyValue(nameof(Translation), ref translation, value); }
        }

        [Browsable(false)]
        [Association("StrongsDictionaryItems")]
        public StrongCode StrongCode {
            get { return strongCode; }
            set { SetPropertyValue(nameof(StrongCode), ref strongCode, value); }
        }

        [Browsable(false)]
        [Association("GrammarCodesDictionaryItems")]
        public GrammarCode GrammarCode {
            get { return grammarCode; }
            set { SetPropertyValue(nameof(GrammarCode), ref grammarCode, value); }
        }

        [Browsable(false)]
        [Association("DictionaryItemsVerses", UseAssociationNameAsIntermediateTableName = true)]
        public XPCollection<VerseInfo> VersesReferences {
            get { return GetCollection<VerseInfo>(nameof(VersesReferences)); }
        }
        public AncientDictionaryItem() : base() { }
        public AncientDictionaryItem(Session session) : base(session) { }
    }
}
