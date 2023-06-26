using ChurchServices.Data.Import.EIB.Model.Bible;
using ChurchServices.Data.Import.EIB.Model.Osis;
using ChurchServices.Data.Model;
using ChurchServices.Extensions;
using DevExpress.Xpo;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text;
using System.Text.RegularExpressions;
using static DevExpress.Data.Helpers.ExpressiveSortInfo;

namespace ChurchServices.Data.Import.EIB.Test {
    [TestClass]
    public class UnitTest1 {

        [TestInitialize]
        public void Init() {
            new ConnectionHelper().Connect(
                connectionString: @"XpoProvider=SQLite;data source=..\..\..\..\..\..\db\IBE.SQLite3");
        }


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

        private void SetBookInfo(BookModel book, BibleModel model) {
            var uow = new UnitOfWork();
            var dbBook = new XPQuery<ChurchServices.Data.Model.BookBase>(uow).Where(x => x.NumberOfBook == book.NumberOfBook).FirstOrDefault();
            if (dbBook != null) {
                book.PlaceWhereBookWasWritten = RecognizeBookInfo(dbBook.PlaceWhereBookWasWritten, model, uow);
                book.AuthorName = RecognizeBookInfo(dbBook.AuthorName, model, uow);
                book.TimeOfWriting = RecognizeBookInfo(dbBook.TimeOfWriting, model, uow);
                book.Purpose = RecognizeBookInfo(dbBook.Purpose, model, uow);
                book.Subject = RecognizeBookInfo(dbBook.Subject, model, uow);
            }
        }

        private string RemoveOrphans(string text) {
            var pattern = @"((?<text1>\s[a-zA-Z])(?<space1>\s)(?<text2>[a-zA-Z])(?<space2>\s))|((?<text3>\s[a-zA-Z])(?<space3>\s))|((?<text4>\s[0-9]+)(?<space4>\s))|(^(?<text5>[a-zA-Z])(?<space5>\s))";
            return Regex.Replace(text, pattern, delegate (Match m) {
                if (m.Groups["text1"] != null && m.Groups["text1"].Value.IsNotNullOrEmpty()) {
                    return $"{m.Groups["text1"].Value}\u00A0{m.Groups["text2"].Value}\u00A0";
                }
                if (m.Groups["text3"] != null && m.Groups["text3"].Value.IsNotNullOrEmpty()) {
                    return $"{m.Groups["text3"].Value}\u00A0";
                }
                if (m.Groups["text4"] != null && m.Groups["text4"].Value.IsNotNullOrEmpty()) {
                    return $"{m.Groups["text4"].Value}\u00A0";
                }
                if (m.Groups["text5"] != null && m.Groups["text5"].Value.IsNotNullOrEmpty()) {
                    return $"{m.Groups["text5"].Value}\u00A0";
                }
                return m.Value; // nic nie zmieniam
            });
        }

        private FormattedText RecognizeBookInfo(string text, BibleModel model, UnitOfWork uow) {
            text = text.Replace("&nbsp;", "\u00A0");
            var result = text.Contains("<x>") ? new FormattedText() : new FormattedText(RemoveOrphans(text));
            if (result.Items == null) {
                result.Items = new List<object>();

                var pattern = @"(?<prefix>[\w\s\(\)\d\,\;\.\:]+)?(\<x\>(?<book>[0-9]+)\s(?<chapterStart>[0-9]+)(\s)?\:(\s)?(?<verseStart>[0-9]+)(\-(?<verseEnd>[0-9]+))?\<\/x\>)(?<postfix>[\w\s\(\)\d\,\;\.\:]+)?";
                var matches = Regex.Matches(text, pattern);
                foreach (Match match in matches) {
                    if (match.Groups["prefix"] != null && match.Groups["prefix"].Value.IsNotNullOrEmpty()) {
                        result.Items.Add(RemoveOrphans(match.Groups["prefix"].Value));
                    }
                    var sb = new StringBuilder();
                    var sb2 = new StringBuilder();
                    if (match.Groups["book"] != null && match.Groups["book"].Value.IsNotNullOrEmpty()) {
                        var bn = match.Groups["book"].Value.ToInt();
                        var dbBook = new XPQuery<ChurchServices.Data.Model.BookBase>(uow).Where(x => x.NumberOfBook == bn).FirstOrDefault();
                        var bm = model.Books.Where(x => x.NumberOfBook == bn).FirstOrDefault();
                        sb2.Append($"{dbBook.BookShortcut}\u00A0");
                        sb.Append($"{bm.BookShortcut}\u00A0");
                    }
                    if (match.Groups["chapterStart"] != null && match.Groups["chapterStart"].Value.IsNotNullOrEmpty()) {
                        sb2.Append(match.Groups["chapterStart"].Value);
                        sb.Append(match.Groups["chapterStart"].Value);
                    }
                    if (match.Groups["verseStart"] != null && match.Groups["verseStart"].Value.IsNotNullOrEmpty()) {
                        sb2.Append($":{match.Groups["verseStart"].Value}");
                        sb.Append($":{match.Groups["verseStart"].Value}");
                    }
                    if (match.Groups["verseEnd"] != null && match.Groups["verseEnd"].Value.IsNotNullOrEmpty()) {
                        sb2.Append($"-{match.Groups["verseEnd"].Value}");
                        sb.Append($"-{match.Groups["verseEnd"].Value}");
                    }
                    if (sb.Length > 0) {
                        var sb3 = $"[[{sb2}>>{sb}]]";
                        result.Items.Add(sb3);
                    }

                    if (match.Groups["postfix"] != null && match.Groups["postfix"].Value.IsNotNullOrEmpty()) {
                        result.Items.Add(RemoveOrphans(match.Groups["postfix"].Value));
                    }
                }

            }
            return result;
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

                    foreach (var book in bmodel.Books) {
                        SetBookInfo(book, bmodel);
                    }

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
        public void ExportDbBibleBWToLogosFile() { ExportDbBibleToLogosFile("BW"); }

        [TestMethod]
        public void ExportDbBibleEkuToLogosFile() { ExportDbBibleToLogosFile("EKU'18"); }

        private void ExportDbBibleToLogosFile(string name) {
            var fn = 1;
            if (name != null) {
                var srv = new BibleModelService();
                var uow = new UnitOfWork();
                var dbTranslation = new XPQuery<ChurchServices.Data.Model.Translation>(uow).Where(x => x.Name == name).FirstOrDefault();
                if (dbTranslation != null) {
                    var model = new BibleModel() {
                        Shortcut = dbTranslation.Name,
                        Name = dbTranslation.Description,
                        Type = Model.Bible.TranslationType.Default,
                        Books = new List<BookModel>()
                    };
                    foreach (var dbBook in dbTranslation.Books) {

                        var dbBaseBook = new XPQuery<ChurchServices.Data.Model.BookBase>(uow).Where(x => x.NumberOfBook == dbBook.NumberOfBook).FirstOrDefault();

                        var book = new BookModel() {
                            BookShortcut = srv.GetShortcutFromEIBAbbreviation(dbBaseBook.BookShortcut),
                            NumberOfBook = dbBook.NumberOfBook,
                            BookName = dbBook.BookName,
                            Color = dbBook.Color,
                            Chapters = new List<ChapterModel>()
                        };

                        model.Books.Add(book);

                        foreach (var dbChapter in dbBook.Chapters.OrderBy(x => x.NumberOfChapter)) {
                            var chapter = new ChapterModel() {
                                NumberOfChapter = dbChapter.NumberOfChapter,
                                Items = new List<object>()
                            };
                            book.Chapters.Add(chapter);

                            foreach (var dbVerse in dbChapter.Verses.OrderBy(x => x.NumberOfVerse)) {

                                var dbSubtitles = new XPQuery<ChurchServices.Data.Model.Subtitle>(uow).Where(x => x.ParentChapter.Oid == dbChapter.Oid && x.BeforeVerseNumber == dbVerse.NumberOfVerse).OrderBy(x => x.Level).ToList();
                                if (dbSubtitles.Count > 0) {
                                    foreach (var dbSubtitle in dbSubtitles) {
                                        var stext = dbSubtitle.Text.Replace("<", "").Replace(">", "");
                                        chapter.Items.Add(
                                            new FormattedText(RemoveOrphans(stext))
                                        );
                                    }
                                }
                                var verse = new VerseModel() {
                                    NumberOfVerse = dbVerse.NumberOfVerse,
                                    Style = VerseStyle.Default,
                                    StartFromNewLine = dbVerse.StartFromNewLine,
                                    Items = new List<object>()
                                };

                                // add content
                                var text = dbVerse.Text
                                    .Replace("<J>", "{{field-on:words-of-christ}}")
                                    .Replace("</J>", "{{field-off:words-of-christ}}")
                                    .Replace("<e>", "{{field-on:ot-quote}}")
                                    .Replace("</e>", "{{field-off:ot-quote}}")
                                    .Replace("<t>", "")
                                    .Replace("</t>", "")
                                    .Replace("<i>", "")
                                    .Replace("</i>", "")
                                    .Replace("<u>", "")
                                    .Replace("</u>", "")
                                    .Replace("<b>", "")
                                    .Replace("</b>", "")
                                    .Replace("<pb>", "")
                                    .Replace("<pb/>", "")
                                    .Replace("<br/>", "")
                                    .Replace("*", "");                                   

                                text = Regex.Replace(text, @"\<f\>\[[0-9]+\]\<\/f\>", delegate (Match e) {
                                    return "";
                                });
                                text = Regex.Replace(text, @"\!{2,3}", delegate (Match e) {
                                    return "!";
                                });
                                text = Regex.Replace(text, @"\s{2,3}", delegate (Match e) {
                                    return " ";
                                });
                                text = Regex.Replace(text, @"\<h\>(?<text>([AaĄąBbCcĆćDdEeĘęFfGgHhIiJjKkLlŁłMmNnŃńOoÓóPpRrSsŚśTtUuWwYyZzŹźŻż\s\-\.\;\:\?\""\'\,])+)\<\/h\>", delegate (Match e) {
                                    chapter.Items.Add(
                                        new FormattedText(RemoveOrphans(e.Groups["text"].Value))
                                    );
                                    return "";
                                });
                                text = Regex.Replace(text, @"\<n\>(?<text>.+)\<\/n\>", delegate (Match e) {
                                    var footnoteText = e.Groups["text"].Value;
                                    if (footnoteText.IsNotNullOrEmpty()) {
                                        verse.Items.Add(new NoteModel() {
                                            Number = fn.ToString(),
                                            Type = NoteType.Default,
                                            Items = new List<object> {
                                            footnoteText
                                        }
                                        });
                                        fn++;
                                    }
                                    return "";
                                });

                                //text = text.Replace("<", "").Replace(">", "");

                                if (text.IsNotNullOrEmpty()) {
                                    text = RemoveOrphans(text);
                                    verse.Items.Add(new Span(text));
                                }
                                chapter.Items.Add(verse);
                            }
                        }
                    }

                    using (var service = new LogosBibleModelService()) {
                        service.Export(model, @$"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\{dbTranslation.Name.Replace("'", "").Replace("+", "")}.docx", true);
                        srv.SaveBibleModelToFile(model, @$"D:\OneDrive\WBST\2020\Fakultety\Biblistyka\EIB\SNP_BibleEngine\SNP_BibleEngine\SNPD\{dbTranslation.Name.Replace("'", "").Replace("+", "")}.xml");
                    }
                }
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