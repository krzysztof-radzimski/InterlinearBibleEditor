using System.ComponentModel;

namespace IBE.Data.Model.Grammar {
    public enum CaseOfDeclinationType {
        [Description("")]
        [Category("")]
        None,
        [Description("Biernik")]
        [Category("A")]
        Accusative,
        [Description("Celownik")]
        [Category("D")]
        Dative,
        [Description("Dopełniacz")]
        [Category("G")]
        Genitive,
        [Description("Mianownik")]
        [Category("N")]
        Nominative,
        [Description("Wołacz")]
        [Category("V")]
        Vocative,
        [Description("Nadrzędnik")]
        [Category("I")]
        Instrumental,
        [Description("Miejscownik")]
        [Category("L")]
        Locative
    }
}
