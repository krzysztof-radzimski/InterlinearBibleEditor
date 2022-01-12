using DevExpress.XtraEditors;
using System;
using System.Linq;
using WBST.Bibliography.Model;

namespace WBST.Bibliography.Controllers {
    public interface IBibliographyFootnoteController {
        Microsoft.Office.Interop.Word.Document Document { get; }
        void InsertFootNote(BibliographySource item);
    }
    public class BibliographyFootnoteController : IBibliographyFootnoteController {
        public Microsoft.Office.Interop.Word.Document Document { get; }

        private BibliographyFootnoteController() { }
        public BibliographyFootnoteController(Microsoft.Office.Interop.Word.Document doc) : this() {
            Document = doc;
        }
        public void InsertFootNote(BibliographySource item) {
            FootnoteTitleType type = FootnoteTitleType.Default;           
            if (item != null) {
                var footNote = Document.Footnotes.Add(Document.Range(Document.ActiveWindow.Selection.Start, Document.ActiveWindow.Selection.End), Text: "");
                footNote.Range.Select();

                // Tamże
                if (footNote.Index > 1) {
                    var f = Document.Footnotes[footNote.Index - 1];
                    if (f.Range.Text.Contains(item.Title) || (!String.IsNullOrEmpty(item.ShortTitle) && f.Range.Text.Contains(item.ShortTitle))) {
                        type = FootnoteTitleType.Next;
                    }
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
                                if ((author as BibliographyNameList).People != null) {
                                    foreach (var person in (author as BibliographyNameList).People) {
                                        var s = "";
                                        if (!String.IsNullOrEmpty(person.Middle)) { s += $"{person.Middle.Substring(0, 1).ToUpper()}."; }
                                        if (!String.IsNullOrEmpty(person.First)) { s += $"{person.First.Substring(0, 1).ToUpper()}."; }

                                        if (item.SourceType == SourceTypeEnum.ArticleInAPeriodical || item.SourceType == SourceTypeEnum.JournalArticle) {
                                            if (person == (author as BibliographyNameList).People.Last()) {
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
                            else {
                                Document.ActiveWindow.Selection.TypeText($"{author}, ");
                            }
                        }
                    }
                }

                // Tytuł       
                Document.ActiveWindow.Selection.Font.Italic = 1;
                if (type == FootnoteTitleType.Short) {
                    if (!String.IsNullOrEmpty(item.ShortTitle)) {
                        Document.ActiveWindow.Selection.TypeText(item.ShortTitle + "…");
                    }
                    else {
                        string shortTitle;
                        if (item.Title.Length > 10) {
                            shortTitle = item.Title.Substring(0, 10) + "…";
                            item.ShortTitle = item.Title.Substring(0, 10);
                        }
                        else {
                            shortTitle = item.Title;
                        }
                        Document.ActiveWindow.Selection.TypeText(shortTitle);
                    }
                }
                else if (type == FootnoteTitleType.Next) {
                    Document.ActiveWindow.Selection.Font.Italic = 0;
                    Document.ActiveWindow.Selection.TypeText("Tamże");
                }
                else { Document.ActiveWindow.Selection.TypeText(item.Title); }

                Document.ActiveWindow.Selection.Font.Italic = 0;
                Document.ActiveWindow.Selection.TypeText(", ");

                // Nazwa magazynu
                if (!String.IsNullOrEmpty(item.JournalName)) {
                    Document.ActiveWindow.Selection.TypeText("w: ");
                    Document.ActiveWindow.Selection.Font.Italic = 1;
                    Document.ActiveWindow.Selection.TypeText(item.JournalName);
                    Document.ActiveWindow.Selection.Font.Italic = 0;
                    Document.ActiveWindow.Selection.TypeText(", ");
                }

                // Numer magazynu
                if (!String.IsNullOrEmpty(item.Issue)) {
                    Document.ActiveWindow.Selection.TypeText($"nr. {item.Issue}, ");
                }

                // strony w magazynie, artykule
                if (!String.IsNullOrEmpty(item.Pages)) {
                    Document.ActiveWindow.Selection.TypeText($"str. {item.Pages}, ");
                }

                // Wydanie
                if (!String.IsNullOrEmpty(item.Edition)) {
                    Document.ActiveWindow.Selection.TypeText($"wyd. {item.Edition}, ");
                }

                // Tom
                if (!String.IsNullOrEmpty(item.Volume)) {
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

                    if (item.Author.Translator != null && item.Author.Translator.Objects != null && item.Author.Translator.Objects.Count > 0) {
                        Document.ActiveWindow.Selection.TypeText("przekł. ");
                        foreach (var author in item.Author.Translator.Objects) {
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

                    if (!String.IsNullOrEmpty(item.Publisher)) {
                        Document.ActiveWindow.Selection.TypeText($"{item.Publisher}, ");
                    }
                    if (!String.IsNullOrEmpty(item.City)) {
                        Document.ActiveWindow.Selection.TypeText($"{item.City}");
                    }
                    if (!String.IsNullOrEmpty(item.Year) && item.SourceType != SourceTypeEnum.JournalArticle && item.SourceType != SourceTypeEnum.ArticleInAPeriodical) {
                        Document.ActiveWindow.Selection.TypeText($"{item.Year}");
                    }
                    if (!String.IsNullOrEmpty(item.City) || (!String.IsNullOrEmpty(item.Year) && item.SourceType != SourceTypeEnum.JournalArticle && item.SourceType != SourceTypeEnum.ArticleInAPeriodical)) {
                        Document.ActiveWindow.Selection.TypeText(", ");
                    }
                }

                if (item.SourceType != SourceTypeEnum.InternetSite) {
                    var s = XtraInputBox.Show("Podaj numery stron:", "Numery stron", "");
                    if (!String.IsNullOrEmpty(s)) {
                        Document.ActiveWindow.Selection.TypeText($"s. {s}");
                    }
                }

                Document.ActiveWindow.Selection.TypeText(".");
            }
        }
    }
}
