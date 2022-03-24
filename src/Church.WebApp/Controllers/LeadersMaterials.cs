using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Church.WebApp.Controllers {
    public class LeadersMaterials : Controller {
        [Authorize]
        public IActionResult Index() {
            return View();
        }
    }
}
