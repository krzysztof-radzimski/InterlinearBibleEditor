/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using System.Text;

namespace ChurchServices.WebApp.Controllers {
    [ApiController]
    [Route("/api/[controller]")]
    public class ArticlesDataController : JsonControllerBase {
        public IActionResult Get() {
            var view = new XPView(new UnitOfWork(), typeof(Article)) {
                CriteriaString = "[Hidden] = 0"
            };
            var list = GetArticlesListByView(view);
            return FileJson(list, "Kazania.json");
        }

        [Route("/api/[controller]/{AuthorName}")]
        public IActionResult GetByAuthorName(string AuthorName) {
            if (AuthorName.ToInt() > 0) {
                AuthorName = AuthorName.Replace("+", " ").Replace("-", " ");
                var view = new XPView(new UnitOfWork(), typeof(Article)) {
                    CriteriaString = $"[Hidden] = 0 AND [Oid] = '{AuthorName}'"
                };
                var list = GetArticlesListByView(view);
                return FileJson(list, $"Kazanie-{AuthorName}.json");
            }
            else {
                AuthorName = AuthorName.Replace("+", " ").Replace("-", " ");
                var view = new XPView(new UnitOfWork(), typeof(Article)) {
                    CriteriaString = $"[Hidden] = 0 AND [AuthorName] = '{AuthorName}'"
                };
                var list = GetArticlesListByView(view);
                return FileJson(list, "Kazania.json");
            }
        }

        private List<ArticleDataItem> GetArticlesListByView(XPView view) {
            view.Properties.Add(new ViewProperty("AuthorName", SortDirection.None, "[AuthorName]", false, true));
            view.Properties.Add(new ViewProperty("Date", SortDirection.Descending, "[Date]", false, true));
            view.Properties.Add(new ViewProperty("Lead", SortDirection.None, "[Lead]", false, true));
            view.Properties.Add(new ViewProperty("Subject", SortDirection.None, "[Subject]", false, true));
            view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
            view.Properties.Add(new ViewProperty("Text", SortDirection.None, "[Text]", false, true));

            var list = new List<ArticleDataItem>();

            foreach (ViewRecord item in view) {
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(item["Text"].ToString());
                var root = doc.DocumentNode;
                var sb = new StringBuilder();
                foreach (var node in root.DescendantsAndSelf()) {
                    if (!node.HasChildNodes) {
                        string text = node.InnerText;
                        if (!string.IsNullOrEmpty(text))
                            text = text.Replace("&nbsp;", " ");
                        sb.Append($"{text} ");
                    }
                }

                list.Add(new ArticleDataItem() {
                    AuthorName = item["AuthorName"].ToString(),
                    Text = sb.ToString().Trim(),
                    Date = Convert.ToDateTime(item["Date"].ToString()),
                    Lead = item["Lead"].ToString(),
                    Subject = item["Subject"].ToString(),
                    Type = ((ArticleType)item["Type"]).GetDescription()
                });
            }

            return list;
        }
    }

    public class ArticleDataItem {
        public string Subject { get; set; }
        public string AuthorName { get; set; }
        public DateTime Date { get; set; }
        public string Lead { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
    }
}
