﻿using DevExpress.Xpo;
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
    public class DownloadInterlinearTranslationPdfController :Controller{
        private readonly IConfiguration Configuration;
        public DownloadInterlinearTranslationPdfController(IConfiguration configuration) {
            Configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            var qs = Request.QueryString;

            if (qs.IsNotNull() && qs.Value.IsNotNullOrEmpty() && qs.Value.Length > 5) {
                var queryString = Uri.UnescapeDataString(qs.Value).RemoveAny("?q=");
                var stream = await CreatePdf(queryString);
                if (stream.IsNull()) { return NotFound(); }
                return File(stream, "application/pdf", "file.pdf");
            }

            return NotFound();
        }

        private async Task<Stream> CreatePdf(string queryString) {
            Book book;
            var paramsTable = queryString.Split(',');
            if (paramsTable.Length == 2) {
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

                var result = new InterlinearExporter(licData).ExportBookTranslation(book, ExportSaveFormat.Pdf);

                return new MemoryStream(result);
            }
            return default;
        }
    }
}