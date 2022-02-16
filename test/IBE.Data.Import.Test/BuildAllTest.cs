using DevExpress.Xpo;
using IBE.Data.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace IBE.Data.Import.Test {
    [TestClass]
    public class BuildAllTest {
        //[TestMethod]
        //public void BuildTest() {
        //    var path = @"..\..\..\..\db\IBE.SQLite3";
        //    if (File.Exists(path)) {
        //        File.Delete(path);
        //    }
        //    new GreekImportTest().ImportGreekStrong();
        //    new CustomTests().ImportBaseBooks();
        //    var trans = new TranslationImportTest();
        //    trans.Import_SNP18();
        //    trans.Import_UBG18();
        //    trans.Import_BG();
        //    trans.Import_BJW();
        //    trans.Import_BT();
        //    trans.Import_BW();
        //    trans.Import_EDB();
        //    trans.Import_EKU18();
        //    trans.Import_NBG12();
        //    trans.Import_NTPZ();
        //    trans.Import_PAU();
        //    trans.Import_PBD();
        //    trans.Import_PBP();
        //    trans.Import_PBPW();
        //    trans.Import_PBW();
        //    trans.Import_PNS1997();
        //    trans.Import_POZ75();
        //    trans.Import_PSZ();

        //    // interlinearna 
        //    trans.Import_TRO();
        //    trans.Import_IBHP();
        //}

        [TestMethod]
        public void FillSongVerseIndexes() {

            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();
            foreach (var song in new XPQuery<Song>(uow)) {
                var index = 1;
                foreach (var verse in song.SongVerses) {
                    verse.Index = index;
                    index++;
                }
            }
            uow.CommitChanges();
        }


    }
}
