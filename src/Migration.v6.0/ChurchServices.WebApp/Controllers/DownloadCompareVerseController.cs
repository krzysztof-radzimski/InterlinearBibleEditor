/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using System;

namespace ChurchServices.WebApp.Controllers {
    public abstract class DownloadCompareVerseController : Controller {
        protected readonly IConfiguration Configuration;
        protected readonly IBibleTagController BibleTag;
        protected readonly ITranslationInfoController TranslationInfoController;
        protected abstract ExportSaveFormat Format { get; }
        public DownloadCompareVerseController(IConfiguration configuration, IBibleTagController bibleTagController, ITranslationInfoController translationInfoController) {
            Configuration = configuration;
            BibleTag = bibleTagController;
            TranslationInfoController = translationInfoController;
        }

        private async Task<IActionResult> Get(CompareVerseModel model) {
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

        [Route("/api/[controller]/{trans}/{book}/{chapter}/{verse}")]
        public async Task<IActionResult> Get(string trans, int book, int chapter, int verse) {
            var model = GetModel(new VerseIndex(trans, book, chapter, verse));
            return await Get(model);            
        }

        [Route("/api/[controller]/{trans}/{book}/{chapter}/{verse}/{literal}")]
        public async Task<IActionResult> Get(string trans, int book, int chapter, int verse, bool literal) {
            var model = GetModel(new VerseIndex(trans, book, chapter, verse), literal);
            return await Get(model);
        }

        [HttpGet]
        public async Task<IActionResult> Get() {
            var qs = Request.QueryString;
            var model = GetModel(qs);
            return await Get(model);
        }

        private async Task<Stream> CreateStream(CompareVerseModel model) {
            var licData = await GetLicData();
            var host = (this.Request.IsHttps ? "https://" : "http://") + this.Request.Host;
            TranslationControllerModel transModel = null;
            using (var controller = new CompareVerseController(BibleTag, TranslationInfoController)) {
                transModel = controller.GetTranslationControllerModel();
            }
            var result = new CompareVersesExporter(licData, host).Export(model, Format, BibleTag, transModel);
            if (result.IsNotNull() && result.Length > 0) {
                return new MemoryStream(result);
            }

            return default;
        }

        private CompareVerseModel GetModel(VerseIndex index, bool literalOnly = false) {
            using (var controller = new CompareVerseController(BibleTag, TranslationInfoController)) {
                return controller.GetModel(index, literalOnly);
            }
        }
        private CompareVerseModel GetModel(QueryString qs) {
            using (var controller = new CompareVerseController(BibleTag, TranslationInfoController)) {
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
        public DownloadCompareVersePdfController(IConfiguration configuration, IBibleTagController bibleTagController, ITranslationInfoController translationInfoController) : base(configuration, bibleTagController, translationInfoController) { }
    }


    [ApiController]
    [Route("api/[controller]")]
    public class DownloadCompareVerseDocxController : DownloadCompareVerseController {
        protected override ExportSaveFormat Format => ExportSaveFormat.Docx;
        public DownloadCompareVerseDocxController(IConfiguration configuration, IBibleTagController bibleTagController, ITranslationInfoController translationInfoController) : base(configuration, bibleTagController, translationInfoController) { }              
    }
}
