/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/


namespace ChurchServices.Data.Export.Model {
    public class TranslationControllerModel {
        public Translation Translation { get; }
        public List<BookBaseInfo> Books { get; }
        public string Book { get; }
        public string Chapter { get; }
        public string Verse { get; }
        public int NTBookNumber { get; }
        public int LogosBookNumber { get; }
        public List<TranslationInfo> Translations { get; }
        public BibleStructureInfo StructureInfo { get; }
        public bool IsInterlinear { get; }
        public Language InterlinearLanguage { get; }
        public TranslationControllerModel(Translation t, string book = null, string chapter = null, string verse = null, List<BookBaseInfo> books = null) {
            Translation = t;
            if (t != null) {
                IsInterlinear = t.Type == TranslationType.Interlinear;
                if (IsInterlinear) {
                    try {
                        InterlinearLanguage = t.Books.First().Chapters.First().Verses.First().VerseWords.First().StrongCode.Lang;
                    }
                    catch { }
                }
            }
            Book = book;
            Chapter = chapter;
            Verse = verse;
            Translations = new List<TranslationInfo>();
            Books = books;
            NTBookNumber = GetNTBookNumber();
            LogosBookNumber = GetLogosBookNumber();

            if (book == null) {
                StructureInfo = new BibleStructureInfo() { Name = t.Name, Type = Translation.Type, Books = new List<BibleBookStructureInfo>() };
                foreach (var b in Books.OrderBy(x => x.NumberOfBook)) {
                    var tb = t.Books.Where(x => x.NumberOfBook == b.NumberOfBook).FirstOrDefault();
                    if (tb != null) {
                        var bookInfo = new BibleBookStructureInfo() {
                            NumberOfBook = b.NumberOfBook,
                            Title = b.BookTitle.Replace("<br/>", " "),
                            BookShortcut = tb.BookShortcut,
                            BaseBookShortcut = b.BookShortcut,
                            Chapters = new List<BibleBookChapterStructureInfo>(),
                            ChapterStartNumber = 1,
                            ChapterEndNumber = tb.Chapters != null && tb.Chapters.Count > 0 ? tb.Chapters.Max(x => x.NumberOfChapter) : 0,
                            IsNotTranslated = Translation.Type == TranslationType.Interlinear && !tb.IsTranslated && Translation.BookType == TheBookType.Bible,
                            Color = tb.Color,
                            IsInterlinearBible = Translation.Type == TranslationType.Interlinear && Translation.BookType == TheBookType.Bible,
                            FirstTranslatedChapter = Translation.Type == TranslationType.Interlinear ? (tb.Chapters != null && tb.Chapters.Count > 0 && tb.Chapters.Where(x => x.IsTranslated).Any() ? tb.Chapters.Where(x => x.IsTranslated).Min(x => x.NumberOfChapter) : 0) : (tb.Chapters != null && tb.Chapters.Count > 0 ? tb.Chapters.Min(x => x.NumberOfChapter) : 0)
                        };

                        foreach (var c in tb.Chapters.OrderBy(x => x.NumberOfChapter)) {
                            var chapterInfo = new BibleBookChapterStructureInfo() {
                                ChapterNumber = c.NumberOfChapter,
                                VerseStartNumber = 1,
                                VerseEndNumber = c.Verses.Max(x => x.NumberOfVerse)
                            };
                            bookInfo.Chapters.Add(chapterInfo);
                        }
                        StructureInfo.Books.Add(bookInfo);
                    }
                }
            }
            else { 
            
            }
        }

        private int GetNTBookNumber() {
            if (Translation.Name.Replace("+", String.Empty) == "IPD") { return 67; }
            var book = Book.ToInt();
            var r = 1;
            for (int i = 470; i <= 730; i += 10) {
                if (i == book) {
                    return r;
                }
                r++;
            }
            return r;
        }
        private int GetLogosBookNumber() {
            var book = Book.ToInt();
            var r = 1;
            if (book < 470) {
                foreach (var item in Books) {
                    if (item.NumberOfBook < 470) {
                        if (book == item.NumberOfBook) { return r; }
                    }
                    else {
                        break;
                    }
                    r++;
                }
            }
            else {
                r = 61;
                foreach (var item in Books) {
                    if (item.NumberOfBook >= 470) {
                        if (book == item.NumberOfBook) { return r; }
                        r++;
                    }
                    else {
                        continue;
                    }
                }
            }

            return r;
        }
    }
}
