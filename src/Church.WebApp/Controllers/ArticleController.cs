/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Church.WebApp.Controllers {
    public class ArticleController : Controller {
        private readonly ILogger<ArticleController> _logger;

        public ArticleController(ILogger<ArticleController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            var qs = Request.QueryString;            
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 3) {
                var value = qs.Value;
                if (value.Contains("&")){
                    value = value.Substring(0, value.IndexOf("&"));
                }
                var id = value.ToLower().Replace("?id=", "").Trim().ToInt();
                var article = new XPQuery<Article>(new UnitOfWork()).Where(x => x.Oid == id).FirstOrDefault();
                if (article.IsNotNull()) {
                    return View(article);
                }
            }
            return View();
        }
    }
}
