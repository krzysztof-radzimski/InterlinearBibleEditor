using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Church.WebApp.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class UrlShortenerController : ControllerBase {
        private readonly IConfiguration Configuration;
        public UrlShortenerController(IConfiguration configuration) {
            Configuration = configuration;
        }

        [HttpGet]
        public ActionResult<string> Get() {
            var url = GetReqParam("url");
            var host = this.Request.Host.ToString();
            var hostPrefix = Request.IsHttps ? "https://" : "http://";
            if (url.IsNotNullOrEmpty()) {
                var uow = new UnitOfWork();
                var _url = new XPQuery<UrlShort>(uow).Where(x => x.Url == url).FirstOrDefault();
                if (_url.IsNull()) {
                    _url = new UrlShort(uow) {
                        Url = url,
                        ShortUrl = GetShortUrl(uow)
                    };
                    _url.Save();
                    uow.CommitChanges();
                }

                return $"{hostPrefix}{host}/{_url.ShortUrl}";
            }
            return url;
        }

        private string GetShortUrl(UnitOfWork uow) {
            var id = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 5);
            var result = new XPQuery<UrlShort>(uow).Where(x => x.ShortUrl == id).FirstOrDefault();
            if (result.IsNotNull()) {
                return GetShortUrl(uow);
            }
            return id;

        }

        private string GetReqParam(string paramName) {
            var qs = Request.QueryString;
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 3) {
                var value = Uri.UnescapeDataString(qs.Value).RemoveAny($"?{paramName}=");

                var id = Encoding.UTF8.GetString(Convert.FromBase64String(value));
                return id;
            }
            return default;
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class UrlShortListController : ControllerBase {
        private readonly IConfiguration Configuration;
        public UrlShortListController(IConfiguration configuration) {
            Configuration = configuration;
        }

        [HttpGet]
        public List<UrlShortInfo> Get() {
            var uow = new UnitOfWork();
            var data = new XPQuery<UrlShort>(uow).ToList();
            var result = new List<UrlShortInfo>();
            data.ForEach(x => {
                result.Add(new UrlShortInfo() {
                    Short = x.ShortUrl,
                    Url = x.Url
                });
            });
            return result;
        }
    }


}
