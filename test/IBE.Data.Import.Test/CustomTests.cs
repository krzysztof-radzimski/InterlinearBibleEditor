/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace IBE.Data.Import.Test {
    [TestClass]
    public class CustomTests {
        [TestMethod]
        public void ImportBaseBooks() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();
            // z Uspółcześnionej Gdańskiej protokanoniczne
            new BaseBooksImporter().Import(@"..\..\..\..\db\import\UBG'18.zip", uow);
            // z edycji świętego Pawła wtórnokanoniczne katolickie
            new BaseBooksImporter().Import(@"..\..\..\..\db\import\PAU.zip", uow);
            // z ekumenicznej apokryfy 
            new BaseBooksImporter().Import(@"..\..\..\..\db\import\EKU'18.zip", uow);
            uow.CommitChanges();
        }

        [TestMethod]
        public void SaveMeyersBooks() {
            using (var conn = new SqliteConnection("Data Source=meyer.cmtx")) {
                conn.Open();

                var cmd = conn.CreateCommand();
                int i = 40;
                cmd.CommandText = "select comments from BookCommentary";
                using (var reader = cmd.ExecuteReader()) {
                    while (reader.Read()) {
                        var comment = reader.GetValue(0);
                        if (comment != null) {
                            var bytes = comment as byte[];
                            if (bytes != null) {
                                File.WriteAllBytes($"books\\file{i}.dat", bytes);
                            }

                        }
                    }
                }
            }
        }

        [TestMethod]
        public void UpdateIndexs() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();

            var verses = new XPQuery<Model.Verse>(uow).Where(x=>x.Index == null).ToList();
            foreach (var item in verses) {
                var translationName = item.ParentTranslation.Name.Replace("+", "").Replace("'", "");
                var index = $"{translationName}.{item.ParentChapter.ParentBook.NumberOfBook}.{item.ParentChapter.NumberOfChapter}.{item.NumberOfVerse}";
                item.Index = index;
                item.Save();
            }

            uow.CommitChanges();
        }


    }
}
