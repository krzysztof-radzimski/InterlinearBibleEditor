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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace IBE.Data.Import {
    public class TranslationImporter : BaseImporter<Translation> {
        public override Translation Import(string zipFilePath, UnitOfWork uow) {
            if (File.Exists(zipFilePath)) {
                var fileName = ExtractAndGetFirstArchiveItemFilePath(zipFilePath);
                try {
                    var conn = new SqliteConnection($"DataSource=\"{fileName}\"");
                    SQLitePCL.Batteries.Init();
                    conn.Open();

                    var translation = CreateTranslation(uow, conn, Path.GetFileNameWithoutExtension(fileName));
                    CreateBooks(uow, conn, translation);
                    conn.Close();

                    return translation;
                }
                finally {
                    try { File.Delete(fileName); } catch { }
                }
            }

            return default;
        }


        public Translation ImportInterlinear(string zipFilePath, UnitOfWork uow) {
            if (File.Exists(zipFilePath)) {
                var fileName = ExtractAndGetFirstArchiveItemFilePath(zipFilePath);
                try {
                    var conn = new SqliteConnection($"DataSource=\"{fileName}\"");
                    SQLitePCL.Batteries.Init();
                    conn.Open();

                    var translation = CreateTranslation(uow, conn, Path.GetFileNameWithoutExtension(fileName));
                    CreateBooks(uow, conn, translation, true);
                    conn.Close();

                    return translation;
                }
                finally {
                    try { File.Delete(fileName); } catch { }
                }
            }

            return default;
        }


        private Translation CreateTranslation(UnitOfWork uow, SqliteConnection conn, string transName) {
            var translation = new Translation(uow) {
                Name = transName,
                Type = TranslationType.Default
            };

            using (var command = conn.CreateCommand()) {
                command.CommandText = @"SELECT name, value FROM info";

                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        var name = reader.GetString(0);
                        var value = reader.GetString(1);

                        if (name == "description") { translation.Description = value; }
                        if (name == "detailed_info") { translation.DetailedInfo = value; }
                        if (name == "language") { translation.Language = value.GetLanguage(); }
                        if (name == "chapter_string") { translation.ChapterString = value.IsNotNullOrEmpty() ? value : "Rozdział"; }
                        if (name == "chapter_string_ps") { translation.ChapterPsalmString = value.IsNotNullOrEmpty() ? value : "Psalm"; }
                    }
                }
            }

            if (TableExists(conn, "introductions")) {
                using (var command = conn.CreateCommand()) {
                    command.CommandText = @"SELECT introduction FROM introductions limit 1";

                    using (var reader = command.ExecuteReader()) {
                        while (reader.Read()) {
                            var name = reader.GetString(0);
                            translation.Introduction = name;
                        }
                    }
                }
            }

            translation.Save();

            return translation;
        }
        private void CreateBooks(UnitOfWork uow, SqliteConnection conn, Translation translation, bool interlinear = false) {
            var list = new List<dynamic>();

            using (var command = conn.CreateCommand()) {
                command.CommandText = @"SELECT book_number, short_name, long_name, book_color FROM books";

                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        var numberOfBook = reader.GetInt32(0);
                        var shortName = reader.GetString(1);
                        var longName = reader.GetString(2);
                        var bookColor = reader.GetString(3);

                        list.Add(new {
                            BookName = longName,
                            BookShortcut = shortName,
                            NumberOfBook = numberOfBook,
                            Color = bookColor,
                            BaseBook = new XPQuery<BookBase>(uow).Where(x => x.NumberOfBook == numberOfBook).FirstOrDefault()
                        });

                    }
                }
            }

            foreach (var item in list) {
                var book = new Book(uow) {
                    BookName = item.BookName,
                    BookShortcut = item.BookShortcut,
                    NumberOfBook = item.NumberOfBook,
                    Color = item.Color,
                    BaseBook = item.BaseBook,
                    ParentTranslation = translation
                };
                book.Save();

                CreateChapters(uow, conn, book, interlinear);

                book.Save();
            }
        }
        private void CreateChapters(UnitOfWork uow, SqliteConnection conn, Book book, bool interlinear = false) {
            var list = new List<dynamic>();

            using (var command = conn.CreateCommand()) {
                command.CommandText = $"select distinct chapter from verses where book_number = {book.NumberOfBook}";

                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        var chapterNumber = reader.GetInt32(0);
                        list.Add(new { NumberOfChapter = chapterNumber });
                    }
                }
            }

            book.NumberOfChapters = list.Count;

            foreach (var item in list) {
                var chapter = new Chapter(uow) {
                    NumberOfChapter = item.NumberOfChapter,
                    ParentBook = book
                };
                chapter.Save();

                CreateSubtitles(uow, conn, chapter);
                CreateVerses(uow, conn, chapter, interlinear);

                chapter.Save();
            }
        }
        private void CreateVerses(UnitOfWork uow, SqliteConnection conn, Chapter chapter, bool interlinear = false) {
            var list = new List<dynamic>();

            using (var command = conn.CreateCommand()) {
                command.CommandText = $"select verse, text from verses where book_number = {chapter.ParentBook.NumberOfBook} AND chapter = {chapter.NumberOfChapter}";

                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        var number = reader.GetInt32(0);
                        var text = reader.GetString(1);

                        if (interlinear) {
                            /*
                             <e>βιβλος</e> <n>biblos</n> Zwój – Księga <S>976</S> <m>WTN-NSF</m> 
                             <e>γενεσεως</e> <n>geneseōs</n> narodzenia <S>1078</S> <m>WTN-GSF</m> 
                             <e>ιησου</e> <n>iēsou</n> Jezusa – [JHWH jest zbawieniem] <S>2424</S> <m>WTN-GSM</m> 
                             <e>χριστου</e> <n>christou</n> Pomazańca <S>5547</S> <m>WTN-GSM</m> <e>υιου</e> <n>hyiou</n> syna <S>5207</S> <m>WTN-GSM</m> <e>δαβιδ</e> <n>dabid</n> Dawida – [umiłowany] <S>1138</S> <m>WTN-PRI</m> <e>υιου</e> <n>hyiou</n> syna <S>5207</S> <m>WTN-GSM</m> <e>αβρααμ</e> <n>abraam</n> Abrahama – [ojciec wielu narodów] <S>11</S> <m>WTN-PRI</m>
                             */

                            /*
                             A Pan<S>3068</S> powiedział<S>559</S><S>8799</S> do Kaina<S>7014</S>, gdzie<S>335</S> jest Abel<S>1893</S> twój brat<S>251</S>? A on rzekł:<S>559</S><S>8799</S>, wiem<S>3045</S> Niewłaściwa 08804</S> :: Czy mam mojego brata<S>251</S> opiekun<S>8104</S><S>8802</S>?
                             */

                            // tu trzeba dopisać wyciąganie samego tekstu polskiego z pominięciem tego co po kresce
                            if (chapter.ParentBook.BaseBook.Status.BiblePart == BiblePart.NewTestament) {
                                try {
                                    XElement xml = null;

                                    xml = XElement.Parse($"<verse>{text}</verse>");

                                    if (xml.IsNotNull()) {
                                        text = string.Empty;
                                        foreach (var node in xml.Nodes()) {
                                            if (node.NodeType == System.Xml.XmlNodeType.Text) {
                                                var nodeText = (node as XText).Value.Trim();
                                                if (nodeText.Contains("–")) {
                                                    nodeText = nodeText.Substring(0, nodeText.IndexOf("–") - 1).Trim();
                                                }
                                                text += $"{nodeText} ";
                                            }
                                        }

                                        text = text.Trim();
                                    }
                                }
                                catch (Exception ex) {
                                    if (ex.IsNotNull()) { }
                                }
                            }
                            else {
                                text = System.Text.RegularExpressions.Regex.Replace(text, @"\<S\>[0-9\sa-z]+\<\/S\>", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                text = System.Text.RegularExpressions.Regex.Replace(text, @"\<S\>[0-9\s\w]+\<\/S\>", String.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            }
                        }

                        list.Add(new { NumberOfVerse = number, Text = text });
                    }
                }
            }

            chapter.NumberOfVerses = list.Count;

            foreach (var item in list) {
                var verse = new Verse(uow) {
                    NumberOfVerse = item.NumberOfVerse,
                    Text = item.Text,
                    ParentChapter = chapter
                };
                verse.Save();
            }
        }
        private void CreateSubtitles(UnitOfWork uow, SqliteConnection conn, Chapter chapter) {
            if (!TableExists(conn, "stories")) { return; }
            using (var command = conn.CreateCommand()) {
                command.CommandText = $@"SELECT book_number, chapter, verse, order_if_several, title FROM stories WHERE book_number = {chapter.ParentBook.NumberOfBook} AND chapter = {chapter.NumberOfChapter}";
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        var subtitle = new Subtitle(uow) {
                            BeforeVerseNumber = reader.GetInt32(2),
                            Level = reader.GetInt32(3),
                            ParentChapter = chapter,
                            Text = reader.GetString(4)
                        };
                        subtitle.Save();
                    }
                }
            }
        }
        private bool TableExists(SqliteConnection conn, string tableName) {
            using (var command = conn.CreateCommand()) {
                command.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        var name = reader.GetString(0);
                        return name.IsNotNullOrEmpty();
                    }
                }
            }
            return default;
        }


    }
}