namespace ChurchServices.Data.Model.Grammar {
    public enum AdjectiveDegreeType {
        [Description("")]
        [Category("")]
        None,
        [Description("Równy")]
        [Category("P")]
        Positive,
        [Description("Wyższy")]
        [Category("C")]
        Comparative,
        [Description("Najwyższy")]
        [Category("S")]
        Superlative,
        [Description("Przeczący")]
        [Category("N")]
        Negative
    }
}
