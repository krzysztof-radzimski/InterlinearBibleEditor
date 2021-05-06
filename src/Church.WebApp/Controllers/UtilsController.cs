using IBE.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Church.WebApp.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class UtilsController : ControllerBase {
        private readonly IConfiguration Configuration;
        public UtilsController(IConfiguration configuration) {
            Configuration = configuration;
        }

        [HttpGet]
        public ActionResult<bool> Get() {
            //try {
            //    var callingUrl = Request.Headers["Referer"].ToString();
            //    var isLocal = Url.IsLocalUrl(callingUrl);
            //    if (isLocal) { return true; }
            //}
            //catch { }

            var qs = Request.QueryString;
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 9) {
                var queryString = Uri.UnescapeDataString(qs.Value);
                var pwd = queryString.Replace("?pwd=", "").Trim();
                var password = Configuration["TranslationPwd"];
                if (pwd == password) { return true; }
            }
            return false;
        }
    }
}
