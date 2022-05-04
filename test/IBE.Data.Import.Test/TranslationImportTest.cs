using DevExpress.Xpo;
using HtmlAgilityPack;
using IBE.Common.Extensions;
using IBE.Data.Import.Greek;
using IBE.Data.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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
                trans.BookType = TheBookType.Bible;
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

        [TestMethod] public void Import_TUB() { ImportTranslation(@"..\..\..\..\db\import\TUB.zip", type: TranslationType.Default); }

        //[TestMethod] public void ExportInterlinearTest() {
        //    var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".docx");
        //    var licPath = @"..\..\..\..\..\..\Aspose.Total.lic";

        //    new Export.InterlinearExporter(File.ReadAllBytes(licPath)).Test(path);

        //    if (File.Exists(path)) {
        //        System.Diagnostics.Process.Start(path);
        //    }
        //}
        [TestMethod]
        public void ExportInterlinearChapterToPdf() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();

            var trans = new XPQuery<Translation>(uow).Where(x => x.Name == "IPD+").FirstOrDefault();
            //var book = trans.Books.Where(x => x.NumberOfBook == 470).FirstOrDefault();
            var book = trans.Books.First();
            var chapter = book.Chapters.Where(x => x.NumberOfChapter == 3).FirstOrDefault();

            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".pdf");
            var licPath = @"..\..\..\..\..\..\Aspose.Total.lic";

            new Export.InterlinearExporter(File.ReadAllBytes(licPath),"").Export(chapter, Export.ExportSaveFormat.Pdf, path);

            if (File.Exists(path)) {
                System.Diagnostics.Process.Start(path);
            }
        }

        [TestMethod]
        public void ExportInterlinearBookToPdf() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();

            var trans = new XPQuery<Translation>(uow).Where(x => x.Name == "IPD+").FirstOrDefault();
            var book = trans.Books.FirstOrDefault();

            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".pdf");
            var licPath = @"..\..\..\..\..\..\Aspose.Total.lic";

            new Export.InterlinearExporter(File.ReadAllBytes(licPath),"").Export(book, Export.ExportSaveFormat.Pdf, path);

            if (File.Exists(path)) {
                System.Diagnostics.Process.Start(path);
            }
        }

        [TestMethod]
        public void ExportInterlinearBookToDocx() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();

            var trans = new XPQuery<Translation>(uow).Where(x => x.Name == "IPD+").FirstOrDefault();
            var book = trans.Books.FirstOrDefault();

            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".docx");
            var licPath = @"..\..\..\..\..\..\Aspose.Total.lic";

            new Export.InterlinearExporter(File.ReadAllBytes(licPath),"").Export(book, Export.ExportSaveFormat.Docx, path);

            if (File.Exists(path)) {
                System.Diagnostics.Process.Start(path);
            }
        }


        [TestMethod]
        public void AddStrongCodes() {
            var count = 0;
            //var signs = new char[] { '·', ',', '.' };
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();

            var wordsWithoutStrongCode = new XPQuery<VerseWord>(uow)
                    .Where(x => x.StrongCode == null && x.ParentVerse.ParentChapter.ParentBook.ParentTranslation.Name == "IPD+")
                    .ToArray();

            var wordsWithoutStrongCodeCount = wordsWithoutStrongCode.Length;

            var wordsWithStrongCode = new XPQuery<VerseWord>(uow)
                    .Where(x => x.StrongCode != null && x.StrongCode.Lang == Language.Greek)
                    .Select(x => new {
                        SourceWord = x.SourceWord.Replace("·", "").Replace(",", "").Replace(".", "").Replace("*", "").Replace(";", "").ToLower(),
                        StrongCode = x.StrongCode
                    })
                    .Distinct()
                    .ToArray();

            for (int i = 0; i < wordsWithoutStrongCodeCount; i++) {
                var item = wordsWithoutStrongCode[i];
                var _sourceWord = item.SourceWord.Replace("·", "").Replace(",", "").Replace(".", "").Replace("*", "").Replace(";", "").ToLower();
                var found = wordsWithStrongCode.Where(x => x.SourceWord == _sourceWord).FirstOrDefault();
                if (found.IsNotNull()) {
                    item.StrongCode = found.StrongCode;
                    item.Save();
                    count++;
                }
            }
            //foreach (var item in wordsWithoutStrongCode) {
            //    var _sourceWord = item.SourceWord.Replace("·", "").Replace(",", "").Replace(".", "").ToLower();
            //    var found = wordsWithStrongCode.Where(x => x.SourceWord.Replace("·", "").Replace(",", "").Replace(".", "").ToLower() == _sourceWord).FirstOrDefault();
            //    if (found.IsNotNull()) {
            //        item.StrongCode = found.StrongCode;
            //        item.Save();
            //    }
            //}

            uow.CommitChanges();
        }

        [TestMethod]
        public void AddGrammarCodes() {
            var count = 0;
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();

            var wordsWithoutGrammarCode = new XPQuery<VerseWord>(uow)
                   .Where(x => x.GrammarCode == null && x.ParentVerse.ParentChapter.ParentBook.ParentTranslation.Name == "IPD+")
                   .ToArray();

            var wordsWithGrammarCode = new XPQuery<VerseWord>(uow)
                    .Where(x => x.GrammarCode != null)
                    .Select(x => new {
                        SourceWord = x.SourceWord.Replace("·", "").Replace(",", "").Replace(".", "").Replace("*", "").Replace(";", "").ToLower(),
                        StrongCode = x.StrongCode,
                        GrammarCode = x.GrammarCode
                    })
                    .Distinct()
                    .ToArray();

            var wordsWithoutGrammarCodeCount = wordsWithoutGrammarCode.Length;

            for (int i = 0; i < wordsWithoutGrammarCodeCount; i++) {
                var item = wordsWithoutGrammarCode[i];
                var gc = item.GrammarCode;
                var sc = item.StrongCode;
                var _sourceWord = item.SourceWord.Replace("·", "").Replace(",", "").Replace(".", "").Replace("*", "").Replace(";", "").ToLower();

                var found = wordsWithGrammarCode.Where(x => x.SourceWord == _sourceWord && sc != null && x.StrongCode != null && x.StrongCode.Lang == sc.Lang && x.StrongCode.Code == sc.Code).FirstOrDefault();
                if (found != null) {
                    item.GrammarCode = found.GrammarCode;
                    item.Save();
                    count++;
                }
            }


            uow.CommitChanges();
        }

        [TestMethod]
        public void UpdateTransliterationChapter() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();
            var controller = new GreekTransliterationController();
            var verses = new XPQuery<Verse>(uow).Where(x => x.Index.StartsWith("NPI.10.1"));
            foreach (var verse in verses) {
                var words = verse.VerseWords;
                foreach (var word in words) {
                    controller.TransliterateWord(word);
                }
            }
            uow.CommitChanges();
        }

        [TestMethod]
        public void UpdateTransliteration() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();
            var controller = new GreekTransliterationController();
            var words = new XPQuery<VerseWord>(uow);
            foreach (var word in words) {
                controller.TransliterateWord(word);
            }
            uow.CommitChanges();
        }

        [TestMethod]
        public void TransliterationTest() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            var controller = new GreekTransliterationController();
            var sentence = controller.GetTransliterateSentence(new VerseIndex("NPI.10.1.2"), uow);

            Assert.IsTrue(sentence.TransliteritSentence != null);
        }

        [TestMethod]
        public void UpdateTransliterit() {
            var count = 0;
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            uow.BeginTransaction();

            var words = new XPQuery<VerseWord>(uow).Where(x => x.Transliteration.Contains("ph") || x.Transliteration.Contains("x")).ToArray();
            foreach (var word in words) {
                var tr = FixChar_PH(word.Transliteration);
                tr = FixChar_X(tr);
                word.Transliteration = tr;
                word.Save();
                count++;
            }

            uow.CommitChanges();
        }

        private string FixChar_PH(string text) {
            return text.Replace("ph", "f");
        }
        private string FixChar_X(string text) {
            return text.Replace("x", "ks");
        }

        [TestMethod]
        public void GetTranslationTranslatedStatusInfo() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            var trans = new XPQuery<Translation>(uow).Where(x => x.Name == /*"NPI+"*/"NPI+").FirstOrDefault();
            var info = trans.GetTranslatedInfo();
            if (info.IsNotNullOrEmpty()) {

            }
        }

        [TestMethod]
        public void CreateCitationFile() {
            var builder = new StringBuilder();
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            var trans = new XPQuery<Translation>(uow).Where(x => x.Name == "NPI+").FirstOrDefault();

            var csv = Properties.Resources.Cytaty_z_G_w_NT.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var row in csv) {
                var cells = row.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                var xs = cells[2].Trim();
                if (xs.Contains("Ml")) {

                }
                var pattern = @"(?<abb>([0-9])?(\s)?\w+)\s(?<chapter>[0-9]+)\:(?<verse>[0-9]+)";
                if (Regex.IsMatch(xs, pattern)) {
                    var match = Regex.Match(xs, pattern);
                    var abb = match.Groups["abb"].Value;
                    var chap = match.Groups["chapter"].Value;
                    var ver = match.Groups["verse"].Value;


                    var book = trans.Books.Where(x => x.BaseBook.BookShortcut == abb || x.BaseBook.BookShortcut.Replace(" ", "") == abb).FirstOrDefault();
                    if (book.IsNotNull()) {
                        var chapter = book.Chapters.Where(x => x.NumberOfChapter == chap.ToInt()).FirstOrDefault();
                        if (chapter.IsNotNull()) {
                            var verse = chapter.Verses.Where(x => x.NumberOfVerse == ver.ToInt()).FirstOrDefault();
                            if (verse.IsNotNull()) {
                                var sourceText = verse.GetSourceText();
                                builder.AppendLine($"{cells[0].Trim()};{cells[1].Trim()};{cells[2].Trim()};{sourceText.Replace(";", ".")}");
                            }
                            else {
                                builder.AppendLine($"{cells[0].Trim()};{cells[1].Trim()};{cells[2].Trim()}; ");
                            }
                        }
                    }
                }
            }

            using (var dlg = new SaveFileDialog() { Filter = "CSV file (*.csv)|*.csv" }) {
                if (dlg.ShowDialog() == DialogResult.OK) {
                    if (File.Exists(dlg.FileName)) { File.Delete(dlg.FileName); }
                    File.WriteAllText(dlg.FileName, builder.ToString(), Encoding.UTF8);
                }
            }
        }

        [TestMethod]
        public void ImportRobinsonsMorphologicalAnalysisCodes() {
            ConnectionHelper.Connect();
            var uow = new UnitOfWork();
            var grammarCodes = new XPQuery<GrammarCode>(uow);

            var url = @"http://www.modernliteralversion.org/bibles/bs2/RMAC/RMACindex.html";
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var links = doc.DocumentNode.Descendants().Where(x => x.Name == "a").ToList();
            foreach (var item in links) {
                var href = item.Attributes["href"].Value;
                var link = $"http://www.modernliteralversion.org/bibles/bs2/RMAC/{href}";
                var doc2 = web.Load(link);

                var cells = doc2.DocumentNode.Descendants().Where(x => x.Name == "td").ToList();
                if (cells.Count > 2) {
                    var cell = cells[2];
                    var html = cell.InnerHtml.Replace("<br>", "<br/>");

                    var gc = grammarCodes.Where(x => x.GrammarCodeVariant1 == item.InnerText.Trim()).FirstOrDefault();
                    if (gc.IsNotNull() && gc.GrammarCodeDescription.IsNullOrEmpty()) {
                        gc.GrammarCodeDescription = html;
                        gc.Save();
                    }
                }
            }

            uow.CommitChanges();
        }
    }
}

