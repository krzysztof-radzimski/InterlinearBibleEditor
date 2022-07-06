namespace ChurchServices.Data.Model.Grammar {
    public enum PartOfSpeechType {
        [Description("")]
        [Category("")]
        None,
        [Description("Przymiotnik")]
        [Category("A")]
        Adjective,
        [Description("Przysłówek")]
        [Category("ADV")]
        ADVerb,
        [Description("Arameizm")]
        [Category("ARAM")]
        ARAM,
        [Description("Zaimek odwrotny")]
        [Category("C")]
        reCiprocal_pronoun,
        [Description("Część warunkowa lub koniunkcja ")]
        [Category("COND")]
        CONDitional_particle_or_conjunction,
        [Description("Koniunkcja")]
        [Category("CONJ")]
        CONJunction,
        [Description("Zaimek wskazujący")]
        [Category("D")]
        Demonstrative_pronoun,
        [Description("Zaimek zwrotny")]
        [Category("F")]
        reFlexive_pronoun,
        [Description("Hebraizm")]
        [Category("HEB")]
        HEB,
        [Description("Zaimek pytający")]
        [Category("I")]
        Interrogative_pronoun,
        [Description("Wykrzyknik")]
        [Category("INJ")]
        INterJection,
        [Description("Zaimek korelacyjny")]
        [Category("K")]
        Correlative_pronoun,
        [Description("Rzeczownik")]
        [Category("N")]
        Noun,
        [Description("Zaimek osobowy")]
        [Category("P")]
        Personal_pronoun,
        [Description("Przyimek")]
        [Category("PREP")]
        PREP,
        [Description("Część rozłączna")]
        [Category("PRT")]
        PRT,
        [Description("Zaimek korelacyjny lub pytający")]
        [Category("Q")]
        Correlative_or_interrogative_pronoun,
        [Description("Zaimek względny")]
        [Category("R")]
        Relative_pronoun,
        [Description("Zaimek dzierżawczy")]
        [Category("S")]
        poSsessive_pronoun,
        [Description("Przedimek określony")]
        [Category("T")]
        Definite_article,
        [Description("Czasownik")]
        [Category("V")]
        Verb,
        [Description("Zaimek pytający nieokreślony")]
        [Category("RI")]
        interrogative_Indefinite_pronoun,
        [Description("Zaimek nieokreślony")]
        [Category("X")]
        Indefinite_pronoun,
        [Description("Liczebnik nieokreślony")]
        [Category("NU")]
        Indeclinable_NUmeral
    }
}
