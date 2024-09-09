/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Controllers {
    public class TranslationController : Controller {
        private readonly IConfiguration Configuration;
        private readonly ITranslationInfoController TranslationInfoController;
        public TranslationController(IConfiguration configuration, ITranslationInfoController translationInfoController) {
            Configuration = configuration;
            TranslationInfoController = translationInfoController;
        }

        [TranslationAuthorize]
        public IActionResult Index(string translationName, string book = null, string chapter = null, string verse = null) {
            if (translationName == "SNPPD" || translationName == "SNPD") { translationName = "PBD"; }
            if (translationName == "SNPL") { translationName = "SNP18"; }
            if (translationName == "PNS") { translationName = "PNS1997"; }

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

            return View();
        }
    }
}
