using System.ComponentModel;

namespace EIB.Data.Enumeration {
    public enum TheBookType {
        [Description("")] None = 0,
        [Description("Biblia")] Bible = 1,
        [Description("Patrystyka")] ChurchFathersLetter = 2,
        [Description("Komentarz")] Commentary = 3,
        [Description("Monografia")] Other = 4
    }
}
