﻿/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using System.Text;

namespace ChurchServices.WebApp.Controllers {
    public class BibleByVerseController : Controller {
        protected readonly IConfiguration Configuration;
        public BibleByVerseController(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IActionResult Index() {
            var model = GetModel(10, 1, 1);
            return View(model);
        }

        [Route("/[controller]/{book}/{chapter}/{verse}")]
        public IActionResult Index(int book, int chapter, int verse) {
            var model = GetModel(book, chapter, verse);
            return View(model);
        }

        private BibleByVerseModel GetModel(int book, int chapter, int verse) {
            var uow = new UnitOfWork();
            string orginalText = "";
            string vovelsText = "";
            string vovelsName = "";
            string transliteration = "";

            var baseBooks = new XPQuery<BookBase>(uow).Where(x => x.Status.CanonType == CanonType.Canon).OrderBy(x => x.NumberOfBook).Select(x => new BibleByVerseBookInfo() {
                Name = x.BookShortcut,
                Number = x.NumberOfBook,
                FullName = x.BookTitle,
                Color = x.Color
            });
            if (!baseBooks.Select(x => x.Number).Contains(book)) { return GetModel(10, 1, 1); }

            if (book > 460) {
                var tro = new XPQuery<Verse>(uow).Where(x => x.Index == $"TRO16.{book}.{chapter}.{verse}").FirstOrDefault();
                if (tro == null) { return GetModel(book, 1, 1); }
                orginalText = tro.GetSourceText();
                var npi = new XPQuery<Verse>(uow).Where(x => x.Index == $"NPI.{book}.{chapter}.{verse}").First();
                vovelsText = npi.GetSourceText();
                transliteration = npi.GetTransliterationText();
                vovelsName = "Nestle Aland 28";
            }
            else {
                var lhb = new XPQuery<Verse>(uow).Where(x => x.Index == $"LHB.{book}.{chapter}.{verse}").FirstOrDefault();
                if (lhb == null) { return GetModel(book, 1, 1); }
                orginalText = lhb.Text;

                var hsb = new XPQuery<Verse>(uow).Where(x => x.Index == $"HSB.{book}.{chapter}.{verse}").First();
                vovelsText = hsb.GetSourceText();
                transliteration = hsb.GetTransliterationText();
                vovelsName = "Biblia Hebraica Stuttgartensia";
            }

            var model = new BibleByVerseModel() {
                Book = book,
                Chapter = chapter,
                Verse = verse,
                OrginalText = orginalText,
                VovelsText = vovelsText,
                VovelsName = vovelsName,
                Books = baseBooks.ToList(),
                Transliteration = transliteration
            };

            var baseBook = baseBooks.Where(x => x.Number == book).First();
            model.Shortcut = baseBook.Name;
            model.Title = $"{baseBook.Name} {chapter}:{verse}";

            model.BT = new XPQuery<Verse>(uow).Where(x => x.Index == $"BT99.{book}.{chapter}.{verse}").First().Text;
            var bw = new XPQuery<Verse>(uow).Where(x => x.Index == $"BW.{book}.{chapter}.{verse}").First();
            model.BW = bw.Text;
            model.UBG = new XPQuery<Verse>(uow).Where(x => x.Index == $"UBG18.{book}.{chapter}.{verse}").First().Text;
            model.BG = new XPQuery<Verse>(uow).Where(x => x.Index == $"BG.{book}.{chapter}.{verse}").First().Text;
            model.SNPD = new XPQuery<Verse>(uow).Where(x => x.Index == $"PBD.{book}.{chapter}.{verse}").First().Text;
            model.SNPD = model.SNPD.Substring(0, model.SNPD.IndexOf("<n>")).Replace("<n>", "").Replace("*", "");

            model.ChapterCount = bw.ParentChapter.ParentBook.NumberOfChapters;
            model.VerseCount = bw.ParentChapter.NumberOfVerses;

            return model;
        }
    }

    static class HebrewTransliterator {
        //   בְּרֵאשִׁ֖ית בָּרָ֣א אֱלֹהִ֑ים אֵ֥ת הַשָּׁמַ֖יִם וְאֵ֥ת הָאָֽרֶץ׃
        private static Dictionary<string, string> Map = new Dictionary<string, string>() {
            { "בְּ", "be" },
            { "רֵ" , "re"},
            { "שִׁ֖" , "sz"},
            { "י" , "i"},
            { "בָּ" , "ba"},
            { "רָ֣" , "ra" },
            { "אֱ", "e"},
            { "לֹ", "lo"},
            { "הִ֑", "h"},
            { "אֵ֥" , "e"},
            {  "הַ" , "ha"},
            { "שָּׁ" , "sza"},
            { "מַ֖", "ma"},
            { "יִ" , "ji"},
            {"וְ" , "we"},
            { "הָ" ,"ha"},
            { "אָֽ" , "a" },
            { "רֶ", "re"},
            { "שִׁ֖" , "sz"},

            {"א", ""},
            {"ב", "b"},
            {"ג", "g"},
            {"ד", "d"},
            {"ה", "h"},
            {"ו", "v"},
            {"ז", "z"},
            {"ח", "ch"},
            {"ט", "t"},
            {"כ", "k"},
            {"ל", "l"},
            {"מ", "m"},
            {"נ", "n"},
            {"ס", "s"},
            {"ע", ""},
            {"פ", "p"},
            {"צ", "c"},
            {"ק", "k"},
            {"ר", "r"},
            {"ש", "sz"},
            {"ת", "t"},
            {"ך", "k"},
            {"ם", "m"},
            {"ן", "n"},
            {"ף", "f"},
            {"ץ", "c"}
        };

        public static string Transliterate(this string hebrewText) {
            var transliteratedText = new StringBuilder();
            var _h = hebrewText.Clone().ToString();

            while (_h.Length > 0) {
                foreach (var l in Map) {
                    if (_h.StartsWith(l.Key)) {
                        _h = _h.Substring(l.Key.Length);
                        transliteratedText.Append(l.Value);
                    }
                }
            }



            //foreach (char c in hebrewText) {
            //    if (transliterationMap.ContainsKey(c))
            //        transliteratedText.Append(transliterationMap[c]);
            //    else
            //        transliteratedText.Append(c); // Dodaje niezmapowane znaki bez zmian
            //}

            return transliteratedText.ToString();
        }
    }
}