/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo.DB;

namespace ChurchServices.WebApp.Controllers {
    public class ArticleController : Controller {
        protected readonly IConfiguration Configuration;
        public ArticleController(IConfiguration configuration) {
            Configuration = configuration;
        }

        [Route("/[controller]/{id}")]
        public IActionResult Index(int id) {
            var article = new XPQuery<Article>(new UnitOfWork()).Where(x => x.Oid == id && !x.Hidden).FirstOrDefault();
            if (article.IsNotNull()) {
                return View(new ArticleControllerModel() {
                    Article = new ArticleViewInfo() {
                        AuthorName = article.AuthorName,
                        Passage = article.Passage,
                        AuthorPicture = article.AuthorPicture.IsNotNull() ? Convert.ToBase64String(article.AuthorPicture) : String.Empty,
                        Date = article.Date,
                        Id = article.Oid,
                        Lead = article.Lead,
                        Subject = article.Subject,
                        Text = article.Text,
                        Type = article.Type.GetDescription()
                    }
                });
            }
            else {
                var dl = new SimpleDataLayer(new InMemoryDataStore());
                var uow = new UnitOfWork(dl);
                return View(new ArticleControllerModel() {
                    Article = new ArticleViewInfo() {
                        AuthorName = "Brak artykułu",
                        AuthorPicture = null,
                        Date = DateTime.MinValue,
                        Lead = $"Nie znaleziono artykułu o numerze {id}!",
                        Text = String.Empty,
                        Type = ArticleType.Article.GetDescription(),
                        Passage = String.Empty,
                        Subject = String.Empty, 
                        Id = id
                    }
                });
            }
        }
        public IActionResult Index() {
            var id = 0;
            var qs = Request.QueryString;
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 3) {
                var value = qs.Value;
                if (value.Contains("&")) {
                    value = value.Substring(0, value.IndexOf("&"));
                }
                id = value.ToLower().Replace("?id=", "").Trim().ToInt();
                if (id > 0) {
                    var article = new XPQuery<Article>(new UnitOfWork()).Where(x => x.Oid == id && !x.Hidden).FirstOrDefault();
                    if (article.IsNotNull()) {
                        return View(new ArticleControllerModel() {
                            Article = new ArticleViewInfo() {
                                AuthorName = article.AuthorName,
                                Passage = article.Passage,
                                AuthorPicture = article.AuthorPicture.IsNotNull() ? Convert.ToBase64String(article.AuthorPicture) : String.Empty,
                                Date = article.Date,
                                Id = article.Oid,
                                Lead = article.Lead,
                                Subject = article.Subject,
                                Text = article.Text,
                                Type = article.Type.GetDescription()
                            }
                        });
                    }
                }
            }
            return View(new ArticleControllerModel() {
                Article = new ArticleViewInfo() {
                    AuthorName = "Brak artykułu",
                    AuthorPicture = null,
                    Date = DateTime.MinValue,
                    Lead = $"Nie znaleziono artykułu o numerze {id}!",
                    Text = String.Empty,
                    Type = ArticleType.Article.GetDescription(),
                    Passage = String.Empty,
                    Subject = String.Empty,
                    Id = id
                }
            });
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
        [Route("/api/[controller]/{id}")]
        public async Task<IActionResult> Get(int id) {
            var stream = await GetImage(id);
            if (stream.IsNull()) { return NotFound(); }
            return File(stream, "image/jpg", "article.jpg");
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

        private async Task<Stream> GetImage(int id) {
            if (id > 0) {
                var uow = new UnitOfWork();
                var article = new XPQuery<Article>(uow).Where(x => x.Oid == id).FirstOrDefault();
                if (article.IsNotNull()) {
                    return new MemoryStream(article.AuthorPicture);
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
        [Route("/api/[controller]/{id}/{format}")]
        public async Task<IActionResult> Get(int id, ExportSaveFormat format) {
            DownloadStreamResult result;
            try {
                result = await CreateStream(id, format);
            }
            catch (AuthException) {
                return new RedirectResult("/Account/Index?ReturnUrl=" + Request.Path + HttpUtility.UrlDecode(Request.QueryString.Value));
            }
            if (result.IsNull()) { return NotFound(); }
            return File(result.Stream, Format.GetDescription(), result.FileName);
        }

        private async Task<DownloadStreamResult> CreateStream(int id, ExportSaveFormat format) {
            var fileName = String.Empty;
            var article = new XPQuery<Article>(new UnitOfWork()).Where(x => x.Oid == id).FirstOrDefault();
            if (article != null) {
                fileName = $"{article.Subject.Replace(" ", "-").RemovePolishChars()}.{format.ToString().ToLower()}";
                byte[] licData = await GetLicData();
                var host = (this.Request.IsHttps ? "https://" : "http://") + this.Request.Host;
                var exporter = new ArticleExporter(licData, host);
                return new DownloadStreamResult { Stream = new MemoryStream(exporter.Export(article, format)), FileName = fileName };
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
