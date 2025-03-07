/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Controllers {
    public class SearchController : Controller {
        private const string TYPE_QUERY = "&type=";
        private readonly ILogger<SearchController> Logger;
        private readonly IBibleTagController BibleTag;
        private readonly ITranslationInfoController TranslationInfoController;
        public SearchController(ILogger<SearchController> logger, IBibleTagController bibleTag, ITranslationInfoController translationInfoController) {
            Logger = logger;
            BibleTag = bibleTag;
            TranslationInfoController = translationInfoController;
        }

        [Route("[controller]/{text}/{range}")]
        public IActionResult Index(string text, string range) {
            if (text.IsNotNullOrWhiteSpace()) {
                var type = SearchRangeType.All;
                if (range.IsNotNullOrWhiteSpace()) {
                    type = range.GetEnumByCategory<SearchRangeType>();
                }
                var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return _Search(words, type);
            }
            return View();
        }

        public IActionResult Index() {
            var qs = Request.QueryString;
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 9) {
                var queryString = Uri.UnescapeDataString(qs.Value);
                SearchRangeType type = SearchRangeType.All;
                if (queryString.Contains(TYPE_QUERY)) {
                    var t = queryString.Split('&', StringSplitOptions.RemoveEmptyEntries);
                    if (t.Length > 1) {
                        var t2 = t[1].Split('=', StringSplitOptions.RemoveEmptyEntries);
                        if (t2.Length > 1) {
                            type = t2[1].GetEnumByCategory<SearchRangeType>();
                        }
                    }
                    var endIndex = queryString.IndexOf(TYPE_QUERY);
                    if (endIndex != -1) {
                        queryString = queryString.Substring(0, endIndex);
                    }
                }
                var words = queryString.Replace("?text=", "").Split('+', StringSplitOptions.RemoveEmptyEntries);
                return _Search(words, type);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Index(string text) {
            if (text.IsNotNullOrEmpty() && text.Length > 3) {
                var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return _Search(words, SearchRangeType.All);
            }
            return View();
        }

        public IActionResult Siglum(string text, string type) {
            if (text.IsNotNullOrEmpty() && text.Length > 0) {
                var session = new UnitOfWork();

                // Uzyskanie nagłówków HTTP
                var headers = HttpContext.Request.Headers;
                // Uzyskanie referera (adres strony, z której przyszedł request)
                var referer = headers["Referer"].ToString();
                var host = headers["Host"].ToString();
                var routing = referer.IsNotNullOrEmpty() ? referer.Substring(referer.IndexOf(host) + host.Length + 1) : String.Empty;
                string translationName = null;

                if (routing.Contains("BibleByVerse")) {
                    var siglum = BibleTag.RecognizeSimpleSiglum(session, text);
                    if (siglum != null) {
                        return Redirect($"../BibleByVerse/{siglum.BookNumber}/{siglum.NumberOfChapter}/{siglum.NumberOfVerse}");
                    }
                }
                else if (routing.Contains("CompareVerse")) {
                    var siglum = BibleTag.RecognizeSimpleSiglum(session, text);
                    if (siglum != null) {
                        var s = routing.Substring(routing.IndexOf("/") + 1);
                        var t = s.Substring(0, s.IndexOf("/"));
                        return Redirect($"../CompareVerse/{t}/{siglum.BookNumber}/{siglum.NumberOfChapter}/{siglum.NumberOfVerse}");
                    }
                }
                else if (routing.StartWithAny(TranslationInfoController.GetTranslationNames("/"))) {
                    translationName = routing.IsNotNullOrEmpty() ? routing.Substring(0, routing.IndexOf("/")) : String.Empty;
                }


                var url = string.Empty;

                if (type.IsNotNullOrEmpty() && type == "compare") {
                    url = BibleTag.GetRecognizedCompareUrl(session, text);
                }

                if (url.IsNullOrEmpty()) {
                    url = BibleTag.GetRecognizedSiglumUrl(session, text, translationName);
                }

                if (url.IsNotNullOrEmpty()) {
                    return Redirect(url);
                }
            }
            return View(new string[] { text });
        }

        private IActionResult _Search(string[] words, SearchRangeType type) {
            var session = new UnitOfWork();
            var view = new XPView(session, typeof(Verse));
            var bookShortcuts = TranslationInfoController.GetBookBases(session).Select(x => new KeyValuePair<int, string>(x.NumberOfBook, x.BookShortcut)).ToList();
            var translationNames = new XPQuery<Translation>(session).Where(x => !x.Hidden).Select(x => new KeyValuePair<string, string>(x.Name.Replace("'", "").Replace("+", ""), x.Description)).ToList();

            var query = "Search?text=";
            foreach (var word in words) {
                query += word;
                if (word != words.Last()) {
                    query += "+";
                }
            }
            var dic = new Dictionary<string, string>();
            foreach (SearchRangeType item in Enum.GetValues(typeof(SearchRangeType))) {
                if (item == type) { continue; }
                if (item == SearchRangeType.All) {
                    dic.Add(item.GetDescription(), query);
                }
                else {
                    dic.Add(item.GetDescription(), query + TYPE_QUERY + item.GetCategory());
                }
            }

            var criteria = String.Empty;
            var _words = words.Distinct();
            if (_words.Count() == 0) {
                return View(new SearchResultsModel() { SearchType = type, Links = dic });
            }

            foreach (var word in _words) {

                criteria += $"Contains(Lower([Text]),'{word.ToLower()}')";

                if (word != _words.Last()) {
                    criteria += " AND ";
                }
            }

            switch (type) {
                case SearchRangeType.Psalms: { criteria += " AND NumberOfBook = 230"; break; }
                case SearchRangeType.PaulsLetters: {
                        criteria += GetRangeCriteriaString(520, 640);
                        break;
                    }
                case SearchRangeType.NewTestament: {
                        criteria += GetRangeCriteriaString(470, 730);
                        break;
                    }
                case SearchRangeType.Pentateuch: {
                        criteria += GetRangeCriteriaString(10, 50);
                        break;
                    }
                case SearchRangeType.OldTestament: {
                        criteria += GetRangeCriteriaString(10, 460);
                        break;
                    }
                case SearchRangeType.Gospel: {
                        criteria += GetRangeCriteriaString(470, 500);
                        break;
                    }
                case SearchRangeType.Historical: {
                        criteria += GetRangeCriteriaString(60, 190);
                        break;
                    }
                case SearchRangeType.Wisdom: {
                        criteria += GetRangeCriteriaString(220, 260);
                        break;
                    }
                case SearchRangeType.Prophets: {
                        criteria += GetRangeCriteriaString(290, 460);
                        break;
                    }
                case SearchRangeType.CatholicLetters: {
                        criteria += GetRangeCriteriaString(660, 720);
                        break;
                    }
            }


            view.CriteriaString = criteria.Trim();

            view.Properties.Add(new ViewProperty("NumberOfVerse", SortDirection.None, "[NumberOfVerse]", false, true));
            view.Properties.Add(new ViewProperty("VerseText", SortDirection.None, "[Text]", false, true));
            view.Properties.Add(new ViewProperty("Index", SortDirection.None, "[Index]", false, true));
            view.Properties.Add(new ViewProperty("NumberOfBook", SortDirection.None, "[NumberOfBook]", false, true));

            var model = new SearchResultsModel(words) { SearchType = type, Links = dic };
            TranslationControllerModel translateModel = null;
            foreach (ViewRecord record in view) {
                var _index = record["Index"];
                if (_index.IsNotNull()) {
                    var index = new VerseIndex(_index.ToString());
                    var baseBookShortcut = bookShortcuts.Where(x => x.Key == index.NumberOfBook).Select(x => x.Value).FirstOrDefault();
                    var translation = translationNames.Where(x => x.Key == index.TranslationName).FirstOrDefault();
                    if (translation.IsNull() || translation.Key.IsNull()) { continue; }
                    var translationDesc = translation.Value;
                    var verseText = record["VerseText"].ToString();

                    if (verseText.Contains("<n>") && verseText.Contains("<x>")) {
                        if (translateModel == null) { translateModel = GetTranslationControllerModel(session); }
                        verseText = BibleTag.GetInternalVerseRangeText(verseText, translateModel);
                        verseText = BibleTag.GetInternalVerseText(verseText, translateModel);
                        verseText = BibleTag.GetExternalVerseRangeText(verseText, translateModel);
                        verseText = BibleTag.GetExternalVerseText(verseText, translateModel);
                        verseText = BibleTag.GetInternalVerseListText(verseText, translateModel);
                        verseText = BibleTag.GetMultiChapterRangeText(verseText, translateModel);

                        verseText = verseText.Replace("**********", "<sup>10)</sup>");
                        verseText = verseText.Replace("*********", "<sup>9)</sup>");
                        verseText = verseText.Replace("********", "<sup>8)</sup>");
                        verseText = verseText.Replace("*******", "<sup>7)</sup>");
                        verseText = verseText.Replace("******", "<sup>6)</sup>");
                        verseText = verseText.Replace("*****", "<sup>5)</sup>");
                        verseText = verseText.Replace("****", "<sup>4)</sup>");
                        verseText = verseText.Replace("***", "<sup>3)</sup>");
                        verseText = verseText.Replace("**", "<sup>2)</sup>");
                        verseText = verseText.Replace("*", "<sup>1)</sup>");
                    }

                    verseText = verseText
                            .Replace("―", String.Empty)
                            .Replace("<n>", @"<span class=""text-muted"">")
                            .Replace("</n>", "</span>")
                            .Replace("<J>", @"<span class=""text-danger"">")
                            .Replace("</J>", "</span>");
                    var simpleText = BibleTag.GetVerseSimpleText(record["VerseText"].ToString(), index, baseBookShortcut);
                    var translationName = translation.Key;

                    if (translationName == "PBD") { translationName = "SNPPD"; }

                    model.Add(new SearchItemModel() {
                        Book = index.NumberOfBook,
                        BookShortcut = baseBookShortcut,
                        Chapter = index.NumberOfChapter,
                        Verse = record["NumberOfVerse"].ToInt(),
                        TranslationName = translationDesc,
                        Translation = index.TranslationName,
                        VerseText = verseText,
                        SimpleText = simpleText,
                        Index = _index.ToString()
                    });
                }
            }

            return View(model);
        }

        private string GetRangeCriteriaString(int start, int end) {
            return $" AND ([NumberOfBook] Between ({start},{end}))";
        }

        public TranslationControllerModel GetTranslationControllerModel(UnitOfWork uow = null) {
            if (uow == null) { uow = new UnitOfWork(); }
            return new TranslationControllerModel(new Translation() {
                Name = "BW",
                Description = "Biblia Warszawska",
                Language = Language.Polish
            }, books: TranslationInfoController.GetBookBases(uow));
        }
    }



    [ApiController]
    [Route("api/[controller]")]
    public class ScriptureController : Controller {
        private readonly IBibleTagController BibleTag;
        private readonly ITranslationInfoController TranslationInfoController;
        public ScriptureController(IBibleTagController bibleTag, ITranslationInfoController translationInfoController) {
            BibleTag = bibleTag;
            TranslationInfoController = translationInfoController;
        }

        [HttpGet("url/{siglum}")]
        public ActionResult<ScriptureModel> GetUrl(string siglum) {
            if (siglum.IsNotNullOrEmpty()) {                
                var session = new UnitOfWork();
                var url = BibleTag.GetRecognizedSiglumUrl(session, siglum);

                return new ScriptureModel() {
                    Siglum = siglum,
                    Url = $"https://kosciol-jezusa.pl{url}",
                    ShortUrl = url,
                    Text = ""
                };
            }

            return new ScriptureModel() {
                Siglum = siglum,
                Url = "",
                ShortUrl ="",
                Text = ""
            };
        }

        // https://localhost:44378/api/Scripture?siglum=NPI Rdz 22:1-4
        // https://localhost:44378/api/Scripture?siglum=/BW/560/1/3,4,5,6,7,8,9,10,11,12,13,14
        [HttpGet]
        public ActionResult<ScriptureModel> Get(string siglum) {
            if (siglum.IsNotNullOrEmpty()) {
                if (siglum.StartsWith("/")) {
                    var t = siglum.Split('/', StringSplitOptions.RemoveEmptyEntries);
                    if (t.Length >= 3) {
                        var translationName = t[0];
                        if (translationName == "SNPPD" || translationName == "SNPD") { translationName = "PBD"; }
                        if (translationName == "SNPL") { translationName = "SNP18"; }
                        if (translationName == "PNS") { translationName = "PNS1997"; }

                        var bookNumber = t[1];
                        var chapterNumber = t[2];
                        var versesNumbers = t.Length == 3 ? "1" : t[3];
                        var session = new UnitOfWork();
                        var book = TranslationInfoController.GetBookBases(session).Where(x => x.NumberOfBook == bookNumber.ToInt()).FirstOrDefault();

                        if (versesNumbers.Contains(",")) {
                            var numbersOfVerses = versesNumbers.Split(",");
                            var result = new ScriptureModel() { Siglum = "", Text = "", Url = $"" };
                            foreach (var numberOfVerse in numbersOfVerses) {
                                var index = $"{translationName}.{bookNumber}.{chapterNumber}.{numberOfVerse}";
                                var verse = new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
                                result.Text += $" {verse.GetVerySimpleText()}";
                                if (result.Siglum.IsNullOrEmpty()) {
                                    var v = numbersOfVerses.Length > 1 ? $"{numbersOfVerses.First()}-{numbersOfVerses.Last()}" : $"{numbersOfVerses.First()}";
                                    result.Siglum = $"{verse.ParentTranslation.Description}, {book.BookName} {chapterNumber}:{v}";
                                    result.Url = $"/Search/Siglum?text={verse.ParentTranslation.Name.ReplaceAnyWith(String.Empty, "+", "'")} {book.BookShortcut} {chapterNumber}:{v}&type=";
                                }
                            }
                            result.Text = result.Text.Trim();
                            return result;
                        }
                        else {
                            if (translationName == "SNPPD" || translationName == "SNPD") { translationName = "PBD"; }
                            if (translationName == "SNPL") { translationName = "SNP18"; }
                            if (translationName == "PNS") { translationName = "PNS1997"; }

                            var index = $"{translationName}.{bookNumber}.{chapterNumber}.{versesNumbers}";
                            var verse = new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
                            if (verse != null) {
                                var result = new ScriptureModel() {
                                    Siglum = $"{verse.ParentTranslation.Description}, {book.BookName} {chapterNumber}:{versesNumbers}",
                                    Text = $"{verse.GetVerySimpleText().Trim()}",
                                    Url = $"/Search/Siglum?text={verse.ParentTranslation.Name.ReplaceAnyWith(String.Empty, "+", "'")} {book.BookShortcut} {chapterNumber}:{versesNumbers}&type="
                                };
                                return result;
                            }
                            return default;
                        }
                    }
                }


                var recognized = BibleTag.RecognizeSiglum(siglum);
                if (recognized != null) {
                    var session = new UnitOfWork();
                    var book = new XPQuery<BookBase>(session).Where(x => x.BookShortcut == recognized.BookShortcut).FirstOrDefault();
                    var result = new ScriptureModel() { Siglum = "", Text = "", Url = $"/Search/Siglum?text={siglum}&type=" };
                    foreach (var numberOfVerse in recognized.NumbersOfVerses) {
                        var index = $"{recognized.TranslationName}.{book.NumberOfBook}.{recognized.NumberOfChapter}.{numberOfVerse}";
                        var verse = new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
                        result.Text += $" {verse.GetVerySimpleText()}";
                        if (result.Siglum.IsNullOrEmpty()) {
                            var v = recognized.NumbersOfVerses.Length > 1 ? $"{recognized.NumbersOfVerses.First()}-{recognized.NumbersOfVerses.Last()}" : $"{recognized.NumbersOfVerses.First()}";
                            result.Siglum = $"{verse.ParentTranslation.Description}, {book.BookName} {recognized.NumberOfChapter}:{v}";
                        }
                    }

                    result.Text = result.Text.Trim();
                    return result;
                }
            }
            return default;
        }
    }

    //[ApiController]
    //[Route("api/[controller]")]
    //public class GetSiglumUrlController : Controller {
    //    private readonly IBibleTagController BibleTag;
    //    private readonly IConfiguration Configuration;
    //    public GetSiglumUrlController(IBibleTagController bibleTag, IConfiguration configuration) {
    //        BibleTag = bibleTag;
    //        Configuration = configuration;
    //    }

    //    [HttpGet]
    //    public async Task<IActionResult> Get() {
    //        var qs = Request.QueryString;

    //        if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 5) {
    //            var text = HttpUtility.UrlDecode(qs.Value).RemoveAny("?q=");
    //            if (text.IsNotNullOrEmpty() && text.Length > 0) {
    //                var session = new UnitOfWork();
    //                var url = BibleTag.GetRecognizedSiglumUrl(session, text);
    //                if (url.IsNotNullOrEmpty()) {
    //                    return Ok($"{Configuration["HostUrl"]}{url}");
    //                }
    //            }
    //        }
    //        return NotFound();
    //    }
    //}

    public class SearchResultsModel : List<SearchItemModel> {
        public IEnumerable<string> Words { get; }
        public Dictionary<string, string> Links { get; set; }
        public SearchRangeType SearchType { get; set; }
        private SearchResultsModel() { }
        public SearchResultsModel(params string[] words) : this() {
            Words = words;
        }
    }

    public class SearchItemModel {
        public string TranslationName { get; set; }
        public string Translation { get; set; }
        public string BookShortcut { get; set; }
        public int Book { get; set; }
        public int Chapter { get; set; }
        public int Verse { get; set; }
        public string VerseText { get; set; }
        public string SimpleText { get; set; }
        public string Index { get; set; }
    }

    public enum SearchRangeType {
        [Description("Wszędzie")]
        [Category("")]
        All,
        [Description("w Starym Przymierzu (Testamencie)")]
        [Category("SP")]
        OldTestament,
        [Description("w Nowym Przymierzu (Testamencie)")]
        [Category("NP")]
        NewTestament,
        [Description("w Piecioksięgu (Tora)")]
        [Category("T")]
        Pentateuch,
        [Description("w Księgach historycznych")]
        [Category("H")]
        Historical,
        [Description("w Księgach mądrościowych")]
        [Category("W")]
        Wisdom,
        [Description("w Księgach prorockich")]
        [Category("PR")]
        Prophets,
        [Description("w Ewangeliach")]
        [Category("E")]
        Gospel,
        [Description("w Psalmach")]
        [Category("P")]
        Psalms,
        [Description("w listach ap. Pawła")]
        [Category("PL")]
        PaulsLetters,
        [Description("w listach powszechnych")]
        [Category("LK")]
        CatholicLetters,
    }

    public class ScriptureModel {
        public string Siglum { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public string ShortUrl {  get; set; }
    }
}
