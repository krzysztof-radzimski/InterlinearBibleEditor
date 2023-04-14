using ChurchServices.Data.Import.Greek;
using ChurchServices.Data.Model;
using DevExpress.Xpo;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChurchServices.Data.Import {
    public class TroInterlinearImporter {
        private IEnumerable<TroVerseInfo> TroVerses = null;
        public TroInterlinearImporter() { }
        public void Execute(string troPath, UnitOfWork uow) {
            var troConnection = GetConnection(troPath);
            var troBooks = GetTroBooks(troConnection);
            var baseBooks = new XPQuery<BookBase>(uow).OrderBy(x => x.NumberOfBook).ToList();
            TroVerses = GetTroVerseInfos(troConnection);

            uow.BeginTransaction();
            try {
                var name = "TRO16";
                var translation = new Translation(uow) {
                    Name = $"{name}+",
                    Description = "Interlinearny Przekład Textus Receptus Oblubienicy",
                    ChapterString = "Rozdział",
                    ChapterPsalmString = "Psalm",
                    Language = Language.Polish,
                    Type = TranslationType.Interlinear,
                    Catolic = false,
                    Recommended = false,
                    Introduction = "",
                    DetailedInfo = "",
                    BookType = TheBookType.Bible,
                    ChapterRomanNumbering = false,
                    OpenAccess = true,
                    WithStrongs = true,
                    WithGrammarCodes = true,
                    Hidden = false
                };

                translation.Save();

                foreach (var troBook in troBooks) {
                    var baseBook = baseBooks.Where(x => x.NumberOfBook == troBook.NumberOfBook).FirstOrDefault();
                    if (baseBook != null) {
                        var book = new Book(uow) {
                            BaseBook = baseBook,
                            ParentTranslation = translation,
                            NumberOfBook = troBook.NumberOfBook,
                            BookShortcut = troBook.ShortName,
                            BookName = troBook.LongName,
                            Color = troBook.BookColor,
                            NumberOfChapters = TroVerses.Where(x => x.Book == troBook.NumberOfBook).Max(x => x.Chapter),
                            IsTranslated = true
                        };

                        book.Save();

                        for (int i = 1; i <= book.NumberOfChapters; i++) {
                            var troVerses = TroVerses.Where(x => x.Book == troBook.NumberOfBook && x.Chapter == i).OrderBy(x => x.Verse);
                            var chapter = new Chapter(uow) {
                                NumberOfChapter = i,
                                NumberOfVerses = troVerses.Max(x => x.Verse),
                                ParentBook = book,
                                IsTranslated = true,
                                Index = $"{name}.{troBook.NumberOfBook}.{i}"
                            };

                            chapter.Save();

                            foreach (var troVerse in troVerses) {
                                var verseText = "";
                                foreach (var troWord in troVerse.Words) {
                                    if (troWord.Translation != null) {
                                        verseText += troWord.Translation.Trim() + " ";
                                    }
                                }

                                var verse = new Verse(uow) {
                                    NumberOfVerse = troVerse.Verse,
                                    ParentChapter = chapter,
                                    StartFromNewLine = false,
                                    Index = $"{name}.{troBook.NumberOfBook}.{i}.{troVerse.Verse}",
                                    Text = verseText.Trim()
                                };

                                verse.Save();

                                var wordNum = 1;
                                foreach (var troWord in troVerse.Words) {
                                    var word = new VerseWord(uow) {
                                        NumberOfVerseWord = wordNum,
                                        StrongCode = new XPQuery<StrongCode>(uow).Where(x => x.Lang == Language.Greek && x.Code == troWord.StrongCode).FirstOrDefault(),
                                        SourceWord = troWord.Text,
                                        Transliteration = troWord.Transliterit,
                                        GrammarCode = new XPQuery<GrammarCode>(uow).Where(x => x.GrammarCodeVariant1 == troWord.GrammarCode || x.GrammarCodeVariant2 == troWord.GrammarCode || x.GrammarCodeVariant3 == troWord.GrammarCode).FirstOrDefault(),
                                        Translation = troWord.Translation == null ? "―" : troWord.Translation,
                                        Citation = false,
                                        WordOfJesus = false,
                                        ParentVerse = verse,
                                        FootnoteText = null
                                    };

                                    word.Save();

                                    wordNum++;
                                }
                            }
                        }
                    }
                }

                uow.CommitTransaction();
            }
            catch {
                uow.RollbackTransaction();
            }
        }
        private SqliteConnection GetConnection(string filePath) {
            var conn = new SqliteConnection($"DataSource=\"{filePath}\"");
            SQLitePCL.Batteries.Init();
            conn.Open();
            return conn;
        }
        private List<TroVerseInfo> GetTroVerseInfos(SqliteConnection troConnection) {
            var list = new List<TroVerseInfo>();
            var command = troConnection.CreateCommand();
            command.CommandText = $"SELECT * FROM verses";
            using (var r = command.ExecuteReader()) {
                while (r.Read()) {
                    list.Add(new TroVerseInfo(r.GetInt32(0), r.GetInt32(1), r.GetInt32(2), r.GetString(3)));
                }
            }
            return list;
        }
        private List<TroBookInfo> GetTroBooks(SqliteConnection troConnection) {
            var list = new List<TroBookInfo>();
            var command = troConnection.CreateCommand();
            command.CommandText = $"SELECT * FROM books";
            using (var r = command.ExecuteReader()) {
                while (r.Read()) {
                    list.Add(new TroBookInfo() { NumberOfBook = r.GetInt32(0), ShortName = r.GetString(1), LongName = r.GetString(2), BookColor = r.GetString(3) });
                }
            }
            return list;

        }
    }
}
