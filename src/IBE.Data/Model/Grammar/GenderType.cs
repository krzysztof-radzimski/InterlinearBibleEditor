using System.ComponentModel;

namespace IBE.Data.Model.Grammar {
    public enum GenderType {
        [Description("")]
        [Category("")]
        None,
        [Description("Męski")]
        [Category("M")]
        Masculine,
        [Description("Żeński")]
        [Category("F")]
        Feminine,
        [Description("Nijaki")]
        [Category("N")]
        Neuter,
    }
}
