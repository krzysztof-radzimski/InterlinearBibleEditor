using System.ComponentModel;

namespace IBE.Data.Model.Grammar {
    public enum FormType {
        [Description("")]
        [Category("")]
        None,
        [Description("Domyślna")]
        [Category("")]
        Long,
        [Description("Skrócona")]
        [Category("C")]
        Contracted,
        [Description("Skrócona z kai")]
        [Category("K")]
        Contracted_with_kai,
        [Description("Attycka")]
        [Category("ATT")]
        Attic,
        [Description("W skrócie")]
        [Category("ABB")]
        Abbrevieated
    }
}
