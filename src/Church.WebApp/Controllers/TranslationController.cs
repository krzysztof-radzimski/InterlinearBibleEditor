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
using IBE.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
                    var translation = new XPQuery<Translation>(new UnitOfWork()).Where(x => x.Name.Replace("'", "").Replace("+", "") == translationName).FirstOrDefault();
                    if (translation != null) {
                        return View(new TranslationControllerModel(translation));
                    }
                }
                else {
                    var translation = new XPQuery<Translation>(new UnitOfWork()).Where(x => x.Name.Replace("'", "").Replace("+", "") == translationName).FirstOrDefault();
                    if (translation != null) {
                        var result = new TranslationControllerModel(translation, book, chapter, verse);

                        var view = new XPView(new UnitOfWork(), typeof(Translation));
                        view.CriteriaString = $"[Books][[NumberOfBook] = '{book}']";
                        view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
                        view.Properties.Add(new ViewProperty("Description", SortDirection.None, "[Description]", false, true));
                        view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
                        view.Properties.Add(new ViewProperty("Catholic", SortDirection.None, "[Catolic]", false, true));
                        view.Properties.Add(new ViewProperty("Recommended", SortDirection.None, "[Recommended]", false, true));
                        foreach (ViewRecord item in view) {
                            result.Translations.Add(new TranslationInfo() {
                                Name = item["Name"].ToString(),
                                Description = item["Description"].ToString(),
                                TranslationType = ((TranslationType)item["Type"]).GetDescription(),
                                Catholic = (bool)item["Catholic"],
                                Recommended = (bool)item["Recommended"]
                            });
                        }

                        return View(result);
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
        public List<TranslationInfo> Translations { get; }
        public TranslationControllerModel(Translation t, string b = null, string c = null, string v = null) {
            Translation = t;
            Book = b;
            Chapter = c;
            Verse = v;
            Translations = new List<TranslationInfo>();
        }
    }
    public class TranslationInfo {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TranslationType { get; set; }
        public bool Catholic { get; set; }
        public bool Recommended { get; set; }
    }
}
