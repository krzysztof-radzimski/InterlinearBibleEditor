/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using ChurchServices.Data.Export;
using ChurchServices.Data.Model;
using Microsoft.SqlServer.Server;

namespace ChurchServices.WebApp.Controllers {
    public class ArticleController : Controller {
        protected readonly IConfiguration Configuration;
        public ArticleController(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IActionResult Index() {
            var qs = Request.QueryString;
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 3) {
                var value = qs.Value;
                if (value.Contains("&")) {
                    value = value.Substring(0, value.IndexOf("&"));
                }
                var id = value.ToLower().Replace("?id=", "").Trim().ToInt();
                var article = new XPQuery<Article>(new UnitOfWork()).Where(x => x.Oid == id && !x.Hidden).FirstOrDefault();
                if (article.IsNotNull()) {
                    return View(new ArticleControllerModel() { Article = article });
                }
            }
            return View();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ArticleImageController : Controller {
        private readonly IConfiguration Configuration;
        public ArticleImageController(IConfiguration configuration) {
            Configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Get() {
            var qs = Request.QueryString;

            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty()) {
                var value = qs.Value;
                if (value.Contains("&")) {
                    value = value.Substring(0, value.IndexOf("&"));
                }
                var queryString = Uri.UnescapeDataString(value).RemoveAny("?id=");
                var stream = await GetImage(queryString);
                if (stream.IsNull()) { return NotFound(); }
                return File(stream, "image/jpg", "article.jpg");
            }

            return NotFound();
        }
        private async Task<Stream> GetImage(string queryString) {
            var paramsTable = queryString.Split(',');
            if (paramsTable.Length > 0) {
                var articleId = paramsTable[0].ToInt();
                if (articleId > 0) {
                    var uow = new UnitOfWork();
                    var article = new XPQuery<Article>(uow).Where(x => x.Oid == articleId).FirstOrDefault();
                    if (article.IsNotNull()) {
                        return new MemoryStream(article.AuthorPicture);
                    }
                }
            }
            return default;
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ArticleDownloadController : Controller {
        protected readonly IConfiguration Configuration;
        protected ExportSaveFormat Format { get; }
        public ArticleDownloadController(IConfiguration configuration) {
            Configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Get() {
            var qs = Request.QueryString;

            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 5) {
                var queryString = HttpUtility.UrlDecode(qs.Value).RemoveAny("?q=");
                DownloadStreamResult result;
                try {
                    result = await CreateStream(queryString);
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
            var article = new XPQuery<Article>(new UnitOfWork()).Where(x=> x.Oid == paramsTable[0].ToInt()).FirstOrDefault();
            if (article != null) {
                fileName = $"{article.Subject.Replace(" ", "-").RemovePolishChars()}.{paramsTable[1]}";
                ExportSaveFormat saveFormat = paramsTable[1] == "docx" ? ExportSaveFormat.Docx : ExportSaveFormat.Pdf;
                byte[] licData = await GetLicData();
                var host = (this.Request.IsHttps ? "https://" : "http://") + this.Request.Host;
                var exporter = new ArticleExporter(licData, host);
                return new DownloadStreamResult { Stream = new MemoryStream(exporter.Export(article, saveFormat)), FileName = fileName };
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
}
