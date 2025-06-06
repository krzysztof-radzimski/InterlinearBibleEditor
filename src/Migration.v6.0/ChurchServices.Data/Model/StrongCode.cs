﻿/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.Data.Model {
    public class StrongCode : XPObject {
        private Language lang;
        private int code;
        private string transliteration;
        private string sourceWord;
        private string pronunciation;
        private string shortDefinition;
        private string definition;

        public Language Lang {
            get { return lang; }
            set { SetPropertyValue(nameof(Lang), ref lang, value); }
        }

        public int Code {
            get { return code; }
            set { SetPropertyValue(nameof(Code), ref code, value); }
        }

        [NonPersistent] public string Topic { get { return $"{Lang.GetCategory()}{Code}"; } }
        [NonPersistent] public string FullCode { get { return $"{Lang.GetCategory()}{Code.ToString().PadLeft(4, '0')}"; } }

        [Size(200)]
        public string Transliteration {
            get { return transliteration; }
            set { SetPropertyValue(nameof(Transliteration), ref transliteration, value); }
        }

        [Size(200)]
        public string SourceWord {
            get { return sourceWord; }
            set { SetPropertyValue(nameof(SourceWord), ref sourceWord, value); }
        }

        [Size(500)]
        public string Pronunciation {
            get { return pronunciation; }
            set { SetPropertyValue(nameof(Pronunciation), ref pronunciation, value); }
        }

        [Size(1000)]
        public string ShortDefinition {
            get { return shortDefinition; }
            set { SetPropertyValue(nameof(ShortDefinition), ref shortDefinition, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Definition {
            get { return definition; }
            set { SetPropertyValue(nameof(Definition), ref definition, value); }
        }

        [Association("StrongsVerseWords")]
        public XPCollection<VerseWord> VerseWords {
            get { return GetCollection<VerseWord>(nameof(VerseWords)); }
        }

        [Association("StrongsDictionaryItems")]
        public XPCollection<AncientDictionaryItem> DictionaryItems {
            get { return GetCollection<AncientDictionaryItem>(nameof(DictionaryItems)); }
        }

        public StrongCode(Session session) : base(session) { }

        public IReadOnlyCollection<StrongVerseInfo> GetVersesInfo() {
            var result = new List<StrongVerseInfo>();
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

                    result.Add(new StrongVerseInfo(siglum, text, index));

                    verses.Add(word.ParentVerse.Oid);
                }

            }
            return result;
        }
    }

    public class StrongVerseInfo {
        public string Siglum { get; set; }
        public string Text { get; set; }
        public VerseIndex Index { get; set; }
        public StrongVerseInfo(string siglum, string text, VerseIndex index) {
            Siglum = siglum;
            Text = text;
            Index = index;
        }
    }
}
