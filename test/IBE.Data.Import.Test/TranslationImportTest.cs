using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace IBE.Data.Import.Test {
    [TestClass]
    public class TranslationImportTest {
        private void ImportTranslation(string path, bool interlinear = false, TranslationType type = TranslationType.Default, bool catolic = false, bool recommended = false) {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();

            Translation trans = null;

            if (interlinear) {
                trans = new TranslationImporter().ImportInterlinear(path, uow);
            }
            else {
                trans = new TranslationImporter().Import(path, uow);
            }

            if (trans.IsNotNull()) {
                trans.Type = type;
                trans.Recommended = recommended;
                trans.Catolic = catolic;
            }

            uow.CommitChanges();
        }
        [TestMethod] public void Import_SNP18() { ImportTranslation(@"..\..\..\..\db\import\SNP'18.zip", type: TranslationType.Default, recommended: true); }
        [TestMethod] public void Import_UBG18() { ImportTranslation(@"..\..\..\..\db\import\UBG'18.zip", type: TranslationType.Default); }
        [TestMethod] public void Import_BG() { ImportTranslation(@"..\..\..\..\db\import\BG.zip", type: TranslationType.Default); }
        [TestMethod] public void Import_BJW() { ImportTranslation(@"..\..\..\..\db\import\BJW.zip", type: TranslationType.Default, catolic: true); }
        [TestMethod] public void Import_BT() { ImportTranslation(@"..\..\..\..\db\import\BT'99.zip", type: TranslationType.Default, catolic: true); }
        [TestMethod] public void Import_BW() { ImportTranslation(@"..\..\..\..\db\import\BW.zip", type: TranslationType.Default); }
        [TestMethod] public void Import_EKU18() { ImportTranslation(@"..\..\..\..\db\import\EKU'18.zip", type: TranslationType.Default); }
        [TestMethod] public void Import_NBG12() { ImportTranslation(@"..\..\..\..\db\import\NBG'12.zip", type: TranslationType.Default); }
        [TestMethod] public void Import_NTPZ() { ImportTranslation(@"..\..\..\..\db\import\NTPZ.zip", type: TranslationType.Default); }
        [TestMethod] public void Import_PAU() { ImportTranslation(@"..\..\..\..\db\import\PAU.zip", type: TranslationType.Default, catolic: true); }
        [TestMethod] public void Import_PBP() { ImportTranslation(@"..\..\..\..\db\import\PBP.zip", type: TranslationType.Default, catolic: true); }
        [TestMethod] public void Import_PBW() { ImportTranslation(@"..\..\..\..\db\import\PBW.zip", type: TranslationType.Default); }
        [TestMethod] public void Import_POZ75() { ImportTranslation(@"..\..\..\..\db\import\POZ'75.zip", type: TranslationType.Default, catolic: true); }

        [TestMethod] public void Import_PNS1997() { ImportTranslation(@"..\..\..\..\db\import\PNS1997.zip", type: TranslationType.Dynamic); }
        [TestMethod] public void Import_PSZ() { ImportTranslation(@"..\..\..\..\db\import\PSZ.zip", type: TranslationType.Dynamic); }
        [TestMethod] public void Import_EDB() { ImportTranslation(@"..\..\..\..\db\import\EDB.zip", type: TranslationType.Dynamic); }

        [TestMethod] public void Import_PBD() { ImportTranslation(@"..\..\..\..\db\import\PBD.zip", type: TranslationType.Literal, recommended: true); }
        [TestMethod] public void Import_PBPW() { ImportTranslation(@"..\..\..\..\db\import\PBPW.zip", type: TranslationType.Literal, catolic: true); }
        [TestMethod] public void Import_TRO() { ImportTranslation(@"..\..\..\..\db\import\TRO.zip", true, TranslationType.Literal); }
        [TestMethod] public void Import_IBHP() { ImportTranslation(@"..\..\..\..\db\import\IBHP+.zip", true, TranslationType.Literal); }

        //[TestMethod] public void ExportInterlinearTest() {
        //    var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".docx");
        //    var licPath = @"..\..\..\..\..\..\Aspose.Total.lic";

        //    new Export.InterlinearExporter(File.ReadAllBytes(licPath)).Test(path);

        //    if (File.Exists(path)) {
        //        System.Diagnostics.Process.Start(path);
        //    }
        //}
        [TestMethod] public void ExportInterlinearChapterToPdf() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();

            var trans = new XPQuery<Translation>(uow).Where(x => x.Name == "NPI+").FirstOrDefault();
            var book = trans.Books.Where(x => x.NumberOfBook == 470).FirstOrDefault();
            var chapter = book.Chapters.Where(x => x.NumberOfChapter == 6).FirstOrDefault();

            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".docx");
            var licPath = @"..\..\..\..\..\..\Aspose.Total.lic";

            new Export.InterlinearExporter(File.ReadAllBytes(licPath)).Export(chapter, Export.ExportSaveFormat.Docx, path);

            if (File.Exists(path)) {
                System.Diagnostics.Process.Start(path);
            }
        }
    }
}

