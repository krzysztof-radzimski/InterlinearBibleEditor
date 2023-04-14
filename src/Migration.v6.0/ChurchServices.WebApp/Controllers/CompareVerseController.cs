/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using System.Xml.Linq;

namespace ChurchServices.WebApp.Controllers {
    public class CompareVerseController : Controller {
        protected readonly IBibleTagController BibleTag;
        private readonly ITranslationInfoController TranslationInfoController;
        public CompareVerseController(IBibleTagController bibleTagController, ITranslationInfoController translationInfoController) {
            BibleTag = bibleTagController;
            TranslationInfoController = translationInfoController;
        }
        public IActionResult Index() {
            var qs = Request.QueryString;
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 3) {
                return View(GetModel(qs));
            }
            return View();
        }

        internal CompareVerseModel GetModel(QueryString qs) {
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 3) {
                var value = qs.Value;
                var literalOnly = value.Contains("literal");
                if (value.Contains("&")) {
                    value = value.Substring(0, value.IndexOf("&"));
                }
                var id = value.Replace("?id=", "").Trim();
                var vi = new VerseIndex(id);
                var uow = new UnitOfWork();

                var baseBookShortcut = "";
                var baseBookName = "";
                var biblePart = BiblePart.None;
                var baseBook = TranslationInfoController.GetBookBases(uow).Where(x => x.NumberOfBook == vi.NumberOfBook).FirstOrDefault();
                if (baseBook.IsNotNull()) {
                    baseBookShortcut = baseBook.BookShortcut;
                    baseBookName = baseBook.BookName;
                    biblePart = baseBook.StatusBiblePart;
                }

                var verses = new List<CompareVerseInfo>();
                var result = new CompareVerseModel() {
                    Index = vi,
                    Verses = new List<CompareVerseInfo>(),
                    BookName = baseBookName,
                    BookShortcut = baseBookShortcut,
                    Part = biblePart,
                    LiteralOnly = literalOnly
                };

                var viewChapter = new XPView(uow, typeof(Chapter)) {
                    CriteriaString = $"[Index] = '{vi.TranslationName}.{vi.NumberOfBook}.{vi.NumberOfChapter}'"
                };
                viewChapter.Properties.Add(new ViewProperty("NumberOfVerses", SortDirection.None, "[NumberOfVerses]", false, true));
                foreach (ViewRecord item in viewChapter) {
                    result.LastVerseNumberOfChapter = item["NumberOfVerses"].ToInt();
                    break;
                }

                var criteriaString = "[Hidden] = 0";
                if (literalOnly) {
                    criteriaString += " AND (([Type] = 1) OR ([Type] = 4))";
                }

                var viewTranslation = new XPView(uow, typeof(Translation)) {
                    CriteriaString = criteriaString
                };
                viewTranslation.Properties.Add(new ViewProperty("Oid", SortDirection.None, "[Oid]", false, true));
                viewTranslation.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
                viewTranslation.Properties.Add(new ViewProperty("Description", SortDirection.None, "[Description]", false, true));
                viewTranslation.Properties.Add(new ViewProperty("Type", SortDirection.Descending, "[Type]", false, true));

                var indexes = new List<string>();
                foreach (ViewRecord item in viewTranslation) {
                    var name = item["Name"].ToString();
                    var _index = $"{name.Replace("'", "").Replace("+", "")}.{vi.NumberOfBook}.{vi.NumberOfChapter}.{vi.NumberOfVerse}";
                    indexes.Add(_index);

                    var cvi = new CompareVerseInfo() {
                        Index = new VerseIndex(_index),
                        TranslationName = item["Name"].ToString(),
                        TranslationDescription = item["Description"].ToString(),
                        TranslationType = (TranslationType)item["Type"],
                        SortIndex = ((TranslationType)item["Type"]).GetCategory().ToInt()
                    };
                    verses.Add(cvi);
                }

                var _view = new XPView(uow, typeof(Verse)) {
                    Criteria = new DevExpress.Data.Filtering.InOperator("Index", indexes)
                };

                _view.Properties.Add(new ViewProperty("Index", SortDirection.None, "[Index]", false, true));
                _view.Properties.Add(new ViewProperty("Text", SortDirection.Descending, "[Text]", false, true));

                foreach (ViewRecord item in _view) {
                    var idx = item["Index"].ToString();
                    var text = item["Text"].ToString();
                    var cvi = verses.Where(x => x.Index.Index == idx).FirstOrDefault();
                    if (cvi.IsNotNull()) {
                        cvi.Text = text;
                        cvi.HtmlText = text
                                .Replace("―", String.Empty)
                                .Replace("<n>", @"<span class=""text-muted"">")
                                .Replace("</n>", "</span>")
                                .Replace("<J>", @"<span class=""text-danger"">")
                                .Replace("</J>", "</span>");
                        cvi.SimpleText = BibleTag.GetVerseSimpleText(text, cvi.Index, baseBookShortcut);
                    }
                }

                result.Verses = verses.Where(x => x.Text.IsNotNullOrEmpty()).OrderBy(x => x.SortIndex).ToList();

                return result;
            }
            return null;
        }
    }
}
