using DevExpress.Xpo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBE.Data.Import.Test {
    [TestClass]
    public class TranslationImportTest {
        private void ImportTranslation(string path) {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();
            new TranslationImporter().Import(path, uow);
            uow.CommitChanges();
        }
        [TestMethod] public void Import_SNP18() { ImportTranslation(@"..\..\..\..\db\import\SNP'18.zip"); }
        [TestMethod] public void Import_UBG18() { ImportTranslation(@"..\..\..\..\db\import\UBG'18.zip"); }
        [TestMethod] public void Import_BG() { ImportTranslation(@"..\..\..\..\db\import\BG.zip"); }
        [TestMethod] public void Import_BJW() { ImportTranslation(@"..\..\..\..\db\import\BJW.zip"); }
        [TestMethod] public void Import_BT() { ImportTranslation(@"..\..\..\..\db\import\BT'99.zip"); }
        [TestMethod] public void Import_BW() { ImportTranslation(@"..\..\..\..\db\import\BW.zip"); }
        [TestMethod] public void Import_EDB() { ImportTranslation(@"..\..\..\..\db\import\EDB.zip"); }
        [TestMethod] public void Import_EKU18() { ImportTranslation(@"..\..\..\..\db\import\EKU'18.zip"); }
        [TestMethod] public void Import_NBG12() { ImportTranslation(@"..\..\..\..\db\import\NBG'12.zip"); }
        [TestMethod] public void Import_NTPZ() { ImportTranslation(@"..\..\..\..\db\import\NTPZ.zip"); }
        [TestMethod] public void Import_PAU() { ImportTranslation(@"..\..\..\..\db\import\PAU.zip"); }
        [TestMethod] public void Import_PBD() { ImportTranslation(@"..\..\..\..\db\import\PBD.zip"); }
        [TestMethod] public void Import_PBP() { ImportTranslation(@"..\..\..\..\db\import\PBP.zip"); }
        [TestMethod] public void Import_PBPW() { ImportTranslation(@"..\..\..\..\db\import\PBPW.zip"); }
        [TestMethod] public void Import_PBW() { ImportTranslation(@"..\..\..\..\db\import\PBW.zip"); }
        [TestMethod] public void Import_PNS1997() { ImportTranslation(@"..\..\..\..\db\import\PNS1997.zip"); }
        [TestMethod] public void Import_POZ75() { ImportTranslation(@"..\..\..\..\db\import\POZ'75.zip"); }
        [TestMethod] public void Import_PSZ() { ImportTranslation(@"..\..\..\..\db\import\PSZ.zip"); }
    }
}

