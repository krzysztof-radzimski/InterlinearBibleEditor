namespace ChurchServices.Data.Model.Grammar {
    public enum TenseType {
        [Description("")]
        [Category("")]
        None,
        [Description("Brak określonego czasu (przysłówek rozkazujący)")]
        [Category("X")]
        WithoutTense,
        [Description("Teraźniejszy")]
        [Category("P")]
        Present,
        [Description("Przyszły")]
        [Category("F")]
        Future,
        [Description("Przyszły wtórny")]
        [Category("2F")]
        SecondFuture,
        [Description("Przeszły dokonany")]
        [Category("P")]
        Perfect,
        [Description("Zaprzeszły")]
        [Category("2P")]
        SecondPerfect,
        [Description("Aorist")]
        [Category("A")]
        Aorist,
        [Description("Drugi Aorist")]
        [Category("2A")]
        SecondAorist,
        [Description("Zaprzeszły")]
        [Category("L")]
        Pluperfect,
        [Description("Zaprzeszły wtórny")]
        [Category("2L")]
        SecondPluperfect
    }
}
