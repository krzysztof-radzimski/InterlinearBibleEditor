using ChurchServices.Data.Model;
using ChurchServices.Extensions;
using DevExpress.Xpo;
using Microsoft.Data.Sqlite;

namespace ChurchServices.Data.Import.Hebrew {
    public class LhbBuilder {
        const string NAME = "LHB";
        private List<BookBase> BaseBooks = null;
        public void Build(UnitOfWork uow, string path) {
            var connection = GetConnection(path);
            var bhsVerses = GetVerses(connection);
            BaseBooks = new XPQuery<BookBase>(uow).Where(x => x.Status.BiblePart == BiblePart.OldTestament).OrderBy(x => x.NumberOfBook).ToList();

            var translation = CreateTranslation(uow);
            foreach (var baseBook in BaseBooks) {
                if (bhsVerses.Where(x => x.Book == baseBook.NumberOfBook).Count() == 0) {
                    continue;
                }
                var chapterCount = bhsVerses.Where(x => x.Book == baseBook.NumberOfBook)
                                            .Max(x => x.Chapter);

                var book = new Book(uow) {
                    BaseBook = baseBook,
                    BookName = baseBook.BookName,
                    BookShortcut = baseBook.BookShortcut,
                    Color = baseBook.Color,
                    NumberOfBook = baseBook.NumberOfBook,
                    NumberOfChapters = chapterCount,
                    ParentTranslation = translation
                };
                book.Save();

                for (int chapterIndex = 1; chapterIndex <= chapterCount; chapterIndex++) {
                    var versesCount = bhsVerses.Where(x => x.Book == baseBook.NumberOfBook &&
                                                           x.Chapter == chapterIndex)
                                               .Max(x => x.Verse);
                    var chapter = new Chapter(uow) {
                        NumberOfChapter = chapterIndex,
                        NumberOfVerses = versesCount,
                        ParentBook = book,
                        Index = $"{NAME}.{baseBook.NumberOfBook}.{chapterIndex}"
                    };
                    chapter.Save();

                    for (int verseIndex = 1; verseIndex <= versesCount; verseIndex++) {
                        var bhsVerse = bhsVerses.Where(x => x.Book == baseBook.NumberOfBook &&
                                                            x.Chapter == chapterIndex &&
                                                            x.Verse == verseIndex).FirstOrDefault();
                        if (bhsVerse != null) {
                            var verse = new Verse(uow) {
                                NumberOfVerse = verseIndex,
                                ParentChapter = chapter,
                                Text = bhsVerse.Text,
                                Index = $"{NAME}.{baseBook.NumberOfBook}.{chapterIndex}.{verseIndex}"
                            };
                            verse.Save();
                        }
                    }
                }
            }
            uow.CommitChanges();
        }
        private IEnumerable<LhbVerseInfo> GetVerses(SqliteConnection conn) {
            var list = new List<LhbVerseInfo>();
            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM verses";
            using (var r = command.ExecuteReader()) {
                while (r.Read()) {
                    if (r.IsDBNull(3)) { continue; }
                    var vi = new LhbVerseInfo(r.GetInt32(0), r.GetInt32(1), r.GetInt32(2), r.GetString(3));
                    list.Add(vi);
                }
            }
            return list;
        }
        private Translation CreateTranslation(UnitOfWork uow) {
            var result = new XPQuery<Translation>(uow).Where(x => x.Name == NAME).FirstOrDefault();
            if (result.IsNull()) {
                result = new Translation(uow) {
                    Name = NAME,
                    Description = "Lexham Hebrew Bible",
                    ChapterPsalmString = "Psalm",
                    ChapterString = "Rozdział",
                    DetailedInfo = "",
                    Language = Language.Polish,
                    Type = TranslationType.Literal,
                    BookType = TheBookType.Bible,
                    Introduction = "",
                    Hidden = true
                };
                result.Save();
            }
            return result;
        }
        private SqliteConnection GetConnection(string filePath) {
            var conn = new SqliteConnection($"DataSource=\"{filePath}\"");
            SQLitePCL.Batteries.Init();
            conn.Open();
            return conn;
        }
    }

    class LhbVerseInfo {
        public int Book { get; private set; }
        public int Chapter { get; private set; }
        public int Verse { get; private set; }
        public string Text { get; private set; }
        public LhbVerseInfo(int book, int chapter, int verse, string text) {
            Book = book;
            Chapter = chapter;
            Verse = verse;
            Text = text;
        }
    }
}
