/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using Church.WebApp.Models;
using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Church.WebApp.Controllers {
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
                var article = new XPQuery<Article>(new UnitOfWork()).Where(x => x.Oid == id).FirstOrDefault();
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
}
