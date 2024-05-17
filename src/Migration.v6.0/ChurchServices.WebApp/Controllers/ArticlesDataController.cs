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
        public ArticlesDataController(IConfiguration configuration) : base(configuration) { }
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
            view.Properties.Add(new ViewProperty("Passage", SortDirection.None, "[Passage]", false, true));
            view.Properties.Add(new ViewProperty("Id", SortDirection.None, "[Oid]", false, true));

            var list = new List<ArticleDataItem>();

            foreach (ViewRecord item in view) {
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(item["Text"].ToString());
                var root = doc.DocumentNode;
                var sb2 = new StringBuilder();
                var nn = 1;
                var paragraphs = new List<ArticleParagraphDataItem>();
                foreach (var node in root.DescendantsAndSelf()) {
                    if (node.NodeType == HtmlAgilityPack.HtmlNodeType.Element) {
                        if (node.Name == "ol" || node.Name == "ul") {
                            nn = 1;
                        }
                        if (node.Name == "li") {
                            sb2.Append($" ({nn}) ");
                            nn++;
                        }
                        if (node.Name == "p" || node.Name == "ol" || node.Name == "ul" || node.Name == "h1" || node.Name == "h2") {
                            var last = paragraphs.LastOrDefault();
                            if (last != null) {
                                last.Text = sb2.ToString();
                                sb2.Clear();
                            }

                            var type = node.Name;
                            if (node.HasClass("quote")) {
                                type = "Quotation";
                            }
                            else if (node.Name == "p") {
                                type = "Paragraph";
                            }
                            else if (node.Name == "ol" || node.Name == "ul") {
                                type = "List";
                            }
                            else if (node.Name == "h1") {
                                type = "Heading 1";
                            }
                            else if (node.Name == "h2") {
                                type = "Heading 2";
                            }
                            paragraphs.Add(new ArticleParagraphDataItem() { Type = type, Text = "" });
                        }
                    }

                    if (!node.HasChildNodes) {
                        if (node.ParentNode != null && node.ParentNode.Name == "a" && node.ParentNode.Id.StartsWith("ref-fn")) { continue; }
                        string text = node.InnerText;
                        if (!string.IsNullOrEmpty(text))
                            text = text.Replace("&nbsp;", " ");
                        sb2.Append($"{text}");
                    }
                }

                list.Add(new ArticleDataItem() {
                    Subject = item["Subject"].ToString(),
                    Passage = item["Passage"]?.ToString(),
                    Url = $"{Configuration["HostUrl"]}/article/{item["Id"]}",

                    AuthorName = item["AuthorName"].ToString(),
                    Date = Convert.ToDateTime(item["Date"].ToString()),
                    Lead = item["Lead"].ToString(),
                    Type = ((ArticleType)item["Type"]).ToString(),

                    Paragraphs = paragraphs
                });
            }

            return list;
        }
    }

    public class ArticleDataItem {
        public string Subject { get; set; }
        public string Passage { get; set; }
        public string Url { get; set; }
        public string AuthorName { get; set; }
        public DateTime Date { get; set; }
        public string Lead { get; set; }
        public string Type { get; set; }
        public List<ArticleParagraphDataItem> Paragraphs { get; set; }
    }

    public class ArticleParagraphDataItem {
        public string Type { get; set; }
        public string Text { get; set; }
    }
}
