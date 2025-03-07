/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Controllers {
    public class LeadersMaterialsController : Controller {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LeadersMaterialsController(IWebHostEnvironment webHostEnvironment) {
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize]
        public IActionResult Index() => View(GetFileData());

        private HtmlFileData GetFileData() {
            var dataFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "download\\leaders.html");
            if (System.IO.File.Exists(dataFilePath)) {
                var data = System.IO.File.ReadAllText(dataFilePath);
                return new HtmlFileData() { Data = data };
            }
            return new HtmlFileData() { Data = "<p>Nie udało się wczytać listy materiałów.</p>" };
        }
    }
}
