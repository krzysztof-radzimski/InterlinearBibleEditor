/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using IBE.Translator.Controllers;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

            var verses = new XPQuery<Model.Verse>(uow).Where(x => x.Index == null).ToList();
            foreach (var item in verses) {
                var translationName = item.ParentTranslation.Name.Replace("+", "").Replace("'", "");
                var index = $"{translationName}.{item.ParentChapter.ParentBook.NumberOfBook}.{item.ParentChapter.NumberOfChapter}.{item.NumberOfVerse}";
                item.Index = index;
                item.Save();
            }

            uow.CommitChanges();
        }


        [TestMethod]
        public void UpdateInterlinearVerseText() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();

            var verses = new XPQuery<Model.Verse>(uow).Where(x => x.Index.Contains("NPI") && (x.Text == null || x.Text == "")).ToList();
            foreach (var item in verses) {
                if (item.ParentChapter.IsTranslated) {
                    var text = String.Empty;
                    foreach (var verseWord in item.VerseWords.OrderBy(x => x.NumberOfVerseWord)) {
                        text += $"{verseWord.Translation} ";
                    }

                    item.Text = text.Trim();
                    item.Save();
                }
            }

            uow.CommitChanges();
        }


        [TestMethod]
        public async Task TranslateText() {
            var textToTranslate = @"ἁμαρτία, (ας, ἡ (from 2 aorist ἁμαρτεῖν, as ἀποτυχία from ἀποτύχειν), a failing to hit the mark (see ἁμαρτάνω. In Greek writings (from Aeschylus and Thucydides down). 1st, an error of the understanding (cf. Ackermann, Das Christl. im Plato, p. 59 Anm. 3 (English translation (S. R. Asbury, 1861), p. 57 n. 99)). 2nd, a bad action, evil deed. In the N. T. always in an ethical sense, and";

            var controller = new TranslatorController();
            var result = await controller.Translate(textToTranslate);
            if (result != null) {

            }
        }

        System.Net.WebClient wc = null;

        [TestMethod]
        public void TranslateStrongPage() {
            if (wc == null) {
                wc = new System.Net.WebClient();
                //wc.DownloadStringCompleted += OnDownloadStringCompleted;
            }
            var url = @"https://biblehub.com/greek/266.htm";
            //https://www.blueletterbible.org/lexicon/g266/nasb20/tr/0-1/
            //wc.DownloadStringAsync(new Uri(url));
            var html = wc.DownloadString(url);
            if (html != null) { 
            
            }
        }

        //private void OnDownloadStringCompleted(object sender, System.Net.DownloadStringCompletedEventArgs e) {
        //    if (e.Result != null) {

        //    }
        //}
    }
}
