/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2025 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using System.IO.Compression;

namespace ChurchServices.Data.Import {
    public abstract class BaseImporter<T> where T : class, new() {
        protected const string INTERNAL_SEPARATOR = "/";
        protected const string WINDOWS_SEPARATOR = "\\";
        protected const char HARD_SPACE = '\u00A0';

        protected string ExtractAndGetFirstArchiveItemFilePath(string zipFilePath) {
            if (!zipFilePath.ToLower().EndsWith(".zip")) {
                throw new Exception("The specified file is not a zip archive!");
            }

            var zipFiles = GetAllFiles(zipFilePath);
            if (zipFiles == null || !zipFiles.Any()) {
                throw new Exception("The specified file is not a valid zip archive!");
            }

            var a = zipFiles.First();
            var fileName = Path.Combine(Path.GetTempPath(), a.FileName);
            File.WriteAllBytes(fileName, a.FileData);
            return fileName;
        }

        protected IEnumerable<ArchiveItem> GetAllFiles(string zipFilePath) {
            if (string.IsNullOrEmpty(zipFilePath)) {
                throw new ArgumentNullException(nameof(zipFilePath));
            }

            List<ArchiveItem> files = new();

            using (ZipArchive zip = ZipFile.OpenRead(zipFilePath)) {
                foreach (var entry in zip.Entries) {
                    using (var stream = entry.Open()) {
                        using (var ms = new MemoryStream()) {
                            stream.CopyTo(ms);
                            files.Add(new ArchiveItem() {
                                FileName = entry.FullName,
                                FileData = ms.ToArray()
                            });
                        }
                    }
                }
            }

            return files;
        }

        protected IEnumerable<ArchiveItem> GetFiles(string zipFilePath, string internalPath, string extension) {
            if (string.IsNullOrEmpty(zipFilePath)) {
                throw new ArgumentNullException(nameof(zipFilePath));
            }

            internalPath = internalPath?.Replace(WINDOWS_SEPARATOR, INTERNAL_SEPARATOR);

            var files = new List<ArchiveItem>();

            using (ZipArchive zip = ZipFile.OpenRead(zipFilePath)) {
                var entries = zip.Entries.Where(entry => string.IsNullOrEmpty(internalPath) || entry.FullName.Contains(internalPath));

                if (!string.IsNullOrEmpty(extension)) {
                    entries = entries.Where(entry => Path.GetExtension(entry.FullName).Equals($".{extension}", StringComparison.OrdinalIgnoreCase));
                }

                foreach (var entry in entries) {
                    using (var stream = entry.Open()) {
                        using (var ms = new MemoryStream()) {
                            stream.CopyTo(ms);
                            files.Add(new ArchiveItem() {
                                FileName = entry.FullName,
                                FileData = ms.ToArray()
                            });
                        }
                    }
                }
            }

            return files;
        }

        protected IEnumerable<string> FindPattern(string zipFilePath, string pattern) {
            var foundFiles = new List<string>();
            var regex = new System.Text.RegularExpressions.Regex(pattern);

            using (ZipArchive zip = ZipFile.OpenRead(zipFilePath)) {
                foundFiles.AddRange(zip.Entries.Where(entry => regex.IsMatch(entry.FullName)).Select(entry => entry.FullName));
            }

            return foundFiles;
        }

        protected bool FileExists(string zipFilePath, string internalPath) {
            if (string.IsNullOrEmpty(zipFilePath)) {
                throw new ArgumentNullException(nameof(zipFilePath));
            }

            if (!File.Exists(zipFilePath)) {
                throw new FileNotFoundException("File not found", zipFilePath);
            }

            if (new FileInfo(zipFilePath).Length == 0) {
                throw new Exception($"File {zipFilePath} is empty");
            }

            if (string.IsNullOrEmpty(internalPath)) {
                throw new ArgumentNullException(nameof(internalPath));
            }

            internalPath = internalPath.Replace(WINDOWS_SEPARATOR, INTERNAL_SEPARATOR);

            using (ZipArchive zip = ZipFile.OpenRead(zipFilePath)) {
                return zip.Entries.Any(entry => entry.FullName == internalPath);
            }
        }

        protected byte[] GetFileData(string zipFilePath, string internalPath) {
            if (string.IsNullOrEmpty(zipFilePath)) {
                throw new ArgumentNullException(nameof(zipFilePath));
            }
            if (string.IsNullOrEmpty(internalPath)) {
                throw new ArgumentNullException(nameof(internalPath));
            }

            internalPath = internalPath.Replace(WINDOWS_SEPARATOR, INTERNAL_SEPARATOR);

            using (ZipArchive zip = ZipFile.OpenRead(zipFilePath)) {
                var entry = zip.Entries.FirstOrDefault(e => e.FullName == internalPath);
                if (entry == null) {
                    return null;
                }
                using (var stream = entry.Open()) {
                    using (var ms = new MemoryStream()) {
                        stream.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            }
        }

        public abstract T Import(string zipFilePath, UnitOfWork uow);

        public void Dispose() { }
    }

    public sealed class ArchiveItem {
        public string FileName { get; internal set; }
        public byte[] FileData { get; internal set; }
    }
}


//using ChurchServices.Extensions;
//using DevExpress.Xpo;
//using Ionic.Zip;
//using System.Text;

//namespace ChurchServices.Data.Import {
//    public abstract class BaseImporter<T> where T : class, new() {
//        protected const string INTERNAL_SEPARATOR = "/";
//        protected const string WINDOWS_SEPARATOR = "\\";
//        protected const char HARD_SPACE = '\u00A0';
//        protected string ExtractAndGetFirstArchiveItemFilePath(string zipFilePath) {
//            if (!zipFilePath.ToLower().EndsWith(".zip")) { throw new Exception("The specified file is not a zip archive!"); }
//            var zipFiles = GetAllFiles(zipFilePath);
//            if (zipFilePath.IsNull() || zipFilePath.Length == 0) { throw new Exception("The specified file is not a valid zip archive!"); }
//            var a = zipFiles.First();
//            var fileName = Path.Combine(Path.GetTempPath(), a.FileName);
//            File.WriteAllBytes(fileName, a.FileData);
//            return fileName;
//        }
//        protected IEnumerable<ArchiveItem> GetAllFiles(string zipFilePath) {
//            if (String.IsNullOrEmpty(zipFilePath)) {
//                throw new ArgumentNullException("zipFilePath");
//            }

//            List<ArchiveItem> l = new List<ArchiveItem>();

//            using (var zip = new ZipFile(zipFilePath)) {
//                foreach (var e in zip) {
//                    MemoryStream m = new MemoryStream();
//                    e.Extract(m);
//                    l.Add(new ArchiveItem() {
//                        FileName = e.FileName,
//                        FileData = m.ToArray()
//                    });
//                }
//            }

//            return l;
//        }
//        protected IEnumerable<ArchiveItem> GetFiles(string zipFilePath, string internalPath, string extension) {
//            if (String.IsNullOrEmpty(zipFilePath)) {
//                throw new ArgumentNullException("zipFilePath");
//            }
//            if (String.IsNullOrEmpty(internalPath)) {
//                // ok :)
//            }
//            else {
//                internalPath = internalPath.Replace(WINDOWS_SEPARATOR, INTERNAL_SEPARATOR);
//            }

//            var l = new List<ArchiveItem>();

//            using (var zip = new ZipFile(zipFilePath)) {
//                IEnumerable<ZipEntry> entries = null;
//                if (!String.IsNullOrEmpty(internalPath)) {
//                    var ee = zip.Where(xe => (!String.IsNullOrEmpty(xe.FileName)) && xe.FileName.Contains(internalPath));
//                    if (!String.IsNullOrEmpty(extension)) {
//                        entries = ee.Where(xx => Path.GetExtension(xx.FileName).Replace(".", "").Trim().ToLower() == extension.Trim().ToLower());
//                    }
//                    else {
//                        entries = ee;
//                    }
//                }
//                else {
//                    entries = zip.Where(xe => String.IsNullOrEmpty(extension) ? true : Path.GetExtension(xe.FileName).Contains(extension));
//                }

//                foreach (var e in entries) {
//                    var m = new MemoryStream();
//                    e.Extract(m);
//                    l.Add(new ArchiveItem() {
//                        FileName = e.FileName,
//                        FileData = m.ToArray()
//                    });
//                }
//            }

//            return l;
//        }
//        protected IEnumerable<string> FindPattern(string zipFilePath, string pattern) {
//            var l = new List<string>();
//            var reg = new System.Text.RegularExpressions.Regex(pattern);
//            using (ZipFile zip = new ZipFile(zipFilePath)) {
//                var q = zip.Where(xe => xe.FileName != null && reg.IsMatch(xe.FileName)).Select(xe => xe.FileName);
//                if (q.Count() > 0) {
//                    l.AddRange(q);
//                }
//            }

//            return l;
//        }
//        protected bool FileExists(string zipFilePath, string internalPath) {
//            if (String.IsNullOrEmpty(zipFilePath)) {
//                throw new ArgumentNullException("zipFilePath");
//            }

//            if (!File.Exists(zipFilePath)) {
//                throw new FileNotFoundException("Nie znaleziono pliku", zipFilePath);
//            }

//            if (new FileInfo(zipFilePath).Length == 0) {
//                throw new Exception(String.Format("Plik {0} nie zawiera danych", zipFilePath));
//            }

//            if (String.IsNullOrEmpty(internalPath)) {
//                throw new ArgumentNullException("internalPath");
//            }
//            else {
//                internalPath = internalPath.Replace(WINDOWS_SEPARATOR, INTERNAL_SEPARATOR);
//            }

//            try {
//                using (ZipFile zip = new ZipFile(zipFilePath)) {
//                    return zip[internalPath] != null;
//                }
//            }
//            catch { }

//            return false;
//        }
//        protected byte[] GetFileData(string zipFilePath, string internalPath) {
//            return GetFileData(zipFilePath, internalPath, null);
//        }
//        protected byte[] GetFileData(string zipFilePath, string internalPath, string password) {
//            if (String.IsNullOrEmpty(zipFilePath)) {
//                throw new ArgumentNullException("zipFilePath");
//            }
//            if (String.IsNullOrEmpty(internalPath)) {
//                throw new ArgumentNullException("internalPath");
//            }
//            else {
//                internalPath = internalPath.Replace(WINDOWS_SEPARATOR, INTERNAL_SEPARATOR);
//            }
//            byte[] b = null;

//            using (var zip = new ZipFile(zipFilePath)) {
//                if (zip[internalPath] != null) {
//                    var m = ExctractToMemoryStream(internalPath, password, zip);
//                    b = m.ToArray();
//                }
//            }

//            if (b == null) {
//                using (ZipFile zip = new ZipFile(zipFilePath, Encoding.Default)) {
//                    if (zip[internalPath] != null) {
//                        var m = ExctractToMemoryStream(internalPath, password, zip);
//                        b = m.ToArray();
//                    }
//                }
//            }

//            if (b == null) {
//                try {
//                    var enc = Encoding.GetEncoding("Cp852");
//                    if (enc != null) {
//                        using (ZipFile zip = new ZipFile(zipFilePath, enc)) {
//                            if (zip[internalPath] != null) {
//                                var m = ExctractToMemoryStream(internalPath, password, zip);
//                                b = m.ToArray();
//                            }
//                        }
//                    }
//                }
//                catch { }//CP1250
//            }

//            if (b == null) {
//                try {
//                    var enc = Encoding.GetEncoding("Windows-1250");
//                    if (enc != null) {
//                        using (ZipFile zip = new ZipFile(zipFilePath, enc)) {
//                            if (zip[internalPath] != null) {
//                                var m = ExctractToMemoryStream(internalPath, password, zip);
//                                b = m.ToArray();
//                            }
//                        }
//                    }
//                }
//                catch { }
//            }

//            if (b == null) {
//                try {
//                    var enc = Encoding.GetEncoding("Windows-1252");
//                    if (enc != null) {
//                        using (ZipFile zip = new ZipFile(zipFilePath, enc)) {
//                            if (zip[internalPath] != null) {
//                                var m = ExctractToMemoryStream(internalPath, password, zip);
//                                b = m.ToArray();
//                            }
//                        }
//                    }
//                }
//                catch { }
//            }

//            if (b == null) {
//                using (ZipFile zip = new ZipFile(zipFilePath, Encoding.ASCII)) {
//                    if (zip[internalPath] != null) {
//                        var m = ExctractToMemoryStream(internalPath, password, zip);
//                        b = m.ToArray();
//                    }
//                }
//            }


//            if (b == null) {
//                using (ZipFile zip = new ZipFile(zipFilePath, Encoding.UTF8)) {
//                    if (zip[internalPath] != null) {
//                        var m = ExctractToMemoryStream(internalPath, password, zip);
//                        b = m.ToArray();
//                    }
//                }
//            }

//            return b;
//        }

//        private MemoryStream ExctractToMemoryStream(string internalPath, string password, ZipFile zip) {
//            MemoryStream m = new MemoryStream();
//            if (String.IsNullOrEmpty(password)) {
//                zip[internalPath].Extract(m);
//            }
//            else {
//                zip[internalPath].ExtractWithPassword(m, password);
//            }
//            return m;
//        }

//        public abstract T Import(string zipFilePath, UnitOfWork uow);

//        public void Dispose() { }
//    }

//    public sealed class ArchiveItem {
//        public string FileName { get; internal set; }
//        public byte[] FileData { get; internal set; }
//    }
//}
