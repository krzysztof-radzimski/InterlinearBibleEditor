using System.ComponentModel;

namespace EIB.Data.Enumeration {
    public enum CanonType {
        [Description("")] None = 0,
        [Description("Księga kanoniczna")] Canon = 1,
        [Description("Księga deuterokanoniczna")] SecondCanon = 2,
        [Description("Księga niekanoniczna")] Apocrypha = 3
    }
}
