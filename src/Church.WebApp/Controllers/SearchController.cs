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
using IBE.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Church.WebApp.Controllers {
    public class SearchController : Controller {
        private readonly ILogger<SearchController> _logger;
        public SearchController(ILogger<SearchController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            var qs = Request.QueryString;
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 9) {
                //?text=ala+ma+kota
                var queryString = Uri.UnescapeDataString(qs.Value);
                var words = queryString.Replace("?text=", "").Split('+', StringSplitOptions.RemoveEmptyEntries);
                return _Search(words);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Index(string text) {
            if (!String.IsNullOrEmpty(text) && text.Length > 3) {
                var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return _Search(words);
            }
            return View();
        }

        private IActionResult _Search(string[] words) {
            var session = new UnitOfWork();
            var view = new XPView(session, typeof(Verse));
            var bookShortcuts = new XPQuery<BookBase>(session).Select(x => new KeyValuePair<int, string>(x.NumberOfBook, x.BookShortcut)).ToList();
            var translationNames = new XPQuery<Translation>(session).Where(x=>!x.Hidden).Select(x => new KeyValuePair<string, string>(x.Name.Replace("'", "").Replace("+", ""), x.Description)).ToList();

            var critera = String.Empty;
            foreach (var word in words) {

                critera += $"Contains(Lower([Text]),'{word.ToLower()}')";

                if (word != words.Last()) {
                    critera += " AND ";
                }
            }

            view.CriteriaString = critera;


            //view.Properties.Add(new ViewProperty("NumberOfBook", SortDirection.None, "[ParentChapter.ParentBook.NumberOfBook]", false, true));
            //view.Properties.Add(new ViewProperty("BookShortcut", SortDirection.None, "[ParentChapter.ParentBook.BookShortcut]", false, true));
            //view.Properties.Add(new ViewProperty("NumberOfChapter", SortDirection.None, "[ParentChapter.NumberOfChapter]", false, true));
            view.Properties.Add(new ViewProperty("NumberOfVerse", SortDirection.None, "[NumberOfVerse]", false, true));
            view.Properties.Add(new ViewProperty("VerseText", SortDirection.None, "[Text]", false, true));
            //view.Properties.Add(new ViewProperty("TranslationName", SortDirection.None, "[ParentChapter.ParentBook.ParentTranslation.Description]", false, true));
            //view.Properties.Add(new ViewProperty("Translation", SortDirection.None, "[ParentChapter.ParentBook.ParentTranslation.Name]", false, true));
            view.Properties.Add(new ViewProperty("Index", SortDirection.None, "[Index]", false, true));

            var model = new SearchResultsModel(words);
            foreach (ViewRecord record in view) {

                var index = new VerseIndex(record["Index"].ToString());
                var baseBookShortcut = bookShortcuts.Where(x => x.Key == index.NumberOfBook).Select(x => x.Value).FirstOrDefault();
                var translation = translationNames.Where(x => x.Key == index.TranslationName).FirstOrDefault();
                if (translation.IsNull() || translation.Key.IsNull()) { continue; }
                var translationDesc = translation.Value;

                model.Add(new SearchItemModel() {
                    Book = index.NumberOfBook,//record["NumberOfBook"].ToInt(),
                    BookShortcut = baseBookShortcut,//record["BookShortcut"].ToString(),
                    Chapter = index.NumberOfChapter,//record["NumberOfChapter"].ToInt(),
                    Verse = record["NumberOfVerse"].ToInt(),
                    TranslationName = translationDesc,//record["TranslationName"].ToString(),
                    Translation = index.TranslationName,//record["Translation"].ToString().Replace("'", "").Replace("+", ""),
                    VerseText = record["VerseText"].ToString()
                });
            }

            return View(model);
        }
    }

    public class SearchResultsModel : List<SearchItemModel> {
        public IEnumerable<string> Words { get; }
        public SearchResultsModel(params string[] words) {
            Words = words;
        }
    }

    public class SearchItemModel {
        public string TranslationName { get; set; }
        public string Translation { get; set; }
        public string BookShortcut { get; set; }
        public int Book { get; set; }
        public int Chapter { get; set; }
        public int Verse { get; set; }
        public string VerseText { get; set; }
    }
}
