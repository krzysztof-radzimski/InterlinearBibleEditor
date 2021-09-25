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
    public class AncientDictionaryItem : XPObject {
        private AncientDictionary dictionary;
        private string translation;
        private string transliteration;
        private StrongCode strongCode;
        private GrammarCode grammarCode;
        private string word;

        [System.ComponentModel.Browsable(false)]
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

        [System.ComponentModel.Browsable(false)]
        [Association("StrongsDictionaryItems")]
        public StrongCode StrongCode {
            get { return strongCode; }
            set { SetPropertyValue(nameof(StrongCode), ref strongCode, value); }
        }

        [System.ComponentModel.Browsable(false)]
        [Association("GrammarCodesDictionaryItems")]
        public GrammarCode GrammarCode {
            get { return grammarCode; }
            set { SetPropertyValue(nameof(GrammarCode), ref grammarCode, value); }
        }

        [System.ComponentModel.Browsable(false)]
        [Association("DictionaryItemsVerses", UseAssociationNameAsIntermediateTableName =true)]
        public XPCollection<VerseInfo> VersesReferences {
            get { return GetCollection<VerseInfo>(nameof(VersesReferences)); }
        }

        public AncientDictionaryItem(Session session) : base(session) { }
    }
}
