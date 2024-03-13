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

    [ApiController]
    [Route("api/[controller]")]
    public class SongDownloadController : Controller {
        protected readonly IConfiguration Configuration;
        protected ExportSaveFormat Format { get; }

        public SongDownloadController(IConfiguration configuration) {
            Configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            var qs = Request.QueryString;

            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 5) {
                var queryString = HttpUtility.UrlDecode(qs.Value).RemoveAny("?q=");
                DownloadStreamResult result;
                try {
                    result= await CreateStream(queryString);
                }
                catch (AuthException) {
                    return new RedirectResult("/Account/Index?ReturnUrl=" + Request.Path + HttpUtility.UrlDecode(Request.QueryString.Value));
                }
                if (result.IsNull()) { return NotFound(); }
                return File(result.Stream, Format.GetDescription(), result.FileName);
            }

            return NotFound();
        }

        private async Task<DownloadStreamResult> CreateStream(string queryString) {
            var fileName = String.Empty;
            var paramsTable = queryString.Split(',');
            var song = new XPQuery<Song>(new UnitOfWork()).Where(x => x.Number == paramsTable[0].ToInt()).FirstOrDefault();
            if (song.IsNotNull()) {
                fileName = $"{song.Number}-{song.Name.Replace(" ", "-").RemovePolishChars()}.{paramsTable[1]}";
                ExportSaveFormat saveFormat = paramsTable[1] == "docx" ? ExportSaveFormat.Docx : ExportSaveFormat.Pdf;
                byte[] licData = await GetLicData();
                var host = (this.Request.IsHttps ? "https://" : "http://") + this.Request.Host;
                var exporter = new SongExporter(licData, host);
                return new DownloadStreamResult { Stream = new MemoryStream(exporter.Export(song, saveFormat)), FileName = fileName };
            }

            return default;
        }

        private async Task<byte[]> GetLicData() {
            var licPath = Configuration["AsposeLic"];
            var licInfo = new System.IO.FileInfo(licPath);

            if (licInfo.Exists) {
                return await System.IO.File.ReadAllBytesAsync(licPath);
            }
            return default;
        }
    }

    internal class DownloadStreamResult {
        public Stream Stream { get; set; }
        public string FileName { get; set; }
    }
}
