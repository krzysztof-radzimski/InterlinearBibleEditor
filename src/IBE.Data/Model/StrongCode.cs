using DevExpress.Xpo;
using System.ComponentModel;

namespace IBE.Data.Model {
    public class StrongCode : XPObject {
        private Language lang;
        private int code;
        private string transliteration;
        private string sourceWord;
        private string pronunciation;
        private string strongsdef;
        private string kjvdef;
        private string derivation;

        public Language Lang {
            get { return lang; }
            set { SetPropertyValue(nameof(Lang), ref lang, value); }
        }

        public int Code {
            get { return code; }
            set { SetPropertyValue(nameof(Code), ref code, value); }
        }

        public string Transliteration {
            get { return transliteration; }
            set { SetPropertyValue(nameof(Transliteration), ref transliteration, value); }
        }

        public string SourceWord {
            get { return sourceWord; }
            set { SetPropertyValue(nameof(SourceWord), ref sourceWord, value); }
        }

        public string Pronunciation {
            get { return pronunciation; }
            set { SetPropertyValue(nameof(Pronunciation), ref pronunciation, value); }
        }

        public string StrongsDef {
            get { return strongsdef; }
            set { SetPropertyValue(nameof(StrongsDef), ref strongsdef, value); }
        }

        /// <summary>
        /// Root Word (Etymology)
        /// </summary>
        public string Derivation {
            get { return derivation; }
            set { SetPropertyValue(nameof(Derivation), ref derivation, value); }
        }

        public string KjvDef {
            get { return kjvdef; }
            set { SetPropertyValue(nameof(KjvDef), ref kjvdef, value); }
        }

        [Association("StrongsVerseWords")]
        public XPCollection<VerseWord> VerseWords {
            get { return GetCollection<VerseWord>(nameof(VerseWords)); }
        }

        public StrongCode(Session session) : base(session) { }
    }

    public enum Language {
        None,
        [Category("H")]
        Hebrew,
        [Category("G")]
        Greek,
        [Category("L")]
        Latin,
        [Category("EN")]
        English,
        [Category("PL")]
        Polish
    }
}
