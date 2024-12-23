﻿/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using IBE.Data.Model;
using IBE.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using IBE.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Church.WebApp.Utils;

namespace Church.WebApp.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> Logger;
        private readonly ITranslationInfoController TranslationInfoController;
        public HomeController(ILogger<HomeController> logger, ITranslationInfoController translationInfoController) {
            Logger = logger;
            TranslationInfoController = translationInfoController;
        }

        public IActionResult Index() {
            return View(TranslationInfoController.GetLastFourArticles());
            //var articles = new XPQuery<Article>(new UnitOfWork()).Where(x => !x.Hidden).OrderByDescending(x => x.Date).Take(4);
            //var list = new List<ArticleInfo>();
            //foreach (var item in articles) {
            //    list.Add(new ArticleInfo() {
            //        AuthorName = item.AuthorName,
            //        AuthorPicture = item.AuthorPicture.IsNotNull() ? Convert.ToBase64String(item.AuthorPicture) : String.Empty,
            //        Date = item.Date,
            //        Id = item.Oid,
            //        Lead = item.Lead,
            //        Subject = item.Subject,
            //        Type = item.Type.GetDescription()
            //    });
            //}
            //return View(list);
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

        [Authorize]
        public IActionResult Secured() {
            return View();
        }

        public IActionResult AboutBible() {
            var uow = new UnitOfWork();
            var books = TranslationInfoController.GetBookBases(uow);
            var translation = new XPQuery<Translation>(uow).Where(x => x.Name.Replace("'", "").Replace("+", "").ToLower() == "UBG18".ToLower()).FirstOrDefault();
            return View(new IBE.Data.Export.Model.TranslationControllerModel(translation, books: books));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
