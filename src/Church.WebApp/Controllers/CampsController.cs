using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Church.WebApp.Controllers {
    [Route("/oboz")]
    [Route("/obozy")]
    [Route("/camps")]
    public class CampsController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
