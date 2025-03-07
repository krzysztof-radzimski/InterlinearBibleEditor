/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class SongDownloadController : Controller {
        protected readonly IConfiguration Configuration;
        protected ExportSaveFormat Format { get; }

        public SongDownloadController(IConfiguration configuration) {
            Configuration = configuration;
        }

        [HttpGet]
        [Route("/api/[controller]/{number}/{format}")]
        public async Task<IActionResult> Get(int number, ExportSaveFormat format) {
            DownloadStreamResult result;
            try {
                result = await CreateStream(number, format);
            }
            catch (AuthException) {
                return new RedirectResult("/Account/Index?ReturnUrl=" + Request.Path + HttpUtility.UrlDecode(Request.QueryString.Value));
            }
            if (result.IsNull()) { return NotFound(); }
            return File(result.Stream, Format.GetDescription(), result.FileName);
        }

        private async Task<DownloadStreamResult> CreateStream(int number, ExportSaveFormat format) {
            var fileName = String.Empty;
            var song = new XPQuery<Song>(new UnitOfWork()).Where(x => x.Number == number).FirstOrDefault();
            if (song.IsNotNull()) {
                fileName = $"{song.Number}-{song.Name.Replace(" ", "-").RemovePolishChars()}.{format.ToString().ToLower()}";
                byte[] licData = await GetLicData();
                var host = (this.Request.IsHttps ? "https://" : "http://") + this.Request.Host;
                var exporter = new SongExporter(licData, host);
                return new DownloadStreamResult { Stream = new MemoryStream(exporter.Export(song, format)), FileName = fileName };
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

    [ApiController]
    [Route("api/[controller]")]
    public class SongsDownloadController : Controller {
        protected readonly IConfiguration Configuration;
        protected ExportSaveFormat Format { get; }

        public SongsDownloadController(IConfiguration configuration) {
            Configuration = configuration;
        }

        [HttpGet]
        [Route("/api/[controller]/{format}")]
        public async Task<IActionResult> Get(ExportSaveFormat format) {
            DownloadStreamResult result;
            try {
                result = await CreateStream(format);
            }
            catch (AuthException) {
                return new RedirectResult("/Account/Index?ReturnUrl=" + Request.Path + HttpUtility.UrlDecode(Request.QueryString.Value));
            }
            if (result.IsNull()) { return NotFound(); }
            return File(result.Stream, Format.GetDescription(), result.FileName);
        }

        private async Task<DownloadStreamResult> CreateStream(ExportSaveFormat format) {
            var songs = new XPQuery<Song>(new UnitOfWork()).OrderBy(x => x.Number).ToList();
            var fileName = $"spiewnik-kchb-ndm.{format.ToString().ToLower()}";
            byte[] licData = await GetLicData();
            var host = (this.Request.IsHttps ? "https://" : "http://") + this.Request.Host;
            var exporter = new SongExporter(licData, host);
            return new DownloadStreamResult { Stream = new MemoryStream(exporter.ExportAll(songs, format)), FileName = fileName };
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
