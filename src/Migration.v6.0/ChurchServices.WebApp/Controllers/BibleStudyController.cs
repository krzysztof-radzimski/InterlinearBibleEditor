/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using Newtonsoft.Json;
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

        [Route("/BibleStudy/Register/{email}/{level}")]
        public IActionResult Register(string email, int level) {
            if (HttpContext.User.Identity?.Name != null && HttpContext.User.Identity.Name == "info@kosciol-jezusa.pl") {
                return OnRegisted(level, email);
            }
            return View(new BibleStudyUserModel() { Data = "Nie można wyświetlić danych studium. Uzytkownik nie jest zalogowany!" });
        }

        [Route("/BibleStudy/Register/{level}")]
        public IActionResult Register(int level = -1) {
            if (HttpContext.User.Identity?.Name != null) {
                return OnRegisted(level);
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
                    var lessonFile = $"{partialDir}\\lesson{model.CourseLevel + 1}.json";
                    var emptyLessonFile = $"download\\study\\Lesson{model.CourseLevel + 1}.json";
                    if (FileDataExists(partialFile)) {
                        var _model = GetFileData<BibleStudyUserModel>(partialFile, "Nie znaleziono pliku!");
                        if (_model.Password != model.Password) {
                            return View(new BibleStudyUserModel() { Data = $"Nieprawidłowe hasło użytkownika {model.EmailAddress}!" });
                        }
                        MarkAsLogged(_model);

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
                        SaveFileData(model, partialFile);

                        if (FileDataExists(lessonFile)) {
                            model.CourseItem = GetFileData<BibleStudyCourseModel>(lessonFile, "Nie znaleziono pliku lekcji!");
                        }
                        else {
                            model.CourseItem = GetFileData<BibleStudyCourseModel>(emptyLessonFile, "Nie znaleziono pliku lekcji!");
                            SaveFileData(model.CourseItem, lessonFile);
                        }

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

        private void MarkAsLogged(BibleStudyUserModel model) {
            var userClaims = new List<Claim>() {
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

    public class BibleStudyCourseInfoModel {
        public string EmailAddress { get; set; }
        public int CourseLevel { get; set; }
    }
    public enum BibleStudyCourseStaus {
        Edit = 0, Sent = 1, Approved = 2
    }
    public class BibleStudyCourseModel : HtmlFileData {
        private int _chapter;
        private string _verses;
        public string Passage { get; set; }
        public string Introduction { get; set; }
        public string YouTubeLink { get; set; }
        public BibleStudyCourseStaus Status { get; set; }
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
        public string TeacherComment { get; set; } = string.Empty;
    }

    public interface IBibleStudyCourseTeacherHelper {
        IEnumerable<BibleStudyCourseInfoModel> GetSentCourses();
        IEnumerable<BibleStudyCourseInfoModel> GetApprovedCourses(string userName = null);
    }
    public class BibleStudyCourseTeacherHelper : IBibleStudyCourseTeacherHelper {
        protected readonly IWebHostEnvironment _webHostEnvironment;
        public BibleStudyCourseTeacherHelper(IWebHostEnvironment webHostEnvironment) { _webHostEnvironment = webHostEnvironment; }
        public IEnumerable<BibleStudyCourseInfoModel> GetApprovedCourses(string userName = null) {
            var list = new List<BibleStudyCourseInfoModel>();
            var dirPath = Path.Combine(_webHostEnvironment.WebRootPath, "download\\study\\");
            var directory = new DirectoryInfo(dirPath);
            var dirs = directory.GetDirectories();
            foreach (var dir in dirs) {
                
                var u = GetFileData<BibleStudyUserModel>(Path.Combine(dir.FullName, "register.json"));
                if (u != null) {
                    if (userName.IsNotNullOrEmpty() && userName != u.EmailAddress) { continue; }
                    var files = dir.GetFiles("lesson*.json");

                    foreach (var file in files) {
                        var c = GetFileData<BibleStudyCourseModel>(file.FullName);
                        if (c != null && c.Status == BibleStudyCourseStaus.Approved) {
                            list.Add(new BibleStudyCourseInfoModel() {
                                CourseLevel = file.Name.Replace("lesson", "").Replace(".json", "").ToInt(),
                                EmailAddress = u.EmailAddress
                            });
                        }
                    }
                }
            }
            return list;
        }
        public IEnumerable<BibleStudyCourseInfoModel> GetSentCourses() {
            var list = new List<BibleStudyCourseInfoModel>();
            var dirPath = Path.Combine(_webHostEnvironment.WebRootPath, "download\\study\\");
            var directory = new DirectoryInfo(dirPath);
            var dirs = directory.GetDirectories();
            foreach (var dir in dirs) {
                var files = dir.GetFiles("lesson*.json");
                var u = GetFileData<BibleStudyUserModel>(Path.Combine(dir.FullName, "register.json"));
                foreach (var file in files) {
                    var c = GetFileData<BibleStudyCourseModel>(file.FullName);
                    if (c != null && c.Status == BibleStudyCourseStaus.Sent) {
                        list.Add(new BibleStudyCourseInfoModel() {
                            CourseLevel = file.Name.Replace("lesson", "").Replace(".json", "").ToInt(),
                            EmailAddress = u.EmailAddress
                        });
                    }
                }
            }
            return list;
        }

        protected T GetFileData<T>(string dataFilePath) where T : HtmlFileData, new() {
            if (System.IO.File.Exists(dataFilePath)) {
                var data = System.IO.File.ReadAllText(dataFilePath);
                return JsonConvert.DeserializeObject<T>(data);
            }
            return default;
        }
    }
}
