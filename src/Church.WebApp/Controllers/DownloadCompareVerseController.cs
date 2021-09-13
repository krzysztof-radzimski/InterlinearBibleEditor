using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Export;
using IBE.Data.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Church.WebApp.Controllers {
    public abstract class DownloadCompareVerseController : Controller {
        protected readonly IConfiguration Configuration;
        protected abstract ExportSaveFormat Format { get; }
        public DownloadCompareVerseController(IConfiguration configuration) {
            Configuration = configuration;
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

            var result = new CompareVersesExporter(licData).Export(model, Format);
            if (result.IsNotNull() && result.Length > 0) {
                return new MemoryStream(result);
            }

            return default;
        }

        private async Task<CompareVerseModel> GetModel(QueryString qs) {
            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 3) {
                var value = qs.Value;
                var onlyLiteral = value.Contains("literal");
                if (value.Contains("&")) {
                    value = value.Substring(0, value.IndexOf("&"));
                }
                var id = value.Replace("?id=", "").Trim();
                var vi = new VerseIndex(id);
                var verses = new XPQuery<Verse>(new UnitOfWork())
                        .Where(x => x.Index.EndsWith($".{vi.NumberOfBook}.{vi.NumberOfChapter}.{vi.NumberOfVerse}") && x.Text != null && x.Text != "" && !x.ParentChapter.ParentBook.ParentTranslation.Hidden && (onlyLiteral ? (x.ParentChapter.ParentBook.ParentTranslation.Type == TranslationType.Literal || x.ParentChapter.ParentBook.ParentTranslation.Type == TranslationType.Interlinear) : true))
                        .OrderBy(x => x.ParentChapter.ParentBook.ParentTranslation.Type)
                        .ToList();
                return new CompareVerseModel() { Index = vi, Verses = verses };
            }
            return default;
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
        public DownloadCompareVersePdfController(IConfiguration configuration) : base(configuration) { }
    }


    [ApiController]
    [Route("api/[controller]")]
    public class DownloadCompareVerseDocxController : DownloadCompareVerseController {
        protected override ExportSaveFormat Format => ExportSaveFormat.Docx;
        public DownloadCompareVerseDocxController(IConfiguration configuration) : base(configuration) { }
    }
}
