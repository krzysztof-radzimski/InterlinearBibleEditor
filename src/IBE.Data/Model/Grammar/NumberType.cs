using System.ComponentModel;

namespace IBE.Data.Model.Grammar {
    public enum NumberType {
        [Description("")]
        [Category("")]
        None,
        [Description("Pojedyncza")]
        [Category("S")]
        Singular,
        [Description("Mnoga")]
        [Category("P")]
        Plural
    }
}
