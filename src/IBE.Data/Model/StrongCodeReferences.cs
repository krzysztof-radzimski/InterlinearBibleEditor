using DevExpress.Xpo;

namespace IBE.Data.Model {
    public class StrongCodeReferences : XPObject {
        private Language lang;
        private int code;
        private StrongCode parent;

        public Language Lang {
            get { return lang; }
            set { SetPropertyValue(nameof(Lang), ref lang, value); }
        }

        public int Code {
            get { return code; }
            set { SetPropertyValue(nameof(Code), ref code, value); }
        }

        [Association("StrongsCodesReferences")]
        public StrongCode Parent {
            get { return parent; }
            set { SetPropertyValue(nameof(Parent), ref parent, value); }
        }

        public StrongCodeReferences(Session session) : base(session) { }
    }
}
