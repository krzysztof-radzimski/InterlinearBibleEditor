/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using System.Text;

namespace ChurchServices.WebApp.Controllers {
    public abstract class JsonControllerBase : Controller {
        protected readonly IConfiguration Configuration;
        public JsonControllerBase(IConfiguration configuration) {
            Configuration = configuration;
        }
        protected IActionResult FileJson(object data, string fileDownloadName) {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var fileContents = Encoding.UTF8.GetBytes(json);
            return File(fileContents, "application/json", fileDownloadName: fileDownloadName);
        }
    }
}
