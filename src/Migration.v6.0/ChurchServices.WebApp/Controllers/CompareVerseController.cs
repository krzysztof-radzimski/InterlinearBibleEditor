/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Controllers {
    public class CompareVerseController : Controller {
        protected readonly IBibleTagController BibleTag;
        private readonly ITranslationInfoController TranslationInfoController;
        public CompareVerseController(IBibleTagController bibleTagController, ITranslationInfoController translationInfoController) {
            BibleTag = bibleTagController;
            TranslationInfoController = translationInfoController;
        }
        [Route("/[controller]/{trans}/{book}/{chapter}/{verse}")]
        public IActionResult Index(string trans, int book, int chapter, int verse) {
            return View(GetModel(new VerseIndex(trans, book, chapter, verse)));
        }
        [Route("/[controller]/{trans}/{book}/{chapter}/{verse}/{literal}")]
        public IActionResult Index(string trans, int book, int chapter, int verse, bool literal) {
            return View(GetModel(new VerseIndex(trans, book, chapter, verse), literal));
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

                return GetModel(vi, literalOnly);

            }
            return null;
        }

        internal CompareVerseModel GetModel(VerseIndex vi, bool literalOnly = false) {
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
            viewTranslation.Properties.Add(new ViewProperty("Language", SortDirection.Descending, "[Language]", false, true));

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
                    SortIndex = ((TranslationType)item["Type"]).GetCategory().ToInt(),
                    Language = (Language)item["Language"]
                };
                verses.Add(cvi);
            }

            var _view = new XPView(uow, typeof(Verse)) {
                Criteria = new DevExpress.Data.Filtering.InOperator("Index", indexes)
            };

            _view.Properties.Add(new ViewProperty("Index", SortDirection.None, "[Index]", false, true));
            _view.Properties.Add(new ViewProperty("Text", SortDirection.Descending, "[Text]", false, true));

            var translateModel = GetTranslationControllerModel(uow);

            foreach (ViewRecord item in _view) {
                var idx = item["Index"].ToString();
                var text = item["Text"].ToString();
                var text2 = item["Text"].ToString();
                var cvi = verses.Where(x => x.Index.Index == idx).FirstOrDefault();
                if (cvi.IsNotNull()) {
                    cvi.Text = text;

                    if (text2.Contains("<n>") && text2.Contains("<x>")) {
                        text2 = BibleTag.GetInternalVerseRangeText(text2, translateModel);
                        text2 = BibleTag.GetInternalVerseText(text2, translateModel);
                        text2 = BibleTag.GetExternalVerseRangeText(text2, translateModel);
                        text2 = BibleTag.GetExternalVerseText(text2, translateModel);
                        text2 = BibleTag.GetInternalVerseListText(text2, translateModel);
                        text2 = BibleTag.GetMultiChapterRangeText(text2, translateModel);

                        text2 = text2.Replace("**********", "<sup>10)</sup>");
                        text2 = text2.Replace("*********", "<sup>9)</sup>");
                        text2 = text2.Replace("********", "<sup>8)</sup>");
                        text2 = text2.Replace("*******", "<sup>7)</sup>");
                        text2 = text2.Replace("******", "<sup>6)</sup>");
                        text2 = text2.Replace("*****", "<sup>5)</sup>");
                        text2 = text2.Replace("****", "<sup>4)</sup>");
                        text2 = text2.Replace("***", "<sup>3)</sup>");
                        text2 = text2.Replace("**", "<sup>2)</sup>");
                        text2 = text2.Replace("*", "<sup>1)</sup>");
                    }

                    cvi.HtmlText = text2
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

        public TranslationControllerModel GetTranslationControllerModel(UnitOfWork uow = null) {
            if (uow == null) { uow = new UnitOfWork(); }
            return new TranslationControllerModel(new XPQuery<Translation>(uow).Where(x => x.Name == "BW").FirstOrDefault(), books: GetBookBases(uow));
        }

        protected List<BookBaseInfo> GetBookBases(UnitOfWork uow = null) {
            var result = new List<BookBaseInfo>();
            if (uow == null) { uow = new UnitOfWork(); }
            var books = new XPQuery<BookBase>(uow).ToList();
            foreach (var item in books) {
                result.Add(new BookBaseInfo() {
                    BookName = item.BookName,
                    BookShortcut = item.BookShortcut,
                    BookTitle = item.BookTitle,
                    Color = item.Color,
                    NumberOfBook = item.NumberOfBook,
                    StatusBiblePart = item.StatusBiblePart,
                    StatusBookType = item.StatusBookType,
                    StatusCanonType = item.StatusCanonType
                });
            }
            return result;
        }
    }
}
