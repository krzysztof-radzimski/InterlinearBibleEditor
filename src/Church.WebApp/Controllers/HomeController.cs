/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using IBE.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace IBE.WebApp.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult WhatWeBelieve() {
            return View();
        }

        public IActionResult About() {
            return View();
        }

        public IActionResult Service() {
            return View();
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
