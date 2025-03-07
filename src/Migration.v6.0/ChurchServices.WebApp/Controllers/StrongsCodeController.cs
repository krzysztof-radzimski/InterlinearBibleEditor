/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Controllers {
    public class StrongsCodeController : Controller {
        private readonly ILogger<StrongsCodeController> _logger;

        public StrongsCodeController(ILogger<StrongsCodeController> logger) {
            _logger = logger;
        }

        [Route("/[controller]/{code}")]
        public IActionResult Index(string code) {
            if (code.IsNotNullOrEmpty()) {
                var number = String.Empty;
                var lang = Language.Greek;
                if (code.StartsWith("g", StringComparison.CurrentCultureIgnoreCase)) {
                    number = code.Substring(1);
                }
                if (code.StartsWith("h", StringComparison.CurrentCultureIgnoreCase)) {
                    number = code.Substring(1);
                    lang = Language.Hebrew;
                }
                var id = number.ToInt();
                if (id > 0) {
                    var strongCode = new XPQuery<StrongCode>(new UnitOfWork()).Where(x => x.Code == id && x.Lang == lang).FirstOrDefault();
                    if (strongCode.IsNotNull()) {
                        return View(strongCode);
                    }
                }
            }
            return View();
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
                if (id > 0) {
                    var strongCode = new XPQuery<StrongCode>(new UnitOfWork()).Where(x => x.Code == id && x.Lang == lang).FirstOrDefault();
                    if (strongCode.IsNotNull()) {
                        return View(strongCode);
                    }
                }
            }
            return View();
        }
    }
}
