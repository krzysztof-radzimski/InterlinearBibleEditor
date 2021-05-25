using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Church.WebApp.Controllers {
    public class GrammarsCodeController : Controller {
        public IActionResult Index() {
            var qs = Request.QueryString;
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 3) {
                var value = qs.Value;
                if (value.Contains("&")) {
                    value = value.Substring(0, value.IndexOf("&"));
                }
                var id = value.Replace("?id=", "").Trim();
                var grammarCode = new XPQuery<GrammarCode>(new UnitOfWork()).Where(x => x.GrammarCodeVariant1 == id).FirstOrDefault();
                if (grammarCode.IsNotNull()) {
                    return View(grammarCode);
                }
            }
            return View();
        }
    }
}
