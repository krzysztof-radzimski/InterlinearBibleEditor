using System.ComponentModel;

namespace IBE.Data.Model {
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
