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
            var articles = new XPQuery<Article>(new UnitOfWork()).Where(x=>!x.Hidden).OrderByDescending(x => x.Date);
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

        public string GetDaysAgo() {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - Date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (DateTime.UtcNow.Ticks < Date.Ticks) {
                return "nadchodzące";
            }

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "sekundę temu" : ts.Seconds + " sekund temu";

            if (delta < 2 * MINUTE)
                return "minutę temu";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minut temu";

            if (delta < 90 * MINUTE)
                return "godzinę temu";

            if (delta < 24 * HOUR)
                return ts.Hours + " godzin temu";

            if (delta < 48 * HOUR)
                return "wczoraj";

            if (delta < 30 * DAY)
                return ts.Days + " dni temu";

            if (delta < 12 * MONTH) {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                switch (months) {
                    case 0:
                    case 1: { return "miesiąc temu"; }
                    case 2:
                    case 3:
                    case 4: { return months+ " miesiące temu"; }
                    default: { return months + " miesięcy temu"; }
                }
            }
            else {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "rok temu" : years + " lat temu";
            }
        }

    }

    public class ArticleInfo : ArticleInfoBase {
        public string Lead { get; set; }
        public string AuthorPicture { get; set; }
        public string Type { get; set; }
    }
}
