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
    public class ChurchJournalController : ChurchControllerBase {
        public ChurchJournalController(IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment) { }
        public IActionResult Index() => View(GetJournalFileData());
        [Route("/legalacts")]
        public IActionResult LegalActs() => View(GetLegalActsFileData());
        private HtmlFileData GetJournalFileData() => GetFileData("download\\journal\\journal.html", "Nie udało się wczytać listy informatorów zborowych.");
        private HtmlFileData GetLegalActsFileData() => GetFileData("download\\legalacts\\index.html", "Nie udało się wczytać listy aktów prawnych.");
    }
}
