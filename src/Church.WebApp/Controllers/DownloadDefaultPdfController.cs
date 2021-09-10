using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Export;
using IBE.Data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Church.WebApp.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class DownloadDefaultPdfController : Controller {
        private readonly IConfiguration Configuration;
        public DownloadDefaultPdfController(IConfiguration configuration) {
            Configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            var qs = Request.QueryString;

            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 5) {
                var queryString = HttpUtility.UrlDecode(qs.Value).RemoveAny("?q=");
                Stream stream = null;
                try {
                    stream = await CreatePdf(queryString);
                }
                catch (AuthException) {
                    return new RedirectResult("/Account/Index?ReturnUrl=" + Request.Path + HttpUtility.UrlDecode(Request.QueryString.Value));
                }
                if (stream.IsNull()) { return NotFound(); }
                return File(stream, "application/pdf", "file.pdf");
            }

            return NotFound();
        }

        private async Task<Stream> CreatePdf(string queryString) {
            Book book;
            Chapter chapter;
            //string path;
            var paramsTable = queryString.Split(',');
            if (paramsTable.Length == 3) {
                var translationName = paramsTable[0];
                var bookNumber = paramsTable[1].ToInt();
                var chapterNumber = paramsTable[2].ToInt();

                var uow = new UnitOfWork();

                var trans = new XPQuery<Translation>(uow).Where(x => !x.Hidden && x.Name == translationName).FirstOrDefault();
                if (trans.IsNull() && translationName.EndsWith(" ")) {
                    translationName = translationName.Trim() + "+";
                    trans = new XPQuery<Translation>(uow).Where(x => x.Name == translationName).FirstOrDefault();
                }
                if (trans.IsNull()) { return default; }
                if (!trans.OpenAccess && !User.Identity.IsAuthenticated) {
                    throw new AuthException();
                }

                book = trans.Books.Where(x => x.NumberOfBook == bookNumber).FirstOrDefault();
                chapter = book.Chapters.Where(x => x.NumberOfChapter == chapterNumber).FirstOrDefault();

                var licPath = Configuration["AsposeLic"];
                var licInfo = new System.IO.FileInfo(licPath);
                byte[] licData = null;
                if (licInfo.Exists) {
                    licData = System.IO.File.ReadAllBytes(licPath);
                }

                var result = new DefaultExporter(licData).Export(chapter, ExportSaveFormat.Pdf);


                return new MemoryStream(result);
            }
            else if (paramsTable.Length == 2) {
                var translationName = paramsTable[0];
                var bookNumber = paramsTable[1].ToInt();

                var uow = new UnitOfWork();

                var trans = new XPQuery<Translation>(uow).Where(x => !x.Hidden && x.Name == translationName).FirstOrDefault();
                if (trans.IsNull() && translationName.EndsWith(" ")) {
                    translationName = translationName.Trim() + "+";
                    trans = new XPQuery<Translation>(uow).Where(x => x.Name == translationName).FirstOrDefault();
                }
                if (trans.IsNull()) { return default; }
                if (!trans.OpenAccess && !User.Identity.IsAuthenticated) {
                    throw new AuthException();
                }

                book = trans.Books.Where(x => x.NumberOfBook == bookNumber).FirstOrDefault();

                var licPath = Configuration["AsposeLic"];
                var licInfo = new System.IO.FileInfo(licPath);
                byte[] licData = null;
                if (licInfo.Exists) {
                    licData = System.IO.File.ReadAllBytes(licPath);
                }

                var result = new DefaultExporter(licData).Export(book, ExportSaveFormat.Pdf);


                return new MemoryStream(result);
            }

            return default;
        }
    }
}
