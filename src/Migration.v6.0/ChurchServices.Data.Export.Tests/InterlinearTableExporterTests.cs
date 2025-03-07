using ChurchServices.Data.Import;
using ChurchServices.Data.Model;
using DevExpress.Xpo;

namespace ChurchServices.Data.Export.Tests {
    [TestClass]
    public class InterlinearTableExporterTests {
        [TestInitialize]
        public void Init() {
            new ConnectionHelper().Connect(
                connectionString: @"XpoProvider=SQLite;data source=..\..\..\..\..\..\db\IBE.SQLite3");
        }

        [TestMethod]
        public void GetTextSizeTestMethod() {
            var bytes = File.ReadAllBytes(@"..\..\..\..\..\..\db\Aspose.Total.NET.lic");
            var uow = new UnitOfWork();
            var q = new XPQuery<Verse>(uow);
            var verse = q.Where(x => x.Index == "NPI.680.1.2").FirstOrDefault();
            if (verse != null) {
                var service = new InterlinearTableExporter(bytes, "");
                var fileName = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".docx");
                service.ExportChapter(verse.ParentChapter, outputPath: fileName);
            }
        }

        [TestMethod]
        public void UpdateChapterIndexes() {
            var uow = new UnitOfWork();
            var q = new XPQuery<Chapter>(uow);
            foreach (var chapter in q) {
                chapter.Index = $"{chapter.ParentBook.ParentTranslation.Name.Replace("'", "").Replace("+", "")}.{chapter.ParentBook.NumberOfBook}.{chapter.NumberOfChapter}";
                chapter.Save();
            }
            uow.CommitChanges();

        }

        [TestMethod]
        public void ImportTroPlus() {
            var uow = new UnitOfWork();
            var controller = new TroInterlinearImporter();
            controller.Execute(@"..\..\..\..\..\..\db\import\TRO+.SQLite3", uow);
        }

    }
}