using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;


namespace Church.WebApp.Controllers {
    public class CompareVerseController : Controller {
        public IActionResult Index() {
            var qs = Request.QueryString;
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 3) {
                var value = qs.Value;
                if (value.Contains("&")) {
                    value = value.Substring(0, value.IndexOf("&"));
                }
                var id = value.Replace("?id=", "").Trim();
                var vi = new VerseIndex(id);
                var verses = new XPQuery<Verse>(new UnitOfWork()).Where(x => x.Index.EndsWith($".{vi.NumberOfBook}.{vi.NumberOfChapter}.{vi.NumberOfVerse}") && x.Text != null && x.Text != "" && !x.ParentChapter.ParentBook.ParentTranslation.Hidden).OrderBy(x => x.ParentChapter.ParentBook.ParentTranslation.Type).ToList();
                return View(new CompareVerseModel() { Index = vi, Verses = verses });

            }
            return View();
        }
    }

    public class CompareVerseModel {
        public VerseIndex Index { get; set; }
        public List<Verse> Verses { get; set; }
    }
}
