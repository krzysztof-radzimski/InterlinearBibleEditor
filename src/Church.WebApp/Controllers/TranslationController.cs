/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using Church.WebApp.Utils;
using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Church.WebApp.Controllers {
    public class TranslationController : Controller {
        private readonly IConfiguration Configuration;
        private readonly ITranslationInfoController TranslationInfoController;
        public TranslationController(IConfiguration configuration, ITranslationInfoController translationInfoController) {
            Configuration = configuration;
            TranslationInfoController = translationInfoController;
        }

        // "{translationName}/{book?}/{chapter?}/{verse?}"
        [TranslationAuthorize]
        public IActionResult Index(string translationName, string book = null, string chapter = null, string verse = null) {
            if (translationName == "SNPPD") { translationName = "PBD"; }

            // adresy skrótowe
            if (!String.IsNullOrEmpty(translationName) && book.IsNull() && translationName.Length == 5) {
                var uow = new UnitOfWork();
                var _url = new XPQuery<UrlShort>(uow).Where(x => x.ShortUrl == translationName).FirstOrDefault();
                if (_url.IsNotNull()) {
                    return Redirect(_url.Url);
                }
            }

            var result = TranslationInfoController.GetTranslationControllerModel(translationName, book, chapter, verse);
            if (result != null) {
                return View(result);
            }

            //if (!String.IsNullOrEmpty(translationName)) {

            //    var books = new XPQuery<BookBase>(uow).ToList();

            //    // wyświetlamy listę ksiąg z tego przekładu
            //    if (String.IsNullOrEmpty(book)) {
            //        var translation = new XPQuery<Translation>(uow).Where(x => x.Name.Replace("'", "").Replace("+", "").ToLower() == translationName.ToLower()).FirstOrDefault();
            //        if (translation != null) {
            //            return View(new TranslationControllerModel(translation, books: books));
            //        }
            //    }
            //    else {
            //        var translation = new XPQuery<Translation>(uow).Where(x => x.Name.Replace("'", "").Replace("+", "").ToLower() == translationName.ToLower()).FirstOrDefault();
            //        if (translation != null) {
            //            var result = new TranslationControllerModel(translation, book, chapter, verse, books);

            //            var view = new XPView(uow, typeof(Translation)) {
            //                CriteriaString = $"[Books][[NumberOfBook] = '{book}'] AND [Hidden] = 0"
            //            };
            //            view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
            //            view.Properties.Add(new ViewProperty("Description", SortDirection.None, "[Description]", false, true));
            //            view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
            //            view.Properties.Add(new ViewProperty("Catholic", SortDirection.None, "[Catolic]", false, true));
            //            view.Properties.Add(new ViewProperty("Recommended", SortDirection.None, "[Recommended]", false, true));
            //            view.Properties.Add(new ViewProperty("OpenAccess", SortDirection.None, "[OpenAccess]", false, true));
            //            foreach (ViewRecord item in view) {
            //                result.Translations.Add(new TranslationInfo() {
            //                    Name = item["Name"].ToString(),
            //                    Description = item["Description"].ToString(),
            //                    Type = (TranslationType)item["Type"],
            //                    Catholic = (bool)item["Catholic"],
            //                    Recommended = (bool)item["Recommended"],
            //                    PasswordRequired = !((bool)item["OpenAccess"])
            //                });
            //            }

            //            return View(result);
            //        }
            //    }
            //}

            return View();
        }
    }
}
