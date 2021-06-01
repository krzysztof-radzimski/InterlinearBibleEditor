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
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace IBE.Data.Model {
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

        [NonPersistent]
        public string Topic { get { return $"{Lang.GetCategory()}{Code}"; } }

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

        public IReadOnlyCollection<KeyValuePair<string, string>> GetVersesInfo() {
            var result = new Dictionary<string, string>();
            var bookShortcuts = new XPQuery<BookBase>(this.Session).Select(x => new KeyValuePair<int, string>(x.NumberOfBook, x.BookShortcut)).ToList();
            var verses = new List<int>();
            var words = VerseWords.Where(x => x.Translation.IsNotNullOrEmpty());
            foreach (var word in words) {
                if (verses.Contains(word.ParentVerse.Oid)) { continue; }

                // NPI.470.14.10
                var regex = new Regex(@"(?<translation>[A-Z]+)\.(?<book>[0-9]+)\.(?<chapter>[0-9]+)\.(?<verse>[0-9]+)");
                var m = regex.Match(word.ParentVerse.Index);

                var numOfBook = m.Groups["book"].Value.ToInt();
                var baseBookShortcut = bookShortcuts.Where(x => x.Key == numOfBook).Select(x => x.Value).FirstOrDefault();

                var siglum = $@"<a href=""/{m.Groups["translation"].Value}/{m.Groups["book"].Value}/{m.Groups["chapter"].Value}/{m.Groups["verse"].Value}"" target=""_blank"" class=""text-decoration-none"">{baseBookShortcut} {m.Groups["chapter"].Value}:{m.Groups["verse"].Value}</a>";
                var text = word.ParentVerse.Text.Replace(word.Translation, $"<mark>{word.Translation}</mark>");

                result.Add(siglum, text);

                verses.Add(word.ParentVerse.Oid);

            }
            return result;
        }
    }
}
