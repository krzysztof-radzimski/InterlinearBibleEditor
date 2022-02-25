using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Church.WebApp.Controllers {
    public class StrongsCodeController : Controller {
        private readonly ILogger<StrongsCodeController> _logger;

        public StrongsCodeController(ILogger<StrongsCodeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            var qs = Request.QueryString;
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 3) {
                var value = qs.Value;
                if (value.Contains("&")) {
                    value = value.Substring(0, value.IndexOf("&"));
                }
                var lang = Language.Greek;
                var _id = value.ToLower().Replace("?id=", "").Trim();
                if (_id.StartsWith("g", StringComparison.CurrentCultureIgnoreCase)) {
                    _id = _id.Substring(1);
                }
                if (_id.StartsWith("h", StringComparison.CurrentCultureIgnoreCase)) {
                    _id = _id.Substring(1);
                    lang = Language.Hebrew;
                }
                var id = _id.ToInt();
                var strongCode = new XPQuery<StrongCode>(new UnitOfWork()).Where(x => x.Code == id && x.Lang == lang).FirstOrDefault();
                if (strongCode.IsNotNull()) {
                    return View(strongCode);
                }
            }
            return View();
        }
    }
}
