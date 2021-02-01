using DevExpress.Xpo;

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
        private string descriptionText;

        public Language Lang {
            get { return lang; }
            set { SetPropertyValue(nameof(Lang), ref lang, value); }
        }

        public int Code {
            get { return code; }
            set { SetPropertyValue(nameof(Code), ref code, value); }
        }

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
        public string StrongsDef {
            get { return strongsdef; }
            set { SetPropertyValue(nameof(StrongsDef), ref strongsdef, value); }
        }

        /// <summary>
        /// Root Word (Etymology)
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public string Derivation {
            get { return derivation; }
            set { SetPropertyValue(nameof(Derivation), ref derivation, value); }
        }

        [Size(1000)]
        public string KjvDef {
            get { return kjvdef; }
            set { SetPropertyValue(nameof(KjvDef), ref kjvdef, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string DescriptionText {
            get { return descriptionText; }
            set { SetPropertyValue(nameof(DescriptionText), ref descriptionText, value); }
        }

        [Association("StrongsVerseWords")]
        public XPCollection<VerseWord> VerseWords {
            get { return GetCollection<VerseWord>(nameof(VerseWords)); }
        }

        [Association("StrongsCodesReferences")]
        public XPCollection<StrongCodeReferences> References {
            get { return GetCollection<StrongCodeReferences>(nameof(References)); }
        }

        public StrongCode(Session session) : base(session) { }

        public override string ToString() {
            try {
                return $@"<strong>{SourceWord} {Transliteration}</strong>, {Pronunciation}; {Derivation}{StrongsDef}{KjvDef} {DescriptionText}";
            }
            catch {
                return base.ToString();
            }
        }
    }
}
