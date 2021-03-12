/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using IBE.Data.Model;
using IBE.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;

namespace Church.WebApp.Controllers {
    public class TranslationController : Controller {
        private readonly ILogger<TranslationController> _logger;
        public TranslationController(ILogger<TranslationController> logger) {
            _logger = logger;
        }

        // "{translationName}/{book?}/{chapter?}/{verse?}"
        public IActionResult Index(string translationName, string book = null, string chapter = null, string verse = null) {
            if (!String.IsNullOrEmpty(translationName)) {
                // wyświetlamy listę ksiąg z tego przekładu
                if (String.IsNullOrEmpty(book)) {
                    var translation = new XPQuery<Translation>(new UnitOfWork()).Where(x => x.Name.Replace("'", "") == translationName).FirstOrDefault();
                    if (translation != null) {
                        return View(new TranslationControllerModel(translation));
                    }
                }
                else {
                    var translation = new XPQuery<Translation>(new UnitOfWork()).Where(x => x.Name.Replace("'", "") == translationName).FirstOrDefault();
                    if (translation != null) {
                        return View(new TranslationControllerModel(translation, book, chapter, verse));
                    }
                }
            }

            return View();
        }
    }

    public class TranslationControllerModel {
        public Translation Translation { get; }
        public string Book { get; }
        public string Chapter { get; }
        public string Verse { get; }

        public TranslationControllerModel(Translation t, string b = null, string c = null, string v = null) {
            Translation = t;
            Book = b;
            Chapter = c;
            Verse = v;
        }
    }
}
