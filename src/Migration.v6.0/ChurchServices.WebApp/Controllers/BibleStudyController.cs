/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using System.ComponentModel.DataAnnotations;
using System.Text;

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
                var dirPath = CreateDirectory(partialDir);
                if (dirPath != null) {
                    var partialFile = $"{partialDir}\\register.json";
                    if (FileDataExists(partialFile)) {
                        var _model = GetFileData<BibleStudyUserModel>(partialFile, "Nie znaleziono pliku!");
                        _model.CourseItem = model.CourseItem;
                        SaveFileData(_model, partialFile);

                        return View(_model);
                    }
                }
            }
            return View(new BibleStudyUserModel() { Data = "Nie znaleziono danych do zapisania!" });
        }
        public IActionResult SendCourseItem(BibleStudyUserModel model) {
            if (model != null) {
                var login = model.EmailAddress;
                var partialDir = $"download\\study\\{login.Replace("@", "_").Replace(".", "_")}";
                var dirPath = CreateDirectory(partialDir);
                if (dirPath != null) {
                    var partialFile = $"{partialDir}\\register.json";
                    if (FileDataExists(partialFile)) {
                        var _model = GetFileData<BibleStudyUserModel>(partialFile, "Nie znaleziono pliku!");
                        _model.CourseItem = model.CourseItem;
                        _model.CourseItem.Sent = true;
                        SaveFileData(_model, partialFile);


                        // wysłanie widomości do prowadzącego
                        partialDir = $"download\\study\\admin";
                        dirPath = CreateDirectory(partialDir);
                        partialDir = $"download\\study\\admin\\{login.Replace("@", "_").Replace(".", "_")}";
                        dirPath = CreateDirectory(partialDir);
                        partialFile = $"{partialDir}\\lesson{model.CourseLevel+1}.json";
                        SaveFileData(_model, partialFile);

                        // tu fajnie by było wygenerować worda

                        return View(_model);
                    }
                }

                

            }
            return View(new BibleStudyUserModel() { Data = "Nie znaleziono danych do zapisania!" });
        }

        public IActionResult Register() {
            if (HttpContext.User.Identity?.Name != null) {
                return OnRegisted();
            }
            return View(new BibleStudyUserModel() { Data = "Nie można wyświetlić danych studium. Uzytkownik nie jest zalogowany!" });
        }

        [HttpPost]
        public IActionResult Register(BibleStudyUserModel model) {
            if (model != null) {
                var partialDir = $"download\\study\\{model.EmailAddress.Replace("@", "_").Replace(".", "_")}";
                var dirPath = CreateDirectory(partialDir);
                if (dirPath != null) {
                    var partialFile = $"{partialDir}\\register.json";
                    if (FileDataExists(partialFile)) {
                        var _model = GetFileData<BibleStudyUserModel>(partialFile, "Nie znaleziono pliku!");
                        if (_model.Password != model.Password) {
                            return View(new BibleStudyUserModel() { Data = $"Nieprawidłowe hasło użytkownika {model.EmailAddress}!" });
                        }
                        MarkAsLogged(_model);

                        if (_model.CourseItem == null) {
                            _model.CourseItem = GetFileData<BibleStudyCourseModel>($"download\\study\\Lesson{_model.CourseLevel + 1}.json", "Nie znaleziono pliku lekcji!");
                            SaveFileData(_model, partialFile);
                        }

                        return View(_model);
                    }
                    else {
                        if (model.CourseItem == null) {
                            model.CourseItem = GetFileData<BibleStudyCourseModel>($"download\\study\\Lesson{model.CourseLevel + 1}.json", "Nie znaleziono pliku lekcji!");
                        }
                        SaveFileData(model, partialFile);
                        MarkAsLogged(model);
                        return View(model);
                    }
                }
            }
            else if (HttpContext.User.Identity?.Name != null) {
                return OnRegisted();
            }
            return NotFound();
        }

        private IActionResult OnRegisted() {
            var login = HttpContext.User.Identity?.Name;
            var partialDir = $"download\\study\\{login.Replace("@", "_").Replace(".", "_")}";
            var dirPath = CreateDirectory(partialDir);
            if (dirPath != null) {
                var partialFile = $"{partialDir}\\register.json";
                if (FileDataExists(partialFile)) {
                    var _model = GetFileData<BibleStudyUserModel>(partialFile, "Nie znaleziono pliku!");

                    if (_model.CourseItem == null) {
                        _model.CourseItem = GetFileData<BibleStudyCourseModel>($"download\\study\\Lesson{_model.CourseLevel + 1}.json", "Nie znaleziono pliku lekcji!");
                        SaveFileData(_model, partialFile);
                    }

                    return View(_model);
                }
            }
            return NotFound();
        }

        private void MarkAsLogged(BibleStudyUserModel model) {
            var userClaims = new List<Claim>()
                   {
                    new Claim(ClaimTypes.Name, model.EmailAddress)
                };

            var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");

            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
            HttpContext.SignInAsync(userPrincipal);
        }
    }


    public class BibleStudyUserModel : HtmlFileData {
        [Required]
        [EmailAddress]
        [Display(Name = "Adres email")]
        public string EmailAddress { get; set; }
        [Required]
        [MinLength(4)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }
        public int CourseLevel { get; set; }

        public BibleStudyCourseModel CourseItem { get; set; }
    }


    public class BibleStudyCourseModel : HtmlFileData {
        private int _chapter;
        private string _verses;
        public string Passage { get; set; }
        public string Introduction { get; set; }
        public string YouTubeLink { get; set; }
        public bool Sent {  get; set; }
        public List<BibleStudyCourseItemModel> Items { get; set; }

        [Display(Name = "Twoje myśli, uwagi")]
        [MaxLength(5000)]
        public string Description { get; set; } = string.Empty;

        public int GetChapter() {
            if (_chapter > 0) { return _chapter; }
            if (Passage != null && Passage.Contains(":")) {
                var t = Passage.Split(":");
                if (t.Length > 0) {
                    _chapter = t[0].ToInt();
                    return _chapter;
                }
            }
            return 1;
        }
        public string GetVerses() {
            if (_verses.IsNotNullOrEmpty()) { return _verses; }
            if (Passage != null && Passage.Contains(":")) {
                var t = Passage.Split(":");
                if (t.Length > 1) {
                    var s = t[1];
                    if (s.Contains("-")) {
                        var t2 = s.Split("-");
                        var start = t2[0].ToInt();
                        var end = t2[1].ToInt();
                        var sb = new StringBuilder();
                        for (int i = start; i <= end; i++) {
                            if (i < end) { sb.Append($"{i},"); }
                            else { sb.Append($"{i}"); }
                        }
                        s = sb.ToString();
                    }
                    _verses = s;
                    return _verses;
                }
            }
            return "1";
        }
    }
    public class BibleStudyCourseItemModel {
        public int Index { get; set; }
        public string Question { get; set; }
        public string QuestionAnswer { get; set; } = string.Empty;
    }
}
