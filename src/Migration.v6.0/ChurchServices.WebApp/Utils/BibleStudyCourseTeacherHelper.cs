/*=====================================================================================

	Church Services
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project and website.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

namespace ChurchServices.WebApp.Utils {
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
            if (File.Exists(dataFilePath)) {
                var data = File.ReadAllText(dataFilePath);
                return JsonConvert.DeserializeObject<T>(data);
            }
            return default;
        }
    }
}
