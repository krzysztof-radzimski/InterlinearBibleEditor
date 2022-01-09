/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using IBE.Data.Export.Model;
using IBE.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Church.WebApp.Controllers {
    public class TranslationController : Controller {
        protected readonly IConfiguration Configuration;

        public TranslationController(IConfiguration configuration) {
            Configuration = configuration;
        }

        // "{translationName}/{book?}/{chapter?}/{verse?}"
        [TranslationAuthorize]
        public IActionResult Index(string translationName, string book = null, string chapter = null, string verse = null) {
            if (!String.IsNullOrEmpty(translationName)) {
                var uow = new UnitOfWork();
                var books = new XPQuery<BookBase>(uow).ToList();

                // wyświetlamy listę ksiąg z tego przekładu
                if (String.IsNullOrEmpty(book)) {
                    var translation = new XPQuery<Translation>(uow).Where(x => x.Name.Replace("'", "").Replace("+", "") == translationName).FirstOrDefault();
                    if (translation != null) {
                        return View(new TranslationControllerModel(translation, books: books));
                    }
                }
                else {
                    var translation = new XPQuery<Translation>(uow).Where(x => x.Name.Replace("'", "").Replace("+", "") == translationName).FirstOrDefault();
                    if (translation != null) {
                        var result = new TranslationControllerModel(translation, book, chapter, verse, books);

                        var view = new XPView(uow, typeof(Translation)) {
                            CriteriaString = $"[Books][[NumberOfBook] = '{book}'] AND [Hidden] = 0"
                        };
                        view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
                        view.Properties.Add(new ViewProperty("Description", SortDirection.None, "[Description]", false, true));
                        view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
                        view.Properties.Add(new ViewProperty("Catholic", SortDirection.None, "[Catolic]", false, true));
                        view.Properties.Add(new ViewProperty("Recommended", SortDirection.None, "[Recommended]", false, true));
                        view.Properties.Add(new ViewProperty("OpenAccess", SortDirection.None, "[OpenAccess]", false, true));
                        foreach (ViewRecord item in view) {
                            result.Translations.Add(new TranslationInfo() {
                                Name = item["Name"].ToString(),
                                Description = item["Description"].ToString(),
                                Type = (TranslationType)item["Type"],
                                Catholic = (bool)item["Catholic"],
                                Recommended = (bool)item["Recommended"],
                                PasswordRequired = !((bool)item["OpenAccess"])
                            });
                        }

                        return View(result);
                    }
                }
            }

            return View();
        }
    }
}
