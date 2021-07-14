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

namespace Church.WebApp.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class DownloadInterlinearDocxController : Controller {
        private readonly IConfiguration Configuration;
        public DownloadInterlinearDocxController(IConfiguration configuration) {
            Configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            var qs = Request.QueryString;

            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 5) {
                var queryString = Uri.UnescapeDataString(qs.Value).RemoveAny("?q=");
                var stream = await CreateDocx(queryString);
                if (stream.IsNull()) { return NotFound(); }
                return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "file.docx");
            }

            return NotFound();
        }

        private async Task<Stream> CreateDocx(string queryString) {
            Book book;
            var paramsTable = queryString.Split(',');
            if (paramsTable.Length == 3) {
                Chapter chapter;

                var translationName = paramsTable[0];
                var bookNumber = paramsTable[1].ToInt();
                var chapterNumber = paramsTable[2].ToInt();

                var uow = new UnitOfWork();

                var trans = new XPQuery<Translation>(uow).Where(x => !x.Hidden && x.Name == translationName).FirstOrDefault();
                if (trans.IsNull()) { return default; }

                book = trans.Books.Where(x => x.NumberOfBook == bookNumber).FirstOrDefault();
                chapter = book.Chapters.Where(x => x.NumberOfChapter == chapterNumber).FirstOrDefault();

                var licPath = Configuration["AsposeLic"];
                var licInfo = new System.IO.FileInfo(licPath);
                byte[] licData = null;
                if (licInfo.Exists) {
                    licData = System.IO.File.ReadAllBytes(licPath);
                }

                var result = new InterlinearExporter(licData).Export(chapter, ExportSaveFormat.Docx);

                return new MemoryStream(result);
            }
            else if (paramsTable.Length == 2) {
                var translationName = paramsTable[0];
                var bookNumber = paramsTable[1].ToInt();

                var uow = new UnitOfWork();

                var trans = new XPQuery<Translation>(uow).Where(x => !x.Hidden && x.Name == translationName).FirstOrDefault();
                if (trans.IsNull()) { return default; }

                book = trans.Books.Where(x => x.NumberOfBook == bookNumber).FirstOrDefault();

                var licPath = Configuration["AsposeLic"];
                var licInfo = new System.IO.FileInfo(licPath);
                byte[] licData = null;
                if (licInfo.Exists) {
                    licData = System.IO.File.ReadAllBytes(licPath);
                }

                var result = new InterlinearExporter(licData).Export(book, ExportSaveFormat.Docx);

                return new MemoryStream(result);
            }

            return default;
        }
    }
}