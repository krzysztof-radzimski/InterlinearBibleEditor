using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IBE.Common.Extensions;
using DevExpress.Xpo;
using IBE.Data.Model;

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
                var id = value.ToLower().Replace("?id=", "").Trim().ToInt();
                var strongCode = new XPQuery<StrongCode>(new UnitOfWork()).Where(x => x.Code == id && x.Lang == Language.Greek).FirstOrDefault();
                if (strongCode.IsNotNull()) {
                    return View(strongCode);
                }
            }
            return View();
        }
    }
}
