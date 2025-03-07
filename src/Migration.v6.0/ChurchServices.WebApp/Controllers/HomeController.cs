/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Controllers {
    public class HomeController : ChurchControllerBase {
        private readonly ILogger<HomeController> Logger;
        private readonly IBibleTagController BibleTag;
        private readonly ITranslationInfoController TranslationInfoController;
        public HomeController(IBibleTagController bibleTag, ILogger<HomeController> logger, ITranslationInfoController translationInfoController, IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment) {
            Logger = logger;
            TranslationInfoController = translationInfoController;
            BibleTag = bibleTag;
        }

        public IActionResult Index() {
            var model = new HomePageModel() {
                Articles = TranslationInfoController.GetLastFourArticles(),
                Info = GetHomeFileData()
            };
            return View(model);
        }
        private HtmlFileData GetHomeFileData() => GetFileData("download\\home.html", "Nie udało się wczytać wyznania wiary.");

        public IActionResult WhatWeBelieve() => View(GetConfessionFileData());
        private HtmlFileData GetConfessionFileData() => GetFileData("download\\confession.html", "Nie udało się wczytać wyznania wiary.");

        public IActionResult About() => View(GetAboutFileData());
        private HtmlFileData GetAboutFileData() => GetFileData("download\\about.html", "Nie udało się pliku informacyjnego.");

        public IActionResult Service() => View();

        public IActionResult Privacy() => View();

        public IActionResult Course() => View();

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

        public IActionResult SiglumUrlPrepare() => View();

        [HttpPost]
        public IActionResult SiglumUrlPrepare(ScriptureModel model) {
            if (model !=null&& model.Siglum.IsNotNullOrEmpty()) {
                using (var controller = new ScriptureController(BibleTag, TranslationInfoController)) {
                    var m = controller.GetUrl(model.Siglum);
                    return View(m.Value);
                }
            }
            return View();
        }
    }
}
