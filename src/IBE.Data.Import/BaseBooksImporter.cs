/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IBE.Data.Import {
    public class BaseBooksImporter : BaseImporter<object> {
        public override object Import(string zipFilePath, UnitOfWork uow) {
            if (File.Exists(zipFilePath)) {
                var fileName = ExtractAndGetFirstArchiveItemFilePath(zipFilePath);
                try {
                    using (var conn = new SqliteConnection($"DataSource=\"{fileName}\"")) {
                        SQLitePCL.Batteries.Init();
                        conn.Open();

                        var command = conn.CreateCommand();
                        command.CommandText = @"SELECT book_number, short_name, long_name, book_color FROM books_all";

                        using (var reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                var number = reader.GetInt32(0);
                                var shortcut = reader.GetString(1);
                                var name = reader.GetString(2);
                                var title = reader.GetString(3);
                                var color = reader.GetString(4);
                                var status = GetBookStatus(uow, GetPart(number), GetCanon(number));
                                var q = new XPQuery<BookBase>(uow).Where(x => x.NumberOfBook == number).FirstOrDefault();
                                if (q.IsNull()) {
                                    var book = new BookBase(uow) {
                                        NumberOfBook = number,
                                        BookName = name,
                                        BookShortcut = shortcut,
                                        BookTitle = title,
                                        Color = color,
                                        Status = status
                                    };
                                    book.Save();
                                    uow.CommitChanges();
                                    uow.ReloadChangedObjects();
                                }
                            }
                        }
                        conn.Close();
                    }
                }
                finally {
                    try { File.Delete(fileName); } catch { }
                }
            }
            return default;
        }

        private BiblePart GetPart(int bookNumber) {
            return bookNumber >= 470 ? BiblePart.NewTestament : BiblePart.OldTestament;
        }
        private CanonType GetCanon(int bookNumber) {
            switch (bookNumber) {
                case 10:
                case 20:
                case 30:
                case 40:
                case 50:
                case 60:
                case 70:
                case 80:
                case 90:
                case 100:
                case 110:
                case 120:
                case 130:
                case 140:
                case 150:
                case 160:
                case 190:
                case 220:
                case 230:
                case 240:
                case 250:
                case 260:
                case 290:
                case 300:
                case 310:
                case 330:
                case 340:
                case 350:
                case 360:
                case 370:
                case 380:
                case 390:
                case 400:
                case 410:
                case 420:
                case 430:
                case 440:
                case 450:
                case 460:
                //---
                case 470:
                case 480:
                case 490:
                case 500:
                case 510:
                case 520:
                case 530:
                case 540:
                case 550:
                case 560:
                case 570:
                case 580:
                case 590:
                case 600:
                case 610:
                case 620:
                case 630:
                case 640:
                case 650:
                case 660:
                case 670:
                case 680:
                case 690:
                case 700:
                case 710:
                case 720:
                case 730: {
                        return CanonType.Canon;
                    }
                case 170:
                case 180:
                case 462:
                case 464:
                case 270:
                case 280:
                case 320: {
                        return CanonType.SecondCanon;
                    }
                default: {
                        return CanonType.Apocrypha;
                    }
            }
        }

        private BookStatus GetBookStatus(UnitOfWork uow, BiblePart part = BiblePart.OldTestament, CanonType canonType = CanonType.Canon) {
            var result = new XPQuery<BookStatus>(uow).Where(x => x.CanonType == canonType && x.BiblePart == part).FirstOrDefault();
            if (result.IsNull()) {
                result = new BookStatus(uow) {
                    BiblePart = part,
                    CanonType = canonType
                };
                result.Save();
            }
            return result;
        }
    }
}
