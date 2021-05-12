/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using IBE.Data.Model;
using IBE.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using LinqKit;

namespace Church.WebApp.Controllers {
    public class TranslationController : Controller {
        private readonly ILogger<TranslationController> _logger;
        public TranslationController(ILogger<TranslationController> logger) {
            _logger = logger;
        }

        // "{translationName}/{book?}/{chapter?}/{verse?}"
        public IActionResult Index(string translationName, string book = null, string chapter = null, string verse = null) {
            if (!String.IsNullOrEmpty(translationName)) {
                var uow = new UnitOfWork();
                var books = new XPQuery<BookBase>(uow).ToList();

                // wyświetlamy listę ksiąg z tego przekładu
                if (String.IsNullOrEmpty(book)) {
                    var translation = new XPQuery<Translation>(uow).Where(x => x.Name.Replace("'", "").Replace("+", "") == translationName).FirstOrDefault();
                    if (translation != null) {
                        return View(new TranslationControllerModel(translation, books: books));
                    }
                }
                else {
                    var translation = new XPQuery<Translation>(uow).Where(x => x.Name.Replace("'", "").Replace("+", "") == translationName).FirstOrDefault();
                    if (translation != null) {
                        var result = new TranslationControllerModel(translation, book, chapter, verse, books);

                        var view = new XPView(uow, typeof(Translation));
                        view.CriteriaString = $"[Books][[NumberOfBook] = '{book}']";
                        view.Properties.Add(new ViewProperty("Name", SortDirection.None, "[Name]", false, true));
                        view.Properties.Add(new ViewProperty("Description", SortDirection.None, "[Description]", false, true));
                        view.Properties.Add(new ViewProperty("Type", SortDirection.None, "[Type]", false, true));
                        view.Properties.Add(new ViewProperty("Catholic", SortDirection.None, "[Catolic]", false, true));
                        view.Properties.Add(new ViewProperty("Recommended", SortDirection.None, "[Recommended]", false, true));
                        view.Properties.Add(new ViewProperty("OpenAccess", SortDirection.None, "[OpenAccess]", false, true));
                        foreach (ViewRecord item in view) {
                            result.Translations.Add(new TranslationInfo() {
                                Name = item["Name"].ToString(),
                                Description = item["Description"].ToString(),
                                TranslationType = ((TranslationType)item["Type"]).GetDescription(),
                                Catholic = (bool)item["Catholic"],
                                Recommended = (bool)item["Recommended"],
                                PasswordRequired = !((bool)item["OpenAccess"])
                            });
                        }

                        return View(result);
                    }
                }
            }

            return View();
        }

        public static string GetInternalVerseRangeHtml(string input, TranslationControllerModel model) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\-(?<verseEnd>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var translationName = model.Translation.Name.Replace("+", "");
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Translation.Books.Where(x => x.NumberOfBook == numberOfBook).First().BaseBook.BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();
                var verseEnd = m.Groups["verseEnd"].Value.ToInt();
                var versesText = String.Empty;
                for (int i = verseStart; i <= verseEnd; i++) {
                    versesText += $"{i}";
                    if (i != verseEnd) { versesText += ","; }
                }

                var verseText = GetVerseTranslation(model.Books.First().Session, numberOfBook, numberOfChapter, verseStart, verseEnd, translationName);

                var link = $"<a href=\"/{translationName}/{numberOfBook}/{numberOfChapter}/{versesText}\">{bookShortcut} {numberOfChapter}:{verseStart}-{verseEnd}</a>";

                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }
        public static string GetInternalVerseRangeText(string input, TranslationControllerModel model) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\-(?<verseEnd>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var translationName = model.Translation.Name.Replace("+", "");
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Translation.Books.Where(x => x.NumberOfBook == numberOfBook).First().BaseBook.BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();
                var verseEnd = m.Groups["verseEnd"].Value.ToInt();

                return $"{bookShortcut} {numberOfChapter}:{verseStart}-{verseEnd}";
            });
            return input;
        }


        public static string GetInternalVerseHtml(string input, TranslationControllerModel model) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var translationName = model.Translation.Name.Replace("+", "");
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Translation.Books.Where(x => x.NumberOfBook == numberOfBook).First().BaseBook.BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();

                var verseText = GetVerseTranslation(model.Books.First().Session, numberOfBook, numberOfChapter, verseStart, translationName: translationName);

                var link = $"<a href=\"/{translationName}/{numberOfBook}/{numberOfChapter}/{verseStart}\">{bookShortcut} {numberOfChapter}:{verseStart}</a>";

                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }
        public static string GetInternalVerseText(string input, TranslationControllerModel model) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Translation.Books.Where(x => x.NumberOfBook == numberOfBook).First().BaseBook.BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();

                return $"{bookShortcut} {numberOfChapter}:{verseStart}";
            });
            return input;
        }

        public static string GetExternalVerseRangeHtml(string input, TranslationControllerModel model) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<translationName>[a-zA-Z]+)\s(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\-(?<verseEnd>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var translationName = m.Groups["translationName"].Value;
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var baseBook = model.Books.Where(x => x.NumberOfBook == numberOfBook).FirstOrDefault();
                var bookShortcut = baseBook.IsNotNull() ? baseBook.BookShortcut : "";
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();
                var verseEnd = m.Groups["verseEnd"].Value.ToInt();
                var versesText = String.Empty;
                for (int i = verseStart; i <= verseEnd; i++) {
                    versesText += $"{i}";
                    if (i != verseEnd) { versesText += ","; }
                }

                var verseText = GetVerseTranslation(model.Books.First().Session, numberOfBook, numberOfChapter, verseStart, verseEnd, translationName);

                var link = $"<a href=\"/{translationName}/{numberOfBook}/{numberOfChapter}/{versesText}\">{bookShortcut} {numberOfChapter}:{verseStart}-{verseEnd}</a>";

                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }
        public static string GetExternalVerseRangeText(string input, TranslationControllerModel model) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<translationName>[a-zA-Z]+)\s(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\-(?<verseEnd>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var baseBook = model.Books.Where(x => x.NumberOfBook == numberOfBook).FirstOrDefault();
                var bookShortcut = baseBook.IsNotNull() ? baseBook.BookShortcut : "";
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();
                var verseEnd = m.Groups["verseEnd"].Value.ToInt();

                return $"{bookShortcut} {numberOfChapter}:{verseStart}-{verseEnd}";
            });
            return input;
        }

        public static string GetExternalVerseHtml(string input, TranslationControllerModel model) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<translationName>[a-zA-Z]+)\s(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var translationName = m.Groups["translationName"].Value;
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var baseBook = model.Books.Where(x => x.NumberOfBook == numberOfBook).FirstOrDefault();
                var bookShortcut = baseBook.IsNotNull() ? baseBook.BookShortcut : "";
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();

                var verseText = GetVerseTranslation(model.Books.First().Session, numberOfBook, numberOfChapter, verseStart, translationName: translationName);

                var link = $"<a href=\"/{translationName}/{numberOfBook}/{numberOfChapter}/{verseStart}\">{bookShortcut} {numberOfChapter}:{verseStart}</a>";
                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }
        public static string GetExternalVerseText(string input, TranslationControllerModel model) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<translationName>[a-zA-Z]+)\s(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var translationName = m.Groups["translationName"].Value;
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var baseBook = model.Books.Where(x => x.NumberOfBook == numberOfBook).FirstOrDefault();
                var bookShortcut = baseBook.IsNotNull() ? baseBook.BookShortcut : "";
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();

                return $"{bookShortcut} {numberOfChapter}:{verseStart}";
            });
            return input;
        }

        private static string GetVerseTranslation(Session session, int numberOfBook, int numberOfChapter, int verseStart, int verseEnd = 0, string translationName = "NPI") {
            if (verseEnd == 0) {
                var index = $"{translationName}.{numberOfBook}.{numberOfChapter}.{verseStart}";
                var verse = new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
                if (verse.IsNotNull()) {
                    var verseText = verse.GetTranslationText();
                    if (verseText.IsNotNullOrWhiteSpace()) {
                        return verseText;
                    }
                    else {
                        if (numberOfBook > 460) {
                            index = $"PBPW.{numberOfBook}.{numberOfChapter}.{verseStart}";
                            verse = new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
                        }
                        else {
                            index = $"SNP18.{numberOfBook}.{numberOfChapter}.{verseStart}";
                            verse = new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
                        }
                        if (verse.IsNotNull()) {
                            verseText = verse.Text;
                            verseText = System.Text.RegularExpressions.Regex.Replace(verseText, @"\[[0-9]+\]", "");

                            return verseText;
                        }
                    }
                }
            }
            else {
                var predicate = PredicateBuilder.New<Verse>();
                for (int i = verseStart; i < verseEnd + 1; i++) {
                    var index = $"{translationName}.{numberOfBook}.{numberOfChapter}.{i}";
                    predicate = predicate.Or(x => x.Index == index);
                }
                var verses = new XPQuery<Verse>(session).Where(predicate);

                if (verses.Count() > 0) {
                    if (verses.First().GetTranslationText().IsNotNullOrWhiteSpace()) {
                        var versesText = String.Empty;
                        foreach (var item in verses) {
                            versesText += item.GetTranslationText() + " ";
                        }
                        return versesText.Trim();
                    }
                    else {
                        if (numberOfBook > 460) {
                            predicate = PredicateBuilder.New<Verse>();
                            for (int i = verseStart; i < verseEnd + 1; i++) {
                                var index = $"PBPW.{numberOfBook}.{numberOfChapter}.{i}";
                                predicate = predicate.Or(x => x.Index == index);
                            }

                            verses = new XPQuery<Verse>(session).Where(predicate);
                        }
                        else {
                            predicate = PredicateBuilder.New<Verse>();
                            for (int i = verseStart; i < verseEnd + 1; i++) {
                                var index = $"SNP18.{numberOfBook}.{numberOfChapter}.{i}";
                                predicate = predicate.Or(x => x.Index == index);
                            }

                            verses = new XPQuery<Verse>(session).Where(predicate);
                        }

                        if (verses.Count() > 0) {
                            var versesText = String.Empty;
                            foreach (var item in verses) {
                                versesText += item.Text + " ";
                            }
                            versesText = System.Text.RegularExpressions.Regex.Replace(versesText, @"\[[0-9]+\]", "");

                            return versesText.Trim();
                        }
                    }
                }
            }
            return String.Empty;
        }
    }

    public class TranslationControllerModel {
        public Translation Translation { get; }
        public List<BookBase> Books { get; }
        public string Book { get; }
        public string Chapter { get; }
        public string Verse { get; }
        public int NTBookNumber { get; }
        public int LogosBookNumber { get; }
        public List<TranslationInfo> Translations { get; }
        public TranslationControllerModel(Translation t, string b = null, string c = null, string v = null, List<BookBase> books = null) {
            Translation = t;
            Book = b;
            Chapter = c;
            Verse = v;
            Translations = new List<TranslationInfo>();
            Books = books;
            NTBookNumber = GetNTBookNumber();
            LogosBookNumber = GetLogosBookNumber();
        }

        private int GetNTBookNumber() {
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
    public class TranslationInfo {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TranslationType { get; set; }
        public bool Catholic { get; set; }
        public bool Recommended { get; set; }
        public bool PasswordRequired { get; set; }
    }
}
