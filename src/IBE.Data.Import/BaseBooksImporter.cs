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
using System.IO;
using System.Linq;

namespace IBE.Data.Import {
    public class BaseBooksImporter : BaseImporter {
        public override void Import(string zipFilePath, UnitOfWork uow) {
            if (File.Exists(zipFilePath)) {
                var fileName = ExtractAndGetFirstArchiveItemFilePath(zipFilePath);
                try {
                    using (var conn = new SqliteConnection($"DataSource=\"{fileName}\"")) {
                    SQLitePCL.Batteries.Init();
                    conn.Open();

                    var command = conn.CreateCommand();
                    command.CommandText = @"SELECT * FROM books_all";

                    var old_status = GetBookStatus(uow);
                    var new_status = GetBookStatus(uow, BiblePart.NewTestament);

                    using (var reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            var number = reader.GetInt32(0);
                            var shortcut = reader.GetString(1);
                            var name = reader.GetString(2);
                            var title = reader.GetString(3);
                            var color= reader.GetString(4);

                            var q = new XPQuery<BookBase>(uow).Where(x => x.NumberOfBook == number).FirstOrDefault();
                            if (q.IsNull()) {
                                var book = new BookBase(uow) {
                                    NumberOfBook = number,
                                    BookName = name,
                                    BookShortcut = shortcut,
                                    BookTitle = title,
                                    Color = color,
                                    Status = number >= 470 ? new_status : old_status
                                };
                                book.Save();
                            }
                        }
                    }

                    }
                }
                finally {
                    try { File.Delete(fileName); } catch { }
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
