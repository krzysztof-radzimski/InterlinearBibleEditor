using DevExpress.Xpo;
using IBE.Data.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IBE.Data.Import {
    public class TranslationImporter : IImporter {
        public void Import(string filePath, UnitOfWork uow) {
            if (File.Exists(filePath)) {
                using (var conn = new SqliteConnection($"DataSource=\"{filePath}\"")) {
                    SQLitePCL.Batteries.Init();
                    conn.Open();

                    var translation = new Translation(uow) {
                        Name = Path.GetFileNameWithoutExtension(filePath)
                    };

                    var command = conn.CreateCommand();
                    command.CommandText = @"SELECT * FROM info";

                    using (var reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            var name = reader.GetString(0);
                            var value = reader.GetString(1);

                            if (name == "description") { translation.Description = value; }
                            if (name == "detailed_info") { translation.DetailedInfo = value; }
                            if (name == "language") { translation.Language = value; }
                            if (name == "chapter_string") { translation.ChapterString = value; }
                            if (name == "chapter_string_ps") { translation.ChapterPsalmString = value; }
                        }
                    }

                    command = conn.CreateCommand();
                    command.CommandText = @"SELECT introduction FROM introductions limit 1";

                    using (var reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            var name = reader.GetString(0);
                            translation.Introduction = name;
                        }
                    }

                    translation.Save();

                    command = conn.CreateCommand();
                    command.CommandText = @"SELECT * FROM books";

                    var status = GetBookStatus(uow);

                    using (var reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            var numberOfBook = reader.GetInt32(0);
                            var shortName = reader.GetString(1);
                            var longName = reader.GetString(2);
                            var bookColor = reader.GetString(3);

                            var book = GetBook(uow, numberOfBook);
                            if (book == null) {
                                book = new Book(uow) {
                                    BookName = longName,
                                    BookShortcut = shortName,
                                    NumberOfBook = numberOfBook,
                                    Color = bookColor,
                                    Status = status
                                };
                            }
                            else {
                                if (book.Color == null) { book.Color = bookColor; }
                            }

                            command = conn.CreateCommand();
                            command.CommandText = $@"SELECT DISTINCT chapter FROM verses WHERE book_number =  {book.NumberOfBook}";

                            var chapterNumber = 0;
                            using (var readerVerses = command.ExecuteReader()) {
                                Chapter chapter = null;
                                while (readerVerses.Read()) {
                                    chapterNumber = readerVerses.GetInt32(0);
                                    chapter = new XPCollection<Chapter>(uow).Where(x => x.ParentBook == book && x.NumberOfChapter == chapterNumber).FirstOrDefault();
                                    if (chapter == null) {
                                        chapter = new Chapter(uow) {
                                            ParentBook = book,
                                            NumberOfChapter = chapterNumber
                                        };

                                        chapter.Save();
                                    }


                                    AddStories(book, chapter, uow, conn);
                                    AddVerses(book, chapter, translation, uow, conn);
                                }
                            }

                            if (book.NumberOfChapters == 0) { book.NumberOfChapters = chapterNumber; }
                            book.Save();
                        }
                    }
                }
            }
        }

        private void AddVerses(Book book, Chapter chapter, Translation translation, UnitOfWork uow, SqliteConnection conn) {
            var command = conn.CreateCommand();
            command.CommandText = $@"SELECT * FROM verses WHERE book_number = {book.NumberOfBook} AND chapter = {chapter.NumberOfChapter}";

            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {

                    var numberOfVerse = reader.GetInt32(2);

                    Verse verse = null;
                    if (chapter.Verses != null) {
                        verse = chapter.Verses.Where(x => x.NumberOfVerse == numberOfVerse).FirstOrDefault();
                    }

                    if (verse == null) {
                        verse = new Verse(uow) {
                            NumberOfVerse = numberOfVerse,
                            ParentChapter = chapter
                        };
                        verse.Save();
                    }


                    var verseVersion = new VerseVersion(uow) {
                        ParentVerse = verse,
                        Text = reader.GetString(3),
                        TranslationName = translation
                    };

                    verseVersion.Save();
                }
            }
        }

        private void AddStories(Book book, Chapter chapter, UnitOfWork uow, SqliteConnection conn) {
            var command = conn.CreateCommand();
            command.CommandText = $@"SELECT * FROM stories WHERE book_number = {book.NumberOfBook} AND chapter = {chapter.NumberOfChapter}";
            using (var reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    var subtitle = new Subtitle(uow) {
                        BeforeVerseNumber = reader.GetInt32(2),
                        Level = 2,
                        ParentChapter = chapter,
                        Text = reader.GetString(4)
                    };
                    subtitle.Save();
                }
            }
        }

        private Book GetBook(UnitOfWork uow, int numberOfBook) {
            return new XPCollection<Book>(uow).Where(x => x.NumberOfBook == numberOfBook).FirstOrDefault();
        }

        private BookStatus GetBookStatus(UnitOfWork uow) {
            return new XPCollection<BookStatus>(uow).FirstOrDefault();
        }

        public void Dispose() {

        }
    }
}
