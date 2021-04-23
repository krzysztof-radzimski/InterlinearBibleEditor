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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Church.WebApp.Controllers {
     public class ArticlesController : Controller {
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(ILogger<ArticlesController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            var articles = new XPQuery<Article>(new UnitOfWork()).OrderByDescending(x => x.Date);
            var leaded = 4;
            var list = new List<ArticleInfoBase>();

            foreach (var item in articles) {
                if (leaded > 0) {
                    leaded--;
                    list.Add(new ArticleInfo() {
                        AuthorName = item.AuthorName,
                        AuthorPicture = item.AuthorPicture.IsNotNull() ? Convert.ToBase64String(item.AuthorPicture) : String.Empty,
                        Date = item.Date,
                        Id = item.Oid,
                        Lead = item.Lead,
                        Subject = item.Subject,
                        Type = item.Type.GetDescription()
                    });
                }
                else {
                    list.Add(new ArticleInfoBase() {
                        Id = item.Oid,
                        Subject = item.Subject,
                        AuthorName = item.AuthorName,
                        Date = item.Date
                    });
                }
            }

            return View(list);
        }   
    }

    public class ArticleInfoBase {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string AuthorName { get; set; }
        public DateTime Date { get; set; }
    }

    public class ArticleInfo : ArticleInfoBase {

        public string Lead { get; set; }
        public string AuthorPicture { get; set; }
        public string Type { get; set; }
    }
}
