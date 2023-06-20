using ChurchServices.Data.Import.EIB.Model.Bible;
using ChurchServices.Data.Import.EIB.Model.Osis;

namespace ChurchServices.Data.Import.EIB.Test {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestOsisModel() {
            var service = new OsisModelService();
            var model = service.GetModelFromFile(@"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\snpd.osis.v3.xml");
            if (model != null) {
                Assert.IsTrue(model.Text != null && model.Text.Divisions != null && model.Text.Divisions.Count > 0);
            }
            else {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void TestConvertOsisModelToBibleModel() {
            var service = new OsisModelService();
            var model = service.GetModelFromFile(@"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\snpd.osis.v3.xml", true);
            if (model != null) {
                var bservice = new BibleModelService();
                var bmodel = bservice.GetBibleModelFromOsisModel(model);
                if (bmodel != null) {
                    Assert.IsTrue(bmodel.Books.Count > 0);
                    var xmlInputFile = @"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\snpd.eib.xml";
                    bservice.SaveBibleModelToFile(bmodel, xmlInputFile);

                    if (File.Exists(xmlInputFile)) {
                        using (var service2 = new LogosBibleModelService()) {
                            var bibleModelOutFilePath = @"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\snpd.eib.docx";
                            service2.Export(xmlInputFile, bibleModelOutFilePath);
                        }
                    }
                    else {
                        Assert.Fail();
                    }
                }
                else {
                    Assert.Fail();
                }
            }
            else {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void BuildLogosWordFromBibleModel() {
            var bibleModelFilePath = @"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\snpd.eib.xml";
            var bibleModelOutFilePath = @"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\snpd.eib.docx";
            using (var service = new LogosBibleModelService()) {
                service.Export(bibleModelFilePath, bibleModelOutFilePath);
            }
        }

        [TestMethod]
        public void TestNoteText() {
            var text = "bezładna i pusta, וּהֹב ָו  וּהֹת ( tohu wawohu ),  tak  opisywane  są  skutki  sądu,  np.  Iz 34:10-11; Jr 4:23. Odczyt w. 2: Ziemia zaś stała się bezładna i pusta i (l. tak, że ) ciemność [rozciągała się] nad otchłanią, a Duch Boży unosił się nad powierzchnią wód, służy za uzasadnienie teorii stworzenia – odnowy, głoszącej,  że  Bóg  stworzył  niebo  i  ziemię, doszło do katastrofy (zob. Iz 45:18; Ez 28:1119), a w. 3 rozpoczyna opis procesu odnowy. Stworzył, א ָר ָבּ ( bara’ ), lub: ukształtował, por.  Rdz  1:27; א ָר ָבּ  zawsze  opisuje  działanie Boga, jak również tworzenie czegoś na nowo, zob. Ps 51:12; Iz 43:15; 65:17.";
            var bservice = new BibleModelService();
            var result = bservice.RecognizeHebrewAndGreekAndBibleTags(text);
            if (result != null) {
                Assert.IsTrue(result.Length > 0);
            }
            else {
                Assert.Fail();
            }
        }
    }
}