using Church.WebApp.Models;
using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Church.WebApp.Utils {
    public interface IBibleTagController {
        string AppendNonBreakingSpaces(string text);
        string CleanVerseText(string text);

        string GetInternalVerseRangeHtml(string input, TranslationControllerModel model);
        string GetInternalVerseRangeText(string input, TranslationControllerModel model);

        string GetInternalVerseHtml(string input, TranslationControllerModel model);
        string GetInternalVerseText(string input, TranslationControllerModel model);

        string GetExternalVerseRangeHtml(string input, TranslationControllerModel model);
        string GetExternalVerseRangeText(string input, TranslationControllerModel model);

        string GetExternalVerseHtml(string input, TranslationControllerModel model);
        string GetExternalVerseText(string input, TranslationControllerModel model);

        string GetInternalVerseListHtml(string input, TranslationControllerModel model);
        string GetMultiChapterRange(string input, TranslationControllerModel model);

        string GetVerseTranslation(Session session, int numberOfBook, int numberOfChapter, int verseStart, int verseEnd = 0, string translationName = "NPI");
    }
    public class BibleTagController : IBibleTagController {
        public string AppendNonBreakingSpaces(string text) {
            text = System.Text.RegularExpressions.Regex.Replace(text, @"[\s\(\,\;][a,i,o,w,z]\s", delegate (System.Text.RegularExpressions.Match m) {
                return " " + m.Value.Trim() + "&nbsp;";
            }, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return text;
        }
        public string CleanVerseText(string text) {
            return text.Replace("</t>", "").Replace("<t>", "").Replace("<pb/>", "").Replace("<n>", "").Replace("</n>", "").Replace("<e>", "").Replace("</e>", "").Replace("―", "").Replace('\'', ' ').Replace("<J>", "").Replace("</J>", "").Replace("<i>", "").Replace("</i>", "");
        }

        public string GetMultiChapterRange(string input, TranslationControllerModel model) {
            var pattern = @"\<x\>(?<book>[0-9]+)\s(?<chapterStart>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)(\s)?\-(\s)?(?<chapterEnd>[0-9]+)(\s)?\:(\s)?(?<verseEnd>[0-9]+)";
            input = System.Text.RegularExpressions.Regex.Replace(input, pattern, delegate (System.Text.RegularExpressions.Match m) {
                var translationName = model.Translation.Name.Replace("+", "").Replace("'", "");
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Translation.Books.Where(x => x.NumberOfBook == numberOfBook).First().BaseBook.BookShortcut;
                var chapterStart = m.Groups["chapterStart"].Value.ToInt();
                var chapterEnd = m.Groups["chapterEnd"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();
                var verseEnd = m.Groups["verseEnd"].Value.ToInt();

                return $"<a href=\"/{translationName}/{numberOfBook}/{chapterStart}/{verseStart}\">{bookShortcut}&nbsp;{chapterStart}:{verseStart}</a>-<a href=\"/{translationName}/{numberOfBook}/{chapterEnd}/{verseEnd}\">{chapterEnd}:{verseEnd}</a>";
            });
            return input;
        }

        public string GetInternalVerseListHtml(string input, TranslationControllerModel model) {
            var pattern = @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verse1>[0-9]+)(\,)(\s)?(?<verse2>[0-9]+)(\,)?(\s)?(?<verse3>[0-9]+)?(\,)?(\s)?(?<verse4>[0-9]+)?(\,)?(\s)?(?<verse5>[0-9]+)?(\,)?(\s)?(?<verse6>[0-9]+)?(\,)?(\s)?(?<verse7>[0-9]+)?(\,)?(\s)?(?<verse8>[0-9]+)?(\,)?(\s)?(?<verse9>[0-9]+)?\<\/x\>";
            input = System.Text.RegularExpressions.Regex.Replace(input, pattern, delegate (System.Text.RegularExpressions.Match m) {
                var translationName = model.Translation.Name.Replace("+", "").Replace("'", "");
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Translation.Books.Where(x => x.NumberOfBook == numberOfBook).First().BaseBook.BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();

                var html = String.Empty;
                var preLink = $"<a href=\"/{translationName}/{numberOfBook}/{numberOfChapter}/";
                var preText = $"{bookShortcut}&nbsp;{numberOfChapter}:";

                html += $"{preLink}{m.Groups["verse1"].Value}\">{preText}{m.Groups["verse1"].Value}</a>";

                for (int i = 2; i < 10; i++) {
                    if (m.Groups[$"verse{i}"] != null && m.Groups[$"verse{i}"].Success) {
                        html += $", {preLink}{m.Groups[$"verse{i}"].Value}\">{m.Groups[$"verse{i}"].Value}</a>";
                    }
                }

                return html;
            });
            return input;
        }

        public string GetInternalVerseRangeHtml(string input, TranslationControllerModel model) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\-(?<verseEnd>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var translationName = model.Translation.Name.Replace("+", "").Replace("'", "");
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

                var verseText = String.Empty;

                var link = $"<a href=\"/{translationName}/{numberOfBook}/{numberOfChapter}/{versesText}\">{bookShortcut}&nbsp;{numberOfChapter}:{verseStart}-{verseEnd}</a>";

                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }
        public string GetInternalVerseRangeText(string input, TranslationControllerModel model) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\-(?<verseEnd>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var translationName = model.Translation.Name.Replace("+", "").Replace("'", "");
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Translation.Books.Where(x => x.NumberOfBook == numberOfBook).First().BaseBook.BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();
                var verseEnd = m.Groups["verseEnd"].Value.ToInt();

                return $"{bookShortcut} {numberOfChapter}:{verseStart}-{verseEnd}";
            });
            return input;
        }


        public string GetInternalVerseHtml(string input, TranslationControllerModel model) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var translationName = model.Translation.Name.Replace("+", "").Replace("'", "");
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Translation.Books.Where(x => x.NumberOfBook == numberOfBook).First().BaseBook.BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();

                var verseText = String.Empty;

                var link = $"<a href=\"/{translationName}/{numberOfBook}/{numberOfChapter}/{verseStart}\">{bookShortcut}&nbsp;{numberOfChapter}:{verseStart}</a>";

                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }
        public string GetInternalVerseText(string input, TranslationControllerModel model) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var bookShortcut = model.Translation.Books.Where(x => x.NumberOfBook == numberOfBook).First().BaseBook.BookShortcut;
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();

                return $"{bookShortcut} {numberOfChapter}:{verseStart}";
            });
            return input;
        }

        public string GetExternalVerseRangeHtml(string input, TranslationControllerModel model) {
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

                var verseText = String.Empty;

                var link = $"<a href=\"/{translationName}/{numberOfBook}/{numberOfChapter}/{versesText}\">{bookShortcut}&nbsp;{numberOfChapter}:{verseStart}-{verseEnd}</a>";

                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }
        public string GetExternalVerseRangeText(string input, TranslationControllerModel model) {
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

        public string GetExternalVerseHtml(string input, TranslationControllerModel model) {
            input = System.Text.RegularExpressions.Regex.Replace(input, @"\<x\>(?<translationName>[a-zA-Z]+)\s(?<book>[0-9]+)\s(?<chapter>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)\<\/x\>", delegate (System.Text.RegularExpressions.Match m) {
                var translationName = m.Groups["translationName"].Value;
                var numberOfBook = m.Groups["book"].Value.ToInt();
                var baseBook = model.Books.Where(x => x.NumberOfBook == numberOfBook).FirstOrDefault();
                var bookShortcut = baseBook.IsNotNull() ? baseBook.BookShortcut : "";
                var numberOfChapter = m.Groups["chapter"].Value.ToInt();
                var verseStart = m.Groups["verseStart"].Value.ToInt();

                var verseText = String.Empty;

                var link = $"<a href=\"/{translationName}/{numberOfBook}/{numberOfChapter}/{verseStart}\">{bookShortcut} {numberOfChapter}:{verseStart}</a>";
                if (verseText.IsNotNullOrEmpty()) {
                    return $"{link} - <i>{verseText}</i>";
                }
                return link;
            });
            return input;
        }
        public string GetExternalVerseText(string input, TranslationControllerModel model) {
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

        public string GetVerseTranslation(Session session, int numberOfBook, int numberOfChapter, int verseStart, int verseEnd = 0, string translationName = "NPI") {
            if (verseEnd == 0) {
                var index = $"{translationName}.{numberOfBook}.{numberOfChapter}.{verseStart}";
                var verse = new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
                if (verse.IsNotNull()) {
                    var verseText = verse.GetTranslationText();
                    if (verseText.IsNotNullOrWhiteSpace()) {
                        return verseText;
                    }
                    else {
                        return GetOtherVerseTranslation(session, numberOfBook, numberOfChapter, verseStart);
                    }
                }
                else {
                    return GetOtherVerseTranslation(session, numberOfBook, numberOfChapter, verseStart);
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
                        return GetOtherVersesTranslation(session, numberOfBook, numberOfChapter, verseStart, verseEnd);
                    }
                }
                else {
                    return GetOtherVersesTranslation(session, numberOfBook, numberOfChapter, verseStart, verseEnd);
                }
            }
        }
        private string GetOtherVersesTranslation(Session session, int numberOfBook, int numberOfChapter, int verseStart, int verseEnd) {
            IEnumerable<Verse> verses = null;
            ExpressionStarter<Verse> predicate = null;
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
            return String.Empty;
        }
        private string GetOtherVerseTranslation(Session session, int numberOfBook, int numberOfChapter, int verseStart) {
            var index = String.Empty;
            Verse verse = null;
            if (numberOfBook > 460) {
                index = $"PBPW.{numberOfBook}.{numberOfChapter}.{verseStart}";
                verse = new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
            }
            else {
                index = $"SNP18.{numberOfBook}.{numberOfChapter}.{verseStart}";
                verse = new XPQuery<Verse>(session).Where(x => x.Index == index).FirstOrDefault();
            }
            if (verse.IsNotNull()) {
                var verseText = verse.Text;
                verseText = System.Text.RegularExpressions.Regex.Replace(verseText, @"\[[0-9]+\]", "");

                return verseText;
            }
            return String.Empty;
        }
    }
}
