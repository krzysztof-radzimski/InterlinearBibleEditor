/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using IBE.Common.Extensions;

namespace IBE.Data.Model {
    public class StrongCode : XPObject {
        private Language lang;
        private int code;
        private string transliteration;
        private string sourceWord;
        private string pronunciation;
        private string shortDefinition;
        private string definition;
        
        public Language Lang {
            get { return lang; }
            set { SetPropertyValue(nameof(Lang), ref lang, value); }
        }

        public int Code {
            get { return code; }
            set { SetPropertyValue(nameof(Code), ref code, value); }
        }

        [NonPersistent]
        public string Topic { get { return $"{Lang.GetCategory()}{Code}"; } }

        [Size(200)]
        public string Transliteration {
            get { return transliteration; }
            set { SetPropertyValue(nameof(Transliteration), ref transliteration, value); }
        }

        [Size(200)]
        public string SourceWord {
            get { return sourceWord; }
            set { SetPropertyValue(nameof(SourceWord), ref sourceWord, value); }
        }

        [Size(500)]
        public string Pronunciation {
            get { return pronunciation; }
            set { SetPropertyValue(nameof(Pronunciation), ref pronunciation, value); }
        }

        [Size(1000)]
        public string ShortDefinition {
            get { return shortDefinition; }
            set { SetPropertyValue(nameof(ShortDefinition), ref shortDefinition, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Definition {
            get { return definition; }
            set { SetPropertyValue(nameof(Definition), ref definition, value); }
        }

        [Association("StrongsVerseWords")]
        public XPCollection<VerseWord> VerseWords {
            get { return GetCollection<VerseWord>(nameof(VerseWords)); }
        }

        [Association("StrongsDictionaryItems")]
        public XPCollection<AncientDictionaryItem> DictionaryItems {
            get { return GetCollection<AncientDictionaryItem>(nameof(DictionaryItems)); }
        }

        public StrongCode(Session session) : base(session) { }                
    }
}
