using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Export;
using IBE.Data.Export.Controllers;
using IBE.Data.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Church.WebApp.Controllers {
    public class CompareVerseController : Controller {
        protected readonly IBibleTagController BibleTag;
        public CompareVerseController(IBibleTagController bibleTagController) { BibleTag = bibleTagController; }
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
                var viewBaseBook = new XPView(uow, typeof(BookBase)) {
                    CriteriaString = $"NumberOfBook == {vi.NumberOfBook}"
                };
                viewBaseBook.Properties.Add(new ViewProperty("Oid", SortDirection.None, "[Oid]", false, true));
                viewBaseBook.Properties.Add(new ViewProperty("BookShortcut", SortDirection.None, "[BookShortcut]", false, true));
                viewBaseBook.Properties.Add(new ViewProperty("BookName", SortDirection.None, "[BookName]", false, true));
                viewBaseBook.Properties.Add(new ViewProperty("BiblePart", SortDirection.None, "[Status.BiblePart]", false, true));
                if (viewBaseBook.Count > 0) {
                    foreach (ViewRecord item in viewBaseBook) {
                        baseBookShortcut = item["BookShortcut"].ToString();
                        baseBookName = item["BookName"].ToString();
                        biblePart = (BiblePart)item["BiblePart"];
                    }
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
                                .Replace("<J>", "<span style='color: darkred;'>")
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
