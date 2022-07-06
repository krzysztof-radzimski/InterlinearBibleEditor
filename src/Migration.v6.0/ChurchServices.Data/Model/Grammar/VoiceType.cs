namespace ChurchServices.Data.Model.Grammar {
    public enum VoiceType {
        [Description("")]
        [Category("")]
        None,
        [Description("Brak określonego głosu")]
        [Category("X")]
        WithoutVoice,
        [Description("Aktywny - strona czynna")]
        [Category("A")]
        Active,
        [Description("Środkowy - strona medialna")]
        [Category("M")]
        Middle,
        [Description("Środkowy deponencyjny")]
        [Category("D")]
        MiddleDeponent,
        [Description("Pasywny - strona bierna")]
        [Category("P")]
        Passive
    }
}
