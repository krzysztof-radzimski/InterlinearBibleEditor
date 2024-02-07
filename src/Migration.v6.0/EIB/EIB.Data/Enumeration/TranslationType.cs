using System.ComponentModel;

namespace EIB.Data.Enumeration {
    public enum TranslationType {
        [Description("")][Category("0")] None = 0,
        //[Description("Przekład interlinearny")][Category("1")] Interlinear = 1,
        [Description("Przekład literacki")][Category("3")] Default = 2,
        //[Description("Przekład dynamiczny")][Category("4")] Dynamic = 3,
        [Description("Przekład dosłowny")][Category("2")] Literal = 4
    }
}
