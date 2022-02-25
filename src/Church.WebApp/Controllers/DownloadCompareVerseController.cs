using IBE.Common.Extensions;
using IBE.Data.Export;
using IBE.Data.Export.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace Church.WebApp.Controllers {
    public abstract class DownloadCompareVerseController : Controller {
        protected readonly IConfiguration Configuration;
        protected readonly IBibleTagController BibleTag;
        protected abstract ExportSaveFormat Format { get; }
        public DownloadCompareVerseController(IConfiguration configuration, IBibleTagController bibleTagController) {
            Configuration = configuration;
            BibleTag = bibleTagController;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            var qs = Request.QueryString;
            var model = await GetModel(qs);
            if (model.IsNotNull()) {
                Stream stream;
                try {
                    stream = await CreateStream(model);
                }
                catch (AuthException) {
                    return new RedirectResult("/Account/Index?ReturnUrl=" + Request.Path + HttpUtility.UrlDecode(Request.QueryString.Value));
                }
                if (stream.IsNull()) { return NotFound(); }
                return File(stream, Format.GetDescription(), Format.GetCategory());
            }
            return NotFound();
        }

        private async Task<Stream> CreateStream(CompareVerseModel model) {
            var licData = await GetLicData();
            var host = (this.Request.IsHttps ? "https://" : "http://") + this.Request.Host;
            var result = new CompareVersesExporter(licData, host).Export(model, Format);
            if (result.IsNotNull() && result.Length > 0) {
                return new MemoryStream(result);
            }

            return default;
        }

        private async Task<CompareVerseModel> GetModel(QueryString qs) {
            using (var controller = new CompareVerseController(BibleTag)) {
                return controller.GetModel(qs);
            }
        }

        private async Task<byte[]> GetLicData() {
            var licPath = Configuration["AsposeLic"];
            var licInfo = new System.IO.FileInfo(licPath);

            if (licInfo.Exists) {
                return await System.IO.File.ReadAllBytesAsync(licPath);
            }
            return default;
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class DownloadCompareVersePdfController : DownloadCompareVerseController {
        protected override ExportSaveFormat Format => ExportSaveFormat.Pdf;
        public DownloadCompareVersePdfController(IConfiguration configuration, IBibleTagController bibleTagController) : base(configuration, bibleTagController) { }
    }


    [ApiController]
    [Route("api/[controller]")]
    public class DownloadCompareVerseDocxController : DownloadCompareVerseController {
        protected override ExportSaveFormat Format => ExportSaveFormat.Docx;
        public DownloadCompareVerseDocxController(IConfiguration configuration, IBibleTagController bibleTagController) : base(configuration, bibleTagController) { }
    }
}
