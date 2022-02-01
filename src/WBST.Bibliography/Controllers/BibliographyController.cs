using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using WBST.Bibliography.Model;

namespace WBST.Bibliography.Controllers {
    public interface IBibliographyController {
        Microsoft.Office.Interop.Word.Document Document { get; }
        void InsertFootNote(BibliographySource item);
        void AppendBibliography(IEnumerable<BibliographySource> sources);
    }
    public class BibliographyController : IBibliographyController {
        private object missing = System.Reflection.Missing.Value;
        public Microsoft.Office.Interop.Word.Document Document { get; }

        private BibliographyController() { }
        public BibliographyController(Microsoft.Office.Interop.Word.Document doc) : this() {
            Document = doc;
        }

        private bool IsNextFootnote(BibliographySource item, int footnoteIndex) {
            if (footnoteIndex > 0) {
                var f = Document.Footnotes[footnoteIndex];
                var fText = f.Range.Text;
                if (String.IsNullOrEmpty(item.ShortTitle)) {
                    GetShortTitle(item);
                }
                if (fText.Contains(item.Title) || (!String.IsNullOrEmpty(item.ShortTitle) && fText.Contains(item.ShortTitle))) {
                    return true;
                }
                else if (fText.Contains("Tamże")) {
                    return IsNextFootnote(item, footnoteIndex - 1);
                }
            }
            return false;
        }

        public void InsertFootNote(BibliographySource item) {
            FootnoteTitleType type = FootnoteTitleType.Default;
            if (item != null) {
                var footNote = Document.Footnotes.Add(Document.Range(Document.ActiveWindow.Selection.Start, Document.ActiveWindow.Selection.End), Text: "");
                footNote.Range.Select();

                // Tamże
                if (footNote.Index > 1) {
                    var isNext = IsNextFootnote(item, footNote.Index - 1);
                    if (isNext) { type = FootnoteTitleType.Next; }
                }

                // Jeżeli nie Tamże
                if (type != FootnoteTitleType.Next) {
                    foreach (Microsoft.Office.Interop.Word.Footnote f in Document.Footnotes) {
                        if (f.Range.Text != null && f.Range.Text.Contains(item.Title)) {
                            type = FootnoteTitleType.Short;
                            break;
                        }
                    }


                    // Autor
                    if (item.Author != null && item.Author.Author != null && item.Author.Author.Objects != null) {
                        foreach (var author in item.Author.Author.Objects) {
                            if (author is BibliographyNameList) {
                                AddAuthorFootnoteText(item, author as BibliographyNameList);
                            }
                            else {
                                Document.ActiveWindow.Selection.TypeText($"{author}, ");
                            }
                        }
                    }
                }

                // Tytuł                      
                if (type == FootnoteTitleType.Short) {
                    Document.ActiveWindow.Selection.Font.Italic = 1;
                    var shortTitle = GetShortTitle(item);
                    if (!String.IsNullOrEmpty(shortTitle)) {
                        Document.ActiveWindow.Selection.TypeText(shortTitle);
                    }
                    Document.ActiveWindow.Selection.Font.Italic = 0;
                }
                else if (type == FootnoteTitleType.Next) {
                    Document.ActiveWindow.Selection.Font.Italic = 0;
                    Document.ActiveWindow.Selection.TypeText("Tamże");
                }
                else {
                    Document.ActiveWindow.Selection.Font.Italic = 1;
                    Document.ActiveWindow.Selection.TypeText(item.Title);
                    Document.ActiveWindow.Selection.Font.Italic = 0;
                    Document.ActiveWindow.Selection.TypeText(", ");
                }

                // Nazwa magazynu
                if (!String.IsNullOrEmpty(item.JournalName) && type != FootnoteTitleType.Next) {
                    Document.ActiveWindow.Selection.TypeText("w: ");
                    Document.ActiveWindow.Selection.Font.Italic = 1;
                    Document.ActiveWindow.Selection.TypeText(item.JournalName);
                    Document.ActiveWindow.Selection.Font.Italic = 0;
                    Document.ActiveWindow.Selection.TypeText(", ");
                }

                // Numer magazynu
                if (!String.IsNullOrEmpty(item.Issue) && type != FootnoteTitleType.Next) {
                    Document.ActiveWindow.Selection.TypeText($"nr. {item.Issue}, ");
                }

                // strony w magazynie, artykule
                if (!String.IsNullOrEmpty(item.Pages) && type != FootnoteTitleType.Next) {
                    Document.ActiveWindow.Selection.TypeText($"str. {item.Pages}, ");
                }

                // Wydanie
                if (!String.IsNullOrEmpty(item.Edition) && type != FootnoteTitleType.Next) {
                    Document.ActiveWindow.Selection.TypeText($"wyd. {item.Edition}, ");
                }

                // Tom
                if (!String.IsNullOrEmpty(item.Volume) && type != FootnoteTitleType.Next) {
                    Document.ActiveWindow.Selection.TypeText($"t. {item.Volume}");
                    if (!String.IsNullOrEmpty(item.NumberVolumes)) {
                        Document.ActiveWindow.Selection.TypeText($" z {item.NumberVolumes}");
                    }
                    Document.ActiveWindow.Selection.TypeText(", ");
                }

                // URL i dostęp
                if (type == FootnoteTitleType.Default) {
                    if (item.SourceType == SourceTypeEnum.InternetSite && !String.IsNullOrEmpty(item.URL)) {
                        Document.ActiveWindow.Selection.TypeText($"online: {item.URL}");
                        if (!String.IsNullOrEmpty(item.Access)) {
                            Document.ActiveWindow.Selection.TypeText($" ({item.Access})");
                        }
                        Document.ActiveWindow.Selection.TypeText(", ");
                    }

                    if (item.Author.Editor != null && item.Author.Editor.Objects != null && item.Author.Editor.Objects.Count > 0) {
                        Document.ActiveWindow.Selection.TypeText("red. ");
                        foreach (var author in item.Author.Editor.Objects) {
                            if (author is BibliographyNameList) {
                                AddAuthorFootnoteText(item, author as BibliographyNameList);
                            }
                            else {
                                Document.ActiveWindow.Selection.TypeText($"{author}, ");
                            }
                        }
                    }

                    if (item.Author.Translator != null && item.Author.Translator.Objects != null && item.Author.Translator.Objects.Count > 0) {
                        Document.ActiveWindow.Selection.TypeText("przekł. ");
                        foreach (var author in item.Author.Translator.Objects) {
                            if (author is BibliographyNameList) {
                                AddAuthorFootnoteText(item, author as BibliographyNameList);
                            }
                            else {
                                Document.ActiveWindow.Selection.TypeText($"{author}, ");
                            }
                        }
                    }

                    if (!String.IsNullOrEmpty(item.Publisher)) {
                        Document.ActiveWindow.Selection.TypeText($"{item.Publisher}, ");
                    }
                    if (!String.IsNullOrEmpty(item.City)) {
                        Document.ActiveWindow.Selection.TypeText($"{item.City}");
                    }
                    if (!String.IsNullOrEmpty(item.Year) && item.SourceType != SourceTypeEnum.JournalArticle && item.SourceType != SourceTypeEnum.ArticleInAPeriodical) {
                        if (!String.IsNullOrEmpty(item.City)) { Document.ActiveWindow.Selection.TypeText(" "); }
                        Document.ActiveWindow.Selection.TypeText($"{item.Year}");
                    }
                }

                if (item.SourceType != SourceTypeEnum.InternetSite) {
                    var s = XtraInputBox.Show("Podaj numery stron:", "Numery stron", "");
                    if (!String.IsNullOrEmpty(s)) {
                        if (!String.IsNullOrEmpty(item.City) || (!String.IsNullOrEmpty(item.Year) && item.SourceType != SourceTypeEnum.JournalArticle && item.SourceType != SourceTypeEnum.ArticleInAPeriodical)) {
                            Document.ActiveWindow.Selection.TypeText(", ");
                        }
                        Document.ActiveWindow.Selection.TypeText($"s. {s}");
                    }
                }

                Document.ActiveWindow.Selection.TypeText(".");
            }
        }

        private string GetShortTitle(BibliographySource item) {
            var shortTitle = item.ShortTitle;
            if (!String.IsNullOrEmpty(shortTitle)) { shortTitle += "…"; }

            if (String.IsNullOrEmpty(shortTitle) && !String.IsNullOrEmpty(item.Title)) {
                if (item.Title.Length > 10) {
                    shortTitle = item.Title.Substring(0, 10) + "…";
                    item.ShortTitle = item.Title.Substring(0, 10);
                }
                else {
                    shortTitle = item.Title + "…";
                    item.ShortTitle = item.Title;
                }
            }
            return shortTitle;
        }

        public void AppendBibliography(IEnumerable<BibliographySource> sources) {
            if (sources.IsNotNullOrMissing()) {

                var groupByComments = sources.Where(x => x.Comments.IsNotNullOrEmpty()).Any();
                if (groupByComments) {
                    var groups = sources.GroupBy(x => x.Comments).OrderBy(x => x.Key);
                    foreach (var group in groups) {
                        AddHeader(2, group.Key);

                        foreach (var item in group) {
                            AddBibliographyItem(item);
                        }
                    }
                }
                else {
                    foreach (var item in sources) {
                        AddBibliographyItem(item);
                    }
                }
            }
        }

        private void AddAuthorFootnoteText(BibliographySource item, BibliographyNameList author) {
            if (author != null) {
                if (author.People != null) {
                    foreach (var person in author.People) {
                        var s = "";
                        if (!String.IsNullOrEmpty(person.First)) { s += $"{person.First.Substring(0, 1).ToUpper()}."; }
                        if (!String.IsNullOrEmpty(person.Middle)) { s += $"{person.Middle.Substring(0, 1).ToUpper()}."; }

                        if (item.SourceType == SourceTypeEnum.ArticleInAPeriodical || item.SourceType == SourceTypeEnum.JournalArticle) {
                            if (person == author.People.Last()) {
                                var ym = "";

                                if (!String.IsNullOrEmpty(item.Month)) {
                                    ym += item.Month;
                                }
                                if (!String.IsNullOrEmpty(item.Year)) {
                                    if (ym != "") { ym += "."; }
                                    ym += item.Year;
                                }
                                s += $" {person.Last} ({ym}), ";
                            }
                            else {
                                s += $" {person.Last}, ";
                            }
                        }
                        else { s += $" {person.Last}, "; }
                        Document.ActiveWindow.Selection.TypeText(s);
                    }
                }
            }
        }

        private void AddBibliographyItem(BibliographySource source) {
            // Autor
            if (source.Author != null && source.Author.Author != null && source.Author.Author.Objects != null) {
                foreach (var author in source.Author.Author.Objects) {
                    if (author is BibliographyNameList) {
                        if ((author as BibliographyNameList).People != null) {
                            foreach (var person in (author as BibliographyNameList).People) {
                                var s = "";
                                if (!String.IsNullOrEmpty(person.First)) { s += $"{person.First}"; }
                                if (!String.IsNullOrEmpty(person.Middle)) { s += $" {person.Middle}"; }

                                if (source.SourceType == SourceTypeEnum.ArticleInAPeriodical || source.SourceType == SourceTypeEnum.JournalArticle) {
                                    if (person == (author as BibliographyNameList).People.Last()) {
                                        var ym = "";

                                        if (!String.IsNullOrEmpty(source.Month)) {
                                            ym += source.Month;
                                        }
                                        if (!String.IsNullOrEmpty(source.Year)) {
                                            if (ym != "") { ym += "."; }
                                            ym += source.Year;
                                        }
                                        s += $" {person.Last} ({ym}), ";
                                    }
                                    else {
                                        s += $" {person.Last}, ";
                                    }
                                }
                                else { s += $" {person.Last}, "; }
                                Document.ActiveWindow.Selection.TypeText(s);
                            }
                        }
                    }
                    else {
                        Document.ActiveWindow.Selection.TypeText($"{author}, ");
                    }
                }
            }

            // Tytuł       
            Document.ActiveWindow.Selection.Font.Italic = 1;
            Document.ActiveWindow.Selection.TypeText(source.Title);

            Document.ActiveWindow.Selection.Font.Italic = 0;
            Document.ActiveWindow.Selection.TypeText(", ");

            // Nazwa magazynu
            if (!String.IsNullOrEmpty(source.JournalName)) {
                Document.ActiveWindow.Selection.TypeText("w: ");
                Document.ActiveWindow.Selection.Font.Italic = 1;
                Document.ActiveWindow.Selection.TypeText(source.JournalName);
                Document.ActiveWindow.Selection.Font.Italic = 0;
                Document.ActiveWindow.Selection.TypeText(", ");
            }

            // Numer magazynu
            if (!String.IsNullOrEmpty(source.Issue)) {
                Document.ActiveWindow.Selection.TypeText($"nr. {source.Issue}, ");
            }

            // strony w magazynie, artykule
            if (!String.IsNullOrEmpty(source.Pages)) {
                Document.ActiveWindow.Selection.TypeText($"str. {source.Pages}, ");
            }

            // Wydanie
            if (!String.IsNullOrEmpty(source.Edition)) {
                Document.ActiveWindow.Selection.TypeText($"wyd. {source.Edition}, ");
            }

            // Tom
            if (!String.IsNullOrEmpty(source.Volume)) {
                Document.ActiveWindow.Selection.TypeText($"t. {source.Volume}");
                if (!String.IsNullOrEmpty(source.NumberVolumes)) {
                    Document.ActiveWindow.Selection.TypeText($" z {source.NumberVolumes}");
                }
                Document.ActiveWindow.Selection.TypeText(", ");
            }

            // URL i dostęp
            if (source.SourceType == SourceTypeEnum.InternetSite && !String.IsNullOrEmpty(source.URL)) {
                Document.ActiveWindow.Selection.TypeText($"online: {source.URL}");
                if (!String.IsNullOrEmpty(source.Access)) {
                    Document.ActiveWindow.Selection.TypeText($" ({source.Access})");
                }
                Document.ActiveWindow.Selection.TypeText(", ");
            }

            if (source.Author != null && source.Author.Editor != null && source.Author.Editor.Objects != null && source.Author.Editor.Objects.Count > 0) {
                Document.ActiveWindow.Selection.TypeText("red. ");
                foreach (var author in source.Author.Editor.Objects) {
                    if (author is BibliographyNameList) {
                        if ((author as BibliographyNameList).People != null) {
                            foreach (var person in (author as BibliographyNameList).People) {
                                var s = "";
                                if (!String.IsNullOrEmpty(person.Middle)) { s += $" {person.Middle.Substring(0, 1).ToUpper()}."; }
                                if (!String.IsNullOrEmpty(person.First)) { s += $" {person.First.Substring(0, 1).ToUpper()}."; }
                                s += $"{person.Last}, ";
                                Document.ActiveWindow.Selection.TypeText(s);
                            }
                        }
                    }
                    else {
                        Document.ActiveWindow.Selection.TypeText($"{author}, ");
                    }
                }
            }

            if (source.Author != null && source.Author.Translator != null && source.Author.Translator.Objects != null && source.Author.Translator.Objects.Count > 0) {
                Document.ActiveWindow.Selection.TypeText("przekł. ");
                foreach (var author in source.Author.Translator.Objects) {
                    if (author is BibliographyNameList) {
                        if ((author as BibliographyNameList).People != null) {
                            foreach (var person in (author as BibliographyNameList).People) {
                                var s = "";
                                if (!String.IsNullOrEmpty(person.Middle)) { s += $" {person.Middle.Substring(0, 1).ToUpper()}."; }
                                if (!String.IsNullOrEmpty(person.First)) { s += $" {person.First.Substring(0, 1).ToUpper()}."; }
                                s += $"{person.Last}, ";
                                Document.ActiveWindow.Selection.TypeText(s);
                            }
                        }
                    }
                    else {
                        Document.ActiveWindow.Selection.TypeText($"{author}, ");
                    }
                }
            }

            if (!String.IsNullOrEmpty(source.Publisher)) {
                Document.ActiveWindow.Selection.TypeText($"{source.Publisher}, ");
            }
            if (!String.IsNullOrEmpty(source.City)) {
                Document.ActiveWindow.Selection.TypeText($"{source.City}");
            }
            if (!String.IsNullOrEmpty(source.Year) && source.SourceType != SourceTypeEnum.JournalArticle && source.SourceType != SourceTypeEnum.ArticleInAPeriodical) {
                if (!String.IsNullOrEmpty(source.City)) { Document.ActiveWindow.Selection.TypeText(" "); }
                Document.ActiveWindow.Selection.TypeText($"{source.Year}");
            }

            Document.ActiveWindow.Selection.TypeText(".");
            Document.ActiveWindow.Selection.TypeText("\r\n");
        }

        private void AddHeader(int level, string text) {
            var start = Document.ActiveWindow.Selection.Range.End;
            Document.ActiveWindow.Selection.TypeText(text);
            var rng = Document.Range(start, Document.ActiveWindow.Selection.Range.End);
            object styleName = $"Naglowek{level}Prosty";
            rng.set_Style(ref styleName);
            Document.ActiveWindow.Selection.TypeText("\r\n");
            object nStyle = "Normalny";
            Document.ActiveWindow.Selection.set_Style(nStyle);
            //Microsoft.Office.Interop.Word.Paragraph para1 = Document.Content.Paragraphs.Add(ref missing);
            //object styleName = $"Nagłówek {level}";
            //para1.Range.set_Style(ref styleName);
            //para1.Range.Text = text;
            //para1.Range.InsertParagraphAfter();
        }
    }
}
