using System.ComponentModel;

namespace IBE.Data.Model.Grammar {
    public enum PersonType {
        [Description("")]
        [Category("")]
        None,
        [Description("Pierwsza")]
        [Category("1")]
        First,
        [Description("Druga")]
        [Category("2")]
        Second,
        [Description("Trzecia")]
        [Category("3")]
        Third
    }
}
