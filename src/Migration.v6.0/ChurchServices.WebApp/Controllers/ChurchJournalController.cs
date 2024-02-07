/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Controllers {
    [Route("/informator")]
    [Route("/churchjournal")]
    public class ChurchJournalController : Controller {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        public ChurchJournalController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment) {
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index() => View(GetJournalFilePath());
        private ChurchJournalData GetJournalFilePath() {
            var filePath = _hostingEnvironment.WebRootPath + "/download/journal/journal.html";
            if (System.IO.File.Exists(filePath)) {
                var data = System.IO.File.ReadAllText(filePath);
                return new ChurchJournalData() { Data = data };
            }
            return new ChurchJournalData() { Data = "<p>Nie udało się wczytać listy informatorów zborowych.</p>" };
        }
    }
    public class ChurchJournalData { public string Data { get; set; } }
}
