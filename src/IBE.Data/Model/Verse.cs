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

namespace IBE.Data.Model {
    public class Verse : XPObject {
        private int numberOfVerse;
        private Chapter parentChapter;
        private string text;
        private string index;
        private bool startFromNewLine;

        public int NumberOfVerse {
            get { return numberOfVerse; }
            set { SetPropertyValue(nameof(NumberOfVerse), ref numberOfVerse, value); }
        }

        [Association("ChapterVerses")]
        public Chapter ParentChapter {
            get { return parentChapter; }
            set { SetPropertyValue(nameof(ParentChapter), ref parentChapter, value); }
        }

        [Association("VerseWords")]
        public XPCollection<VerseWord> VerseWords {
            get { return GetCollection<VerseWord>(nameof(VerseWords)); }
        }

        /// <summary>
        /// Treść wersetu jeżeli nie jest to przekład interlinearny. Treść zapisywana jest w pseudo-HTML
        /// </summary>
        [Size(SizeAttribute.Unlimited)]
        public string Text {
            get { return text; }
            set { SetPropertyValue(nameof(Text), ref text, value); }
        }

        public bool StartFromNewLine {
            get { return startFromNewLine; }
            set { SetPropertyValue(nameof(StartFromNewLine), ref startFromNewLine, value); }
        }

        public string Index {
            get { return index; }
            set { SetPropertyValue(nameof(Index), ref index, value); }
        }


        [NonPersistent]
        public Translation ParentTranslation {
            get {
                if (ParentChapter != null) {
                    return ParentChapter.ParentTranslation;
                }
                return default;
            }
        }

        public Verse(Session session) : base(session) { }

        public string GetBookName() => ParentChapter.ParentBook.BaseBook.BookName;

        public string GetSourceText() {
            var text = string.Empty;
            foreach (var item in VerseWords) {
                text += item.SourceWord + " ";
            }
            return text.Trim();
        }

        public string GetTranslationText() {
            var text = string.Empty;
            foreach (var item in VerseWords) {
                text += item.Translation + " ";
            }
            return text.Trim();
        }

        public string GetTransliterationText() {
            var text = string.Empty;
            foreach (var item in VerseWords) {
                text += item.Transliteration + " ";
            }
            return text.Trim();
        }

        public string GetOblubienicaUrl() {
            var vi = GetVerseIndex();
            if (!vi.IsEmpty) {
                return $"https://biblia.oblubienica.eu/interlinearny/index/book/{vi.NTBookNumber}/chapter/{vi.NumberOfChapter}/verse/{vi.NumberOfVerse}";
            }
            return default;
        }
        public VerseIndex GetVerseIndex() {
            return new VerseIndex(Index);
        }

        public string GetLogosSeptuagintUrl() {
            var vi = GetVerseIndex();
            if (!vi.IsEmpty) {
                return $"https://app.logos.com/books/LLS%3ALELXX/references/bible%2Blxx2.{vi.OTBookNumber}.{vi.NumberOfChapter}.{vi.NumberOfVerse}";
            }
            return default;
        }

        public string GetSiglum(VerseIndex index = null, params BookBase[] bookBases) {
            if (bookBases.IsNotNull()) {
                if (index == null) { index = GetVerseIndex(); }
                var baseBook = bookBases.Where(x => x.NumberOfBook == index.NumberOfBook).FirstOrDefault();
                if (baseBook.IsNotNull()) {
                    return $"{baseBook.BookShortcut} {index.NumberOfChapter}:{index.NumberOfVerse}";
                }
            }

            return $"{GetBookName()} {ParentChapter.NumberOfChapter}:{NumberOfVerse}";
        }
        public string GetBookName(VerseIndex index = null, params BookBase[] bookBases) {
            if (bookBases.IsNotNull()) {
                if (index == null) { index = GetVerseIndex(); }
                var baseBook = bookBases.Where(x => x.NumberOfBook == index.NumberOfBook).FirstOrDefault();
                if (baseBook.IsNotNull()) {
                    return baseBook.BookName;
                }
            }
            return GetBookName();
        }
        public string GetSimpleText(VerseIndex index = null, params BookBase[] bookBases) {
            if (index == null) { index = GetVerseIndex(); }
            var translation = index.TranslationName;
            var baseBookShortcut = bookBases != null ? bookBases.Where(x => x.NumberOfBook == index.NumberOfBook).First().BookShortcut : ParentChapter.ParentBook.BaseBook.BookShortcut;
            var verseText = Text;
            var simpleText = verseText.Replace("</t>", "").Replace("<t>", "").Replace("<pb/>", "").Replace("<n>", "").Replace("</n>", "").Replace("<e>", "").Replace("</e>", "").Replace("―", "").Replace('\'', ' ').Replace("<J>", "").Replace("</J>", "").Replace("<i>", "").Replace("</i>", "");
            if (translation == "NPI" || translation == "IPD") {
                simpleText = simpleText.Replace("―", "");
            }
            if (translation == "PBD") { translation = "SNPPD"; }
            simpleText = System.Text.RegularExpressions.Regex.Replace(simpleText, @"\<f\>\[[0-9]+\]\<\/f\>", "");
            simpleText = $"{baseBookShortcut} {index.NumberOfChapter}:{index.NumberOfVerse} „{simpleText}” ({translation})";
            return simpleText;
        }
    }


}
