/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> Logger;
        private readonly ITranslationInfoController TranslationInfoController;
        public HomeController(ILogger<HomeController> logger, ITranslationInfoController translationInfoController) {
            Logger = logger;
            TranslationInfoController = translationInfoController;
        }

        public IActionResult Index() {  
            return View(TranslationInfoController.GetLastFourArticles());          
        }

        public IActionResult WhatWeBelieve() =>View();

        public IActionResult About() => View();

        public IActionResult Service() => View();

        public IActionResult Privacy() => View();

        [Authorize]
        public IActionResult Secured() => View();

        public IActionResult AboutBible() {
            var uow = new UnitOfWork();
            var books = TranslationInfoController.GetBookBases(uow);
            var translation = new XPQuery<Translation>(uow).Where(x => x.Name.Replace("'", "").Replace("+", "").ToLower() == "UBG18".ToLower()).FirstOrDefault();
            return View(new TranslationControllerModel(translation, books: books));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
