using Newtonsoft.Json;

namespace ChurchServices.WebApp.Controllers {
    public abstract class ChurchControllerBase : Controller {
        protected readonly IWebHostEnvironment _webHostEnvironment;
        public ChurchControllerBase(IWebHostEnvironment webHostEnvironment) {
            _webHostEnvironment = webHostEnvironment;
        }
        protected HtmlFileData GetFileData(string partialPath, string errorText) {
            var dataFilePath = Path.Combine(_webHostEnvironment.WebRootPath, partialPath);
            if (System.IO.File.Exists(dataFilePath)) {
                var data = System.IO.File.ReadAllText(dataFilePath);
                return new HtmlFileData() { Data = data };
            }
            return new HtmlFileData() { Data = $"<p>{errorText}</p>" };
        }

        protected bool FileDataExists(string partialPath) {
            var dataFilePath = Path.Combine(_webHostEnvironment.WebRootPath, partialPath);
            return System.IO.File.Exists(dataFilePath);
        }

        protected T GetFileData<T>(string partialPath, string errorText) where T : HtmlFileData, new() {
            var dataFilePath = Path.Combine(_webHostEnvironment.WebRootPath, partialPath);
            if (System.IO.File.Exists(dataFilePath)) {
                var data = System.IO.File.ReadAllText(dataFilePath);
                return JsonConvert.DeserializeObject<T>(data);
            }
            return new T() { Data = $"<p>{errorText}</p>" };
        }

        protected void SaveFileData<T>(T data, string partialPath) where T : class {
            if (data != null) {
                var dataFilePath = Path.Combine(_webHostEnvironment.WebRootPath, partialPath);
                var json = JsonConvert.SerializeObject(data);
                System.IO.File.WriteAllText(dataFilePath, json);
            }
        }

        protected string CreateDirectory(string partialPath) {
            var dirPath = Path.Combine(_webHostEnvironment.WebRootPath, partialPath);
            if (!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }
            return dirPath;
        }
    }

    public class HtmlFileData { public string Data { get; set; } }
}
