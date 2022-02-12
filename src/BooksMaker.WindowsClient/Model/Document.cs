using DevExpress.XtraEditors;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BooksMaker.WindowsClient.Model {
    public class Document {
        public const string FILTER = "BooksMaker File (*.bmf)|*.bmf";
        public string FileName { get; private set; } = null;
        public DocumentProperties Properties { get; set; }
        public DocumentTitlePage TitlePage { get; set; }
        public DocumentIntroduction Introduction { get; set; }
        public List<DocumentSection> Sections { get; set; }
        public DocumentSummary Summary { get; set; }
        public DocumentBibliography Bibliography { get; set; }

        public void LoadDocument(string filePath) {
            if (File.Exists(filePath)) {
                using var zip = new ZipFile(filePath);

                FileName = filePath;

                foreach (var fileName in zip.EntryFileNames) {
                    if (fileName == "Properties.xml") {
                        Properties = GetFile<DocumentProperties>(zip, fileName);
                    }
                    else if (fileName == "TitlePage.xml") {
                        TitlePage = GetFile<DocumentTitlePage>(zip, fileName);
                    }
                    else if (fileName == "Introduction.xml") {
                        Introduction = GetFile<DocumentIntroduction>(zip, fileName);
                    }
                    else if (fileName == "Summary.xml") {
                        Summary = GetFile<DocumentSummary>(zip, fileName);
                    }
                    else if (fileName == "Bibliography.xml") {
                        Bibliography = GetFile<DocumentBibliography>(zip, fileName);
                    }
                    else {
                        if (Sections == null) { Sections = new List<DocumentSection>(); }
                        Sections.Add(GetFile<DocumentSection>(zip, fileName));
                    }
                }
            }
        }

        public void SaveDocument(string filePath = null) {
            if (filePath != null) {
                FileName = filePath;
            }

            if (FileName == null) {
                using var dlg = new XtraSaveFileDialog() {
                    Filter = FILTER
                };
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    FileName = dlg.FileName;
                }
            }

            if (FileName == null) { throw new ArgumentNullException("filePath"); }

            using var zip = new ZipFile();
            AddToZip(Properties, zip);
            AddToZip(TitlePage, zip);
            AddToZip(Introduction, zip);
            AddToZip(Summary, zip);
            AddToZip(Bibliography, zip);
            if (Sections != null) {
                var i = 1;
                foreach (var section in Sections) {
                    AddToZip(section, zip, $"Section{i}.xml");
                }
            }
            if (File.Exists(filePath)) { try { File.Delete(filePath); } catch { } }
            zip.Save(filePath);
        }

        public void InitNewFile() {
            Properties = new DocumentProperties { NameOfPart = "Document Properties" };
            TitlePage = new DocumentTitlePage { NameOfPart = "Document Title Page" };
            Introduction = new DocumentIntroduction { NameOfPart = "Document Introduction" };
            Summary = new DocumentSummary { NameOfPart = "Document Summary" };
            Bibliography = new DocumentBibliography { NameOfPart = "Document Bibliography" };
            Sections = new List<DocumentSection>();
        }

        private T GetFile<T>(ZipFile zip, string fileName) where T : class, new() {
            var serializer = new XmlSerializer(typeof(T));

            var entry = zip.Entries.Where(x => x.FileName == fileName).FirstOrDefault();
            if (entry != null) {
                var ms = new MemoryStream();
                entry.Extract(ms);
                return serializer.Deserialize(ms) as T;
            }
            return default;
        }

        private void AddToZip<T>(T o, ZipFile zip, string fileName = null) where T : IBaseDocumentPart, new() {
            if (o != null) {
                if (fileName == null) { fileName = o.DefaultFileName; }

                var serializer = new XmlSerializer(o.GetType());
                var stringWriter = new Utf8StringWriter();
                using var writer = System.Xml.XmlWriter.Create(stringWriter, new System.Xml.XmlWriterSettings() {
                    Encoding = Encoding.UTF8
                });
                serializer.Serialize(writer, o);
                if (stringWriter != null) {
                    var xmlText = stringWriter.ToString();
                    var bytes = Encoding.UTF8.GetBytes(xmlText);
                    if (bytes != null && bytes.Length > 0) {
                        if (zip.ContainsEntry(fileName)) { zip.RemoveEntry(fileName); }
                        zip.AddEntry(fileName, bytes);
                    }
                }
            }
            else {
                if (zip.ContainsEntry(fileName)) { zip.RemoveEntry(fileName); }
            }
        }

        class Utf8StringWriter : StringWriter {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
