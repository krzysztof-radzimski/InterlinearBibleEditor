/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Controllers {
    public class BibleStudyController : ChurchControllerBase {

        public BibleStudyController(IWebHostEnvironment webHostEnvironment) : base(webHostEnvironment) { }

        public IActionResult Signup() {
            return View();
        }
        public IActionResult SaveCourseItem(BibleStudyUserModel model) {
            if (model != null) {
                var login = model.EmailAddress;
                var partialDir = $"download\\study\\{login.Replace("@", "_").Replace(".", "_")}";
                var lessonFile = $"{partialDir}\\lesson{model.CourseLevel + 1}.json";
                var dirPath = CreateDirectory(partialDir);
                if (dirPath != null) {
                    SaveFileData(model.CourseItem, lessonFile);
                    return View(model);
                }
            }
            return View(new BibleStudyUserModel() { Data = "Nie znaleziono danych do zapisania!" });
        }
        public IActionResult SendCourseItem(BibleStudyUserModel model) {
            if (model != null) {
                var login = model.EmailAddress;
                var partialDir = $"download\\study\\{login.Replace("@", "_").Replace(".", "_")}";
                var lessonFile = $"{partialDir}\\lesson{model.CourseLevel + 1}.json";
                var dirPath = CreateDirectory(partialDir);
                if (dirPath != null) {
                    model.CourseItem.Status = BibleStudyCourseStaus.Sent;
                    SaveFileData(model.CourseItem, lessonFile);
                    return View(model);
                }
            }
            return View(new BibleStudyUserModel() { Data = "Nie znaleziono danych do zapisania!" });
        }
        public IActionResult ApprovedCourseItem(BibleStudyUserModel model) {
            if (model != null) {
                var login = model.EmailAddress;
                var partialDir = $"download\\study\\{login.Replace("@", "_").Replace(".", "_")}";
                var partialFile = $"{partialDir}\\register.json";
                var _model = GetFileData<BibleStudyUserModel>(partialFile, "Nie znaleziono pliku!");
                if (_model != null) {
                    _model.CourseLevel += 1;
                    SaveFileData(_model, partialFile);
                }

                var lessonFile = $"{partialDir}\\lesson{model.CourseLevel + 1}.json";
                var dirPath = CreateDirectory(partialDir);
                if (dirPath != null) {
                    model.CourseItem.Status = BibleStudyCourseStaus.Approved;
                    SaveFileData(model.CourseItem, lessonFile);
                    return View(model);
                }
            }
            return View(new BibleStudyUserModel() { Data = "Nie znaleziono danych do zapisania!" });
        }

        [Route("/[controller]/Index/{email}/{level}")]
        public IActionResult Index(string email, int level) {
            if (HttpContext.User.Identity?.Name != null) {
                return OnRegisted(level, email);
            }
            return View(new BibleStudyUserModel() { Data = "Nie można wyświetlić danych studium. Uzytkownik nie jest zalogowany!" });
        }

        [Route("/[controller]/Index/{level}")]
        public IActionResult Index(int level = -1) {
            if (HttpContext.User.Identity?.Name != null) {
                return OnRegisted(level);
            }
            return View(new BibleStudyUserModel() { Data = "Nie można wyświetlić danych studium. Uzytkownik nie jest zalogowany!" });
        }

        [HttpPost]
        public async Task<IActionResult> Index(BibleStudyUserModel model) {
            if (model != null) {
                var partialDir = $"download\\study\\{model.EmailAddress.Replace("@", "_").Replace(".", "_")}";
                var dirPath = CreateDirectory(partialDir);
                if (dirPath != null) {
                    var partialFile = $"{partialDir}\\register.json";
                    var lessonFile = $"{partialDir}\\lesson{model.CourseLevel + 1}.json";
                    var emptyLessonFile = $"download\\study\\Lesson{model.CourseLevel + 1}.json";
                    if (FileDataExists(partialFile)) {
                        var _model = GetFileData<BibleStudyUserModel>(partialFile, "Nie znaleziono pliku!");
                        lessonFile = $"{partialDir}\\lesson{_model.CourseLevel + 1}.json";
                        emptyLessonFile = $"download\\study\\Lesson{_model.CourseLevel + 1}.json";

                        if (_model.Password != model.Password) {
                            return View(new BibleStudyUserModel() { Data = $"Nieprawidłowe hasło użytkownika {model.EmailAddress}!" });
                        }
                        await MarkAsLogged(_model);
                        if (model.EmailAddress == "info@kosciol-jezusa.pl") {
                            return View(model);
                        }

                        if (FileDataExists(lessonFile)) {
                            _model.CourseItem = GetFileData<BibleStudyCourseModel>(lessonFile, "Nie znaleziono pliku lekcji!");
                        }
                        else {
                            _model.CourseItem = GetFileData<BibleStudyCourseModel>(emptyLessonFile, "Nie znaleziono pliku lekcji!");
                            SaveFileData(_model.CourseItem, lessonFile);
                        }

                        return View(_model);
                    }
                    else {
                        model.CourseItem = null;
                        SaveFileData(model, partialFile);

                        if (FileDataExists(lessonFile)) {
                            model.CourseItem = GetFileData<BibleStudyCourseModel>(lessonFile, "Nie znaleziono pliku lekcji!");
                        }
                        else {
                            model.CourseItem = GetFileData<BibleStudyCourseModel>(emptyLessonFile, "Nie znaleziono pliku lekcji!");
                            SaveFileData(model.CourseItem, lessonFile);
                        }

                        await MarkAsLogged(model);
                        return View(model);
                    }
                }
            }
            else if (HttpContext.User.Identity?.Name != null) {
                return OnRegisted();
            }
            return NotFound();
        }

        private IActionResult OnRegisted(int level = -1, string login = null) {
            if (login.IsNullOrEmpty()) { login = HttpContext.User.Identity?.Name; }
            var partialDir = $"download\\study\\{login.Replace("@", "_").Replace(".", "_")}";
            var dirPath = CreateDirectory(partialDir);
            if (dirPath != null) {
                var partialFile = $"{partialDir}\\register.json";
                if (FileDataExists(partialFile)) {
                    var _model = GetFileData<BibleStudyUserModel>(partialFile, "Nie znaleziono pliku!");
                    if (_model != null) {
                        if (level == -1) { level = _model.CourseLevel + 1; }
                        var lessonFile = $"{partialDir}\\lesson{level}.json";
                        var emptyLessonFile = $"download\\study\\Lesson{level}.json";

                        if (FileDataExists(lessonFile)) {
                            _model.CourseItem = GetFileData<BibleStudyCourseModel>(lessonFile, "Nie znaleziono pliku lekcji!");
                        }
                        else {
                            _model.CourseItem = GetFileData<BibleStudyCourseModel>(emptyLessonFile, "Nie znaleziono pliku lekcji!");
                            SaveFileData(_model.CourseItem, lessonFile);
                        }

                        return View(_model);
                    }
                }
            }
            return NotFound();
        }

        private async Task MarkAsLogged(BibleStudyUserModel model) {
            if (HttpContext.User != null && HttpContext.User.Identity != null && HttpContext.User.Identity.Name.IsNotNullOrEmpty()) {
                await HttpContext.SignOutAsync();
                HttpContext.User = null;
            }
            var userClaims = new List<Claim>() {
                new Claim(ClaimTypes.Name, model.EmailAddress)
            };
            var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
            await HttpContext.SignInAsync(userPrincipal);
            if (HttpContext.User.Identity.Name == null) { HttpContext.User = userPrincipal; }
        }
    }
}
