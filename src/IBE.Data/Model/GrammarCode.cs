/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using IBE.Common.Extensions;
using System.ComponentModel;
using System.Xml.Linq;

namespace IBE.Data.Model {
    public class GrammarCode : XPObject {
        private string grammarCodeVariant1;
        private string grammarCodeVariant2;
        private string grammarCodeVariant3;
        private string grammarCodeDescription;
        private string shortDefinition;

        private PartOfSpeechType partOfSpeech;
        private CaseOfDeclinationType caseType;
        private NumberType number;
        private AdjectiveDegreeType adjectiveDegree;

        public string GrammarCodeVariant1 {
            get { return grammarCodeVariant1; }
            set { SetPropertyValue(nameof(GrammarCodeVariant1), ref grammarCodeVariant1, value); }
        }
        public string GrammarCodeVariant2 {
            get { return grammarCodeVariant2; }
            set { SetPropertyValue(nameof(GrammarCodeVariant2), ref grammarCodeVariant2, value); }
        }
        public string GrammarCodeVariant3 {
            get { return grammarCodeVariant3; }
            set { SetPropertyValue(nameof(GrammarCodeVariant3), ref grammarCodeVariant3, value); }
        }
        public string GrammarCodeDescription {
            get { return grammarCodeDescription; }
            set { SetPropertyValue(nameof(GrammarCodeDescription), ref grammarCodeDescription, value); }
        }
        public string ShortDefinition {
            get { return shortDefinition; }
            set { SetPropertyValue(nameof(ShortDefinition), ref shortDefinition, value); }
        }
        public PartOfSpeechType PartOfSpeech {
            get { return partOfSpeech; }
            set { SetPropertyValue(nameof(PartOfSpeech), ref partOfSpeech, value); }
        }
        public CaseOfDeclinationType CaseType {
            get { return caseType; }
            set { SetPropertyValue(nameof(CaseType), ref caseType, value); }
        }
        public NumberType Number {
            get { return number; }
            set { SetPropertyValue(nameof(Number), ref number, value); }
        }
        public AdjectiveDegreeType  AdjectiveDegree {
            get { return adjectiveDegree; }
            set { SetPropertyValue(nameof(AdjectiveDegree), ref adjectiveDegree, value); }
        }

        [NonPersistent]
        public string GrammarCodeDescriptionText {
            get {
                if (GrammarCodeDescription.IsNotNull()) {
                    try {
                        var value = XElement.Parse($"<div>{GrammarCodeDescription}</div>").Value();
                        return value.Replace("\r\n", " ").Replace("\n", " ");
                    }
                    catch { }
                }
                return default;
            }
        }

        [Association("VerseWordGrammarCodes")]
        public XPCollection<VerseWord> VerseWords {
            get { return GetCollection<VerseWord>(nameof(VerseWords)); }
        }

        [Association("GrammarCodesDictionaryItems")]
        public XPCollection<AncientDictionaryItem> DictionaryItems {
            get { return GetCollection<AncientDictionaryItem>(nameof(DictionaryItems)); }
        }

        public GrammarCode(Session session) : base(session) { }
    }

    public enum GenderType {
        [Description("")]
        None,
        [Description("Męski")]
        Masculine,
        [Description("Żeński")]
        Feminine,
        [Description("Nijaki")]
        Neuter,
    }

    public enum NumberType {
        [Description("")]
        None,
        [Description("Pojedyncza")]
        Singular,
        [Description("Mnoga")]
        Plural
    }

    public enum AdjectiveDegreeType {
        [Description("")]
        None,
        [Description("Równy")]
        Positive,
        [Description("Wyższy")]
        Comparative,
        [Description("Najwyższy")]
        Superlative
    }

    public enum CaseOfDeclinationType {
        [Description("")]
        None,
        [Description("Biernik")]
        Accusative,
        [Description("Celownik")]
        Dative,
        [Description("Dopełniacz")]
        Genitive,
        [Description("Mianownik")]
        Nominative,
        [Description("Wołacz")]
        Vocative,
        [Description("Nadrzędnik")]
        Instrumental,
        [Description("Miejscownik")]
        Locative
    }

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
        Indefinite_pronoun
    }
}
