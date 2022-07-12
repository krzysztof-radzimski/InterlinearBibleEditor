/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Controllers {
     public class ArticlesController : Controller {
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(ILogger<ArticlesController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            //var articles = new XPQuery<Article>(new UnitOfWork()).Where(x=>!x.Hidden).OrderByDescending(x => x.Date);
            var view = new XPView(new UnitOfWork(), typeof(Article)) {
                //TopReturnedRecords = 4,
                CriteriaString = "[Hidden] = 0"
            };
            view.Properties.Add(new ViewProperty("AuthorName", SortDirection.None, "[AuthorName]", false, true));
            view.Properties.Add(new ViewProperty("AuthorPicture", SortDirection.None, "[AuthorPicture]", false, true));
            view.Properties.Add(new ViewProperty("Date", SortDirection.Descending, "[Date]", false, true));
            view.Properties.Add(new ViewProperty("Oid", SortDirection.None, "[Oid]", false, true));
            view.Properties.Add(new ViewProperty("Lead", SortDirection.None, "[Lead]", false, true));
            view.Properties.Add(new ViewProperty("Subject", SortDirection.None, "[Subject]", false, true));
            view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));

            var leaded = 4;
            var list = new List<ArticleInfoBase>();

            foreach (ViewRecord item in view) {
                if (leaded > 0) {
                    leaded--;
                    list.Add(new ArticleInfo() {
                        AuthorName = item["AuthorName"].ToString(),
                        AuthorPicture = item["AuthorPicture"].IsNotNull() ? Convert.ToBase64String((byte[])item["AuthorPicture"]) : String.Empty,
                        Date = Convert.ToDateTime(item["Date"].ToString()),
                        Id = item["Oid"].ToInt(),
                        Lead = item["Lead"].ToString(),
                        Subject = item["Subject"].ToString(),
                        Type = ((ArticleType)item["Type"]).GetDescription()
                    });
                }
                else {
                    list.Add(new ArticleInfoBase() {
                        Id = item["Oid"].ToInt(),
                        Subject = item["Subject"].ToString(),
                        AuthorName = item["AuthorName"].ToString(),
                        Date = Convert.ToDateTime(item["Date"].ToString())
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
