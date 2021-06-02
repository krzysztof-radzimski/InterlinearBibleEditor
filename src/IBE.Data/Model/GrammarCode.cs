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
using IBE.Data.Model.Grammar;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace IBE.Data.Model {
    public class GrammarCode : XPObject {
        private string grammarCodeVariant1;
        private string grammarCodeVariant2;
        private string grammarCodeVariant3;
        private string grammarCodeDescription;
        private string shortDefinition;

        private AdjectiveDegreeType adjectiveDegree;
        private CaseOfDeclinationType caseOfDeclination;
        private FormType form;
        private GenderType gender;
        private NumberType number;
        private PartOfSpeechType partOfSpeech;
        private PersonType person;
        private TenseType tense;
        private VoiceType voice;

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
        public CaseOfDeclinationType CaseOfDeclination {
            get { return caseOfDeclination; }
            set { SetPropertyValue(nameof(CaseOfDeclination), ref caseOfDeclination, value); }
        }
        public NumberType Number {
            get { return number; }
            set { SetPropertyValue(nameof(Number), ref number, value); }
        }
        public AdjectiveDegreeType AdjectiveDegree {
            get { return adjectiveDegree; }
            set { SetPropertyValue(nameof(AdjectiveDegree), ref adjectiveDegree, value); }
        }
        public FormType Form {
            get { return form; }
            set { SetPropertyValue(nameof(Form), ref form, value); }
        }
        public GenderType Gender {
            get { return gender; }
            set { SetPropertyValue(nameof(Gender), ref gender, value); }
        }
        public PersonType Person {
            get { return person; }
            set { SetPropertyValue(nameof(Person), ref person, value); }
        }
        public TenseType Tense {
            get { return tense; }
            set { SetPropertyValue(nameof(Tense), ref tense, value); }
        }
        public VoiceType Voice{
            get { return voice; }
            set { SetPropertyValue(nameof(Voice), ref voice, value); }
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


        public IReadOnlyCollection<KeyValuePair<string, string>> GetVersesInfo() {
            var result = new Dictionary<string, string>();
            var bookShortcuts = new XPQuery<BookBase>(this.Session).Select(x => new KeyValuePair<int, string>(x.NumberOfBook, x.BookShortcut)).ToList();
            var verses = new List<int>();
            var words = VerseWords.Where(x => x.Translation.IsNotNullOrEmpty());
            foreach (var word in words) {
                if (verses.Contains(word.ParentVerse.Oid)) { continue; }

                // NPI.470.14.10
                var index = word.ParentVerse.GetVerseIndex();
                if (!index.IsEmpty) {                    
                    var baseBookShortcut = bookShortcuts.Where(x => x.Key == index.NumberOfBook).Select(x => x.Value).FirstOrDefault();

                    var siglum = $@"<a href=""/{index.TranslationName}/{index.NumberOfBook}/{index.NumberOfChapter}/{index.NumberOfVerse}"" target=""_blank"" class=""text-decoration-none"">{baseBookShortcut} {index.NumberOfChapter}:{index.NumberOfVerse}</a>";
                    var text = word.ParentVerse.Text.Replace(word.Translation, $"<mark>{word.Translation}</mark>");

                    result.Add(siglum, text);

                    verses.Add(word.ParentVerse.Oid);
                }

            }
            return result;
        }
    }
}
