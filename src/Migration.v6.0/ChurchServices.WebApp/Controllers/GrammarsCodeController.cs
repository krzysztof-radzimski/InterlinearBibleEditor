/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Controllers {
    public class GrammarsCodeController : Controller {
        [Route("/[controller]/{code}")]
        public IActionResult Index(string code) {
            if (code.IsNotNullOrEmpty()) {
                var grammarCode = new XPQuery<GrammarCode>(new UnitOfWork()).Where(x => x.GrammarCodeVariant1 == code).FirstOrDefault();
                if (grammarCode.IsNotNull()) {
                    return View(grammarCode);
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
                var id = value.Replace("?id=", "").Trim();
                var grammarCode = new XPQuery<GrammarCode>(new UnitOfWork()).Where(x => x.GrammarCodeVariant1 == id).FirstOrDefault();
                if (grammarCode.IsNotNull()) {
                    return View(grammarCode);
                }
            }
            return View();
        }
    }
}
