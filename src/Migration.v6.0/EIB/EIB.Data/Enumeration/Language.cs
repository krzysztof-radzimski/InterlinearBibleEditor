using System.ComponentModel;

namespace EIB.Data.Enumeration {
    public enum Language {
        [Category("")] None,
        [Category("H")] Hebrew,
        [Category("G")] Greek,
        [Category("L")] Latin,
        [Category("EN")] English,
        [Category("PL")] Polish,
        [Category("UA")] Ukrainian
    }
}
