/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Controllers {
    public abstract class DownloadInterlinearController : Controller {
        protected readonly IConfiguration Configuration;
        protected abstract ExportSaveFormat Format { get; }
        public DownloadInterlinearController(IConfiguration configuration) {
            Configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            var qs = Request.QueryString;

            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 5) {
                var queryString = HttpUtility.UrlDecode(qs.Value).RemoveAny("?q=");
                Stream stream;
                try {
                    stream = await CreateStream(queryString);
                }
                catch (AuthException) {
                    return new RedirectResult("/Account/Index?ReturnUrl=" + Request.Path + HttpUtility.UrlDecode(Request.QueryString.Value));
                }
                if (stream.IsNull()) { return NotFound(); }
                return File(stream, Format.GetDescription(), Format.GetCategory());
            }

            return NotFound();
        }

        private async Task<Stream> CreateStream(string queryString) {
            Book book;
            var paramsTable = queryString.Split(',');
            if (paramsTable.Length == 3) {
                Chapter chapter;

                var translationName = paramsTable[0];
                var bookNumber = paramsTable[1].ToInt();
                var chapterNumber = paramsTable[2].ToInt();

                var uow = new UnitOfWork();

                var trans = new XPQuery<Translation>(uow).Where(x => !x.Hidden && x.Name == translationName).FirstOrDefault();
                if (trans.IsNull() && translationName.EndsWith(" ")) {
                    translationName = translationName.Trim() + "+";
                    trans = new XPQuery<Translation>(uow).Where(x => x.Name == translationName).FirstOrDefault();
                }
                if (trans.IsNull()) { return default; }
                if (!trans.OpenAccess && !User.Identity.IsAuthenticated) {
                    throw new AuthException();
                }

                book = trans.Books.Where(x => x.NumberOfBook == bookNumber).FirstOrDefault();
                chapter = book.Chapters.Where(x => x.NumberOfChapter == chapterNumber).FirstOrDefault();

                byte[] licData = await GetLicData();
                var host = (this.Request.IsHttps ? "https://" : "http://") + this.Request.Host;
                var result = new InterlinearExporter(licData, host).Export(chapter, Format);

                return new MemoryStream(result);
            }
            else if (paramsTable.Length == 2) {
                var translationName = paramsTable[0];
                var bookNumber = paramsTable[1].ToInt();

                var uow = new UnitOfWork();

                var trans = new XPQuery<Translation>(uow).Where(x => !x.Hidden && x.Name == translationName).FirstOrDefault();
                if (trans.IsNull() && translationName.EndsWith(" ")) {
                    translationName = translationName.Trim() + "+";
                    trans = new XPQuery<Translation>(uow).Where(x => x.Name == translationName).FirstOrDefault();
                }
                if (trans.IsNull()) { return default; }
                if (!trans.OpenAccess && !User.Identity.IsAuthenticated) {
                    throw new AuthException();
                }

                book = trans.Books.Where(x => x.NumberOfBook == bookNumber).FirstOrDefault();

                byte[] licData = await GetLicData();
                var host = (this.Request.IsHttps ? "https://" : "http://") + this.Request.Host;
                var result = new InterlinearExporter(licData, host).Export(book, Format);

                return new MemoryStream(result);
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
    public class DownloadInterlinearDocxController : DownloadInterlinearController {
        protected override ExportSaveFormat Format => ExportSaveFormat.Docx;
        public DownloadInterlinearDocxController(IConfiguration configuration) : base(configuration) { }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class DownloadInterlinearPdfController : DownloadInterlinearController {
        protected override ExportSaveFormat Format => ExportSaveFormat.Pdf;
        public DownloadInterlinearPdfController(IConfiguration configuration) : base(configuration) { }
    }
}