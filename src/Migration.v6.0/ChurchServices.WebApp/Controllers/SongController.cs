/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Controllers {
    public class SongController : Controller {
        private readonly ITranslationInfoController TranslationInfoController;
        public SongController(ITranslationInfoController translationInfoController) {
            TranslationInfoController = translationInfoController;
        }

        [Route("/[controller]/{number}")]
        public IActionResult Index(int number) {
            var song = new XPQuery<Song>(new UnitOfWork()).Where(x => x.Number == number).FirstOrDefault();
            if (song.IsNotNull()) {
                var maxNumber = TranslationInfoController.GetSongs().Select(x => x.Number).Max();
                var result = new SongControllerModel() { Song = song, MaxNumber = maxNumber };
                return View(result);
            }
            return View();
        }

        public IActionResult Index() {
            var qs = Request.QueryString;
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 3) {
                var value = qs.Value;
                if (value.Contains("&")) {
                    value = value.Substring(0, value.IndexOf("&"));
                }
                var id = value.ToLower().Replace("?id=", "").Trim().ToInt();
                var song = new XPQuery<Song>(new UnitOfWork()).Where(x => x.Number == id).FirstOrDefault();
                if (song.IsNotNull()) {
                    var maxNumber = TranslationInfoController.GetSongs().Select(x => x.Number).Max();
                    var result = new SongControllerModel() { Song = song, MaxNumber = maxNumber };
                    return View(result);
                }
            }
            return View();
        }
    }

    public class SongsController : Controller {
        private readonly ITranslationInfoController TranslationInfoController;
        public SongsController(ITranslationInfoController translationInfoController) {
            TranslationInfoController = translationInfoController;
        }
        public IActionResult Index() => View(TranslationInfoController.GetSongs());
    }
}
