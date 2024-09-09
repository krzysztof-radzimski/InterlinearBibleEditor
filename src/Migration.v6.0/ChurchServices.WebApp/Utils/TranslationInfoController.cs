/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Utils {
    public interface ITranslationInfoController {
        List<Grouping<string, TranslationInfo>> GetBibleTranslations();
        string[] GetTranslationNames(string postfix = null);
        List<Grouping<string, TranslationInfo>> GetPatrologyTranslations();
        List<ArticleInfo> GetLastFourArticles();
        List<SongsInfo> GetSongs();
        List<BookBaseInfo> GetBookBases(UnitOfWork uow = null);
        TranslationControllerModel GetTranslationControllerModel(string translationName, string book = null, string chapter = null, string verse = null);
    }
    public class TranslationInfoController : ITranslationInfoController {
        private const string BIBLETRANSLATIONS = "BibleTranslations";
        private const string PATROLOGYTRANSLATIONS = "PatrologyTranslations";
        private const string LASTFOURARTICLES = "LastFourArticles";
        private const string BOOKBASES = "BookBases";
        private const string SONGS = "Songs";

        private readonly IMemoryCache MemoryCache;

        public TranslationInfoController(IMemoryCache memoryCache) { MemoryCache = memoryCache; }

        public string[] GetTranslationNames(string postfix = null) {
            var names = new List<string>();
            foreach (var translation in GetBibleTranslations()) {
                foreach (var info in translation) {
                    names.Add(info.Name.Replace("+", ""));
                }
            }

            if (postfix.IsNotNullOrEmpty()) {
                for (int i = 0; i < names.Count; i++) {
                    names[i] = names[i] + postfix;
                }
            }

            return names.ToArray();
        }
        public List<Grouping<string, TranslationInfo>> GetBibleTranslations() {
            List<Grouping<string, TranslationInfo>> _allTranslations;
            MemoryCache.TryGetValue(BIBLETRANSLATIONS, out _allTranslations);
            if (_allTranslations != null) {
                return _allTranslations;
            }

            var view = new XPView(new UnitOfWork(), typeof(Translation)) {
                CriteriaString = "[BookType] = 1 AND [Hidden] = 0"
            };
            view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
            view.Properties.Add(new ViewProperty("Description", SortDirection.None, "[Description]", false, true));
            view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
            view.Properties.Add(new ViewProperty("Catolic", SortDirection.None, "[Catolic]", false, true));
            view.Properties.Add(new ViewProperty("Recommended", SortDirection.None, "[Recommended]", false, true));
            view.Properties.Add(new ViewProperty("OpenAccess", SortDirection.None, "[OpenAccess]", false, true));
            view.Properties.Add(new ViewProperty("Language", SortDirection.None, "[Language]", false, true));

            var model = new List<TranslationInfo>();
            foreach (ViewRecord item in view) {
                var tInfo = new TranslationInfo() {
                    Type = (TranslationType)item["Type"],
                    Name = item["Name"].ToString().Replace("'", ""),
                    Description = item["Description"].ToString(),
                    Catholic = (bool)item["Catolic"],
                    Recommended = (bool)item["Recommended"],
                    PasswordRequired = !((bool)item["OpenAccess"]),
                    Language = (Language)item["Language"]
                };
                if (tInfo.Name == "PBD") { tInfo.Name = "SNPD"; }
                if (tInfo.Name == "SNP18") { tInfo.Name = "SNPL"; }
                model.Add(tInfo);
            }

            _allTranslations = new List<Grouping<string, TranslationInfo>> {
                new Grouping<string, TranslationInfo>(TranslationType.Interlinear.GetDescription(), model.Where(x => x.Type == TranslationType.Interlinear).OrderBy(x=>x.Description)),
                new Grouping<string, TranslationInfo>(TranslationType.Literal.GetDescription(), model.Where(x => x.Type == TranslationType.Literal).OrderBy(x=>x.Description)),
                new Grouping<string, TranslationInfo>(TranslationType.Default.GetDescription(), model.Where(x => x.Type == TranslationType.Default).OrderBy(x=>x.Description)),
                new Grouping<string, TranslationInfo>(TranslationType.Dynamic.GetDescription(), model.Where(x => x.Type == TranslationType.Dynamic).OrderBy(x=>x.Description))
            };
            MemoryCache.Set(BIBLETRANSLATIONS, _allTranslations);
            return _allTranslations;
        }
        public List<Grouping<string, TranslationInfo>> GetPatrologyTranslations() {
            List<Grouping<string, TranslationInfo>> _allTranslations;
            MemoryCache.TryGetValue(PATROLOGYTRANSLATIONS, out _allTranslations);
            if (_allTranslations != null) {
                return _allTranslations;
            }

            var view = new XPView(new UnitOfWork(), typeof(Translation)) {
                CriteriaString = "[BookType] = 2 AND [Hidden] = 0"
            };
            view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
            view.Properties.Add(new ViewProperty("Description", SortDirection.None, "[Description]", false, true));
            view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
            view.Properties.Add(new ViewProperty("Catolic", SortDirection.None, "[Catolic]", false, true));
            view.Properties.Add(new ViewProperty("Recommended", SortDirection.None, "[Recommended]", false, true));
            view.Properties.Add(new ViewProperty("OpenAccess", SortDirection.None, "[OpenAccess]", false, true));

            var model = new List<TranslationInfo>();
            foreach (ViewRecord item in view) {
                model.Add(new TranslationInfo() {
                    Type = (TranslationType)item["Type"],
                    Name = item["Name"].ToString().Replace("'", ""),
                    Description = item["Description"].ToString(),
                    Catholic = (bool)item["Catolic"],
                    Recommended = (bool)item["Recommended"],
                    PasswordRequired = !((bool)item["OpenAccess"])
                });
            }

            _allTranslations = new List<Grouping<string, TranslationInfo>> {
                new Grouping<string, TranslationInfo>(TranslationType.Interlinear.GetDescription(), model.Where(x => x.Type == TranslationType.Interlinear)),
                new Grouping<string, TranslationInfo>(TranslationType.Literal.GetDescription(), model.Where(x => x.Type == TranslationType.Literal)),
                new Grouping<string, TranslationInfo>(TranslationType.Default.GetDescription(), model.Where(x => x.Type == TranslationType.Default)),
                new Grouping<string, TranslationInfo>(TranslationType.Dynamic.GetDescription(), model.Where(x => x.Type == TranslationType.Dynamic))
            };
            MemoryCache.Set(PATROLOGYTRANSLATIONS, _allTranslations);
            return _allTranslations;
        }
        public List<ArticleInfo> GetLastFourArticles() {
            List<ArticleInfo> lastFourArticles;
            MemoryCache.TryGetValue(LASTFOURARTICLES, out lastFourArticles);
            if (lastFourArticles != null) {
                return lastFourArticles;
            }

            var view = new XPView(new UnitOfWork(), typeof(Article)) {
                TopReturnedRecords = 4,
                CriteriaString = "[Hidden] = 0"
            };
            view.Properties.Add(new ViewProperty("AuthorName", SortDirection.None, "[AuthorName]", false, true));
            view.Properties.Add(new ViewProperty("AuthorPicture", SortDirection.None, "[AuthorPicture]", false, true));
            view.Properties.Add(new ViewProperty("Date", SortDirection.Descending, "[Date]", false, true));
            view.Properties.Add(new ViewProperty("Oid", SortDirection.None, "[Oid]", false, true));
            view.Properties.Add(new ViewProperty("Lead", SortDirection.None, "[Lead]", false, true));
            view.Properties.Add(new ViewProperty("Subject", SortDirection.None, "[Subject]", false, true));
            view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
            view.Properties.Add(new ViewProperty("Passage", SortDirection.None, "[Passage]", false, true));

            lastFourArticles = new List<ArticleInfo>();
            foreach (ViewRecord item in view) {
                lastFourArticles.Add(new ArticleInfo() {
                    AuthorName = item["AuthorName"].ToString(),
                    AuthorPicture = item["AuthorPicture"].IsNotNull() ? Convert.ToBase64String((byte[])item["AuthorPicture"]) : String.Empty,
                    Date = Convert.ToDateTime(item["Date"].ToString()),
                    Id = item["Oid"].ToInt(),
                    Lead = item["Lead"].ToString(),
                    Subject = item["Subject"].ToString(),
                    Type = ((ArticleType)item["Type"]).GetDescription(),
                    Passage = item["Passage"]?.ToString()
                });
            }
            MemoryCache.Set(LASTFOURARTICLES, lastFourArticles);
            return lastFourArticles;
        }
        public List<SongsInfo> GetSongs() {
            List<SongsInfo> songs;
            MemoryCache.TryGetValue(SONGS, out songs);
            if (songs != null) {
                return songs;
            }
            var view = new XPView(new UnitOfWork(), typeof(Song));
            view.Properties.Add(new ViewProperty("Id", SortDirection.None, "[Oid]", false, true));
            view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
            view.Properties.Add(new ViewProperty("Signature", SortDirection.None, "[Signature]", false, true));
            view.Properties.Add(new ViewProperty("BPM", SortDirection.None, "[BPM]", false, true));
            view.Properties.Add(new ViewProperty("Number", SortDirection.Ascending, "[Number]", false, true));
            view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
            songs = new List<SongsInfo>();
            foreach (ViewRecord record in view) {
                songs.Add(new SongsInfo() {
                    Type = record["Type"] != null ? (SongGroupType)record["Type"] : SongGroupType.Default,
                    BPM = record["BPM"] != null ? record["BPM"].ToString() : String.Empty,
                    Id = record["Id"] != null ? record["Id"].ToInt() : 0,
                    Name = record["Name"] != null ? record["Name"].ToString() : String.Empty,
                    Number = record["Number"] != null ? record["Number"].ToInt() : 0,
                    Signature = record["Signature"] != null ? record["Signature"].ToString() : string.Empty
                });
            }
            MemoryCache.Set(SONGS, songs);
            return songs;
        }

        public List<BookBaseInfo> GetBookBases(UnitOfWork uow = null) {
            List<BookBaseInfo> result;
            MemoryCache.TryGetValue(BOOKBASES, out result);
            if (result != null) {
                return result;
            }

            if (uow == null) { uow = new UnitOfWork(); }
            var books = new XPQuery<BookBase>(uow).ToList();
            result = new List<BookBaseInfo>();
            foreach (var item in books) {
                result.Add(new BookBaseInfo() {
                    BookName = item.BookName,
                    BookShortcut = item.BookShortcut,
                    BookTitle = item.BookTitle,
                    Color = item.Color,
                    NumberOfBook = item.NumberOfBook,
                    StatusBiblePart = item.StatusBiblePart,
                    StatusBookType = item.StatusBookType,
                    StatusCanonType = item.StatusCanonType,
                    AuthorName = item.AuthorName,
                    PlaceWhereBookWasWritten = item.PlaceWhereBookWasWritten,
                    Preface = item.Preface,
                    Purpose = item.Purpose,
                    Subject = item.Subject,
                    TimeOfWriting = item.TimeOfWriting
                });
            }
            MemoryCache.Set(BOOKBASES, result);
            return result;
        }
        public TranslationControllerModel GetTranslationControllerModel(string translationName, string book = null, string chapter = null, string verse = null) {
            TranslationControllerModel result;

            if (!String.IsNullOrEmpty(translationName)) {
                var uow = new UnitOfWork();
                var books = GetBookBases(uow);

                // wyświetlamy listę ksiąg z tego przekładu
                if (String.IsNullOrEmpty(book)) {
                    var translation = new XPQuery<Translation>(uow).Where(x => x.Name.Replace("'", "").Replace("+", "").ToLower() == translationName.ToLower()).FirstOrDefault();
                    if (translation != null) {
                        result = new TranslationControllerModel(translation, books: books);
                        return result;
                    }
                }
                else {
                    var translation = new XPQuery<Translation>(uow).Where(x => x.Name.Replace("'", "").Replace("+", "").ToLower() == translationName.ToLower()).FirstOrDefault();
                    if (translation != null) {
                        result = new TranslationControllerModel(translation, book, chapter, verse, books);
                        List<TranslationInfo> translations;
                        MemoryCache.TryGetValue($"{BIBLETRANSLATIONS}_{book}", out translations);
                        if (translations != null) {
                            result.Translations.AddRange(translations);
                        }
                        else {
                            var view = new XPView(uow, typeof(Translation)) {
                                CriteriaString = $"[Books][[NumberOfBook] = '{book}'] AND [Hidden] = 0"
                            };
                            view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
                            view.Properties.Add(new ViewProperty("Description", SortDirection.None, "[Description]", false, true));
                            view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
                            view.Properties.Add(new ViewProperty("Catholic", SortDirection.None, "[Catolic]", false, true));
                            view.Properties.Add(new ViewProperty("Recommended", SortDirection.None, "[Recommended]", false, true));
                            view.Properties.Add(new ViewProperty("OpenAccess", SortDirection.None, "[OpenAccess]", false, true));
                            translations = new List<TranslationInfo>();
                            foreach (ViewRecord item in view) {
                                translations.Add(new TranslationInfo() {
                                    Name = item["Name"].ToString(),
                                    Description = item["Description"].ToString(),
                                    Type = (TranslationType)item["Type"],
                                    Catholic = (bool)item["Catholic"],
                                    Recommended = (bool)item["Recommended"],
                                    PasswordRequired = !((bool)item["OpenAccess"])
                                });
                            }
                            if (translations.Count > 0) {
                                result.Translations.AddRange(translations);
                                MemoryCache.Set($"{BIBLETRANSLATIONS}_{book}", translations);
                            }
                        }

                        return result;
                    }
                }
            }
            return default;
        }
    }
}
