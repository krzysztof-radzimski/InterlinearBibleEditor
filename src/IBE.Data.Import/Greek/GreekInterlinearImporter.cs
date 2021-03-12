using DevExpress.Xpo;
using IBE.Data.Import.Extensions;
using IBE.Data.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IBE.Data.Import.Greek {
    //public class GreekInterlinearBuilder {
    //}

    public class GreekNewTestamentInterlinearBuilder {
        const string NAME = "NPI+";
        const string EMPTY_TRANSLATION = "---";
        private IEnumerable<ByzVerseInfo> ByzVerses = null;
        private IEnumerable<TroVerseInfo> TroVerses = null;
        private IEnumerable<PbpwVerseInfo> PbpwVerses = null;
        private IQueryable<StrongCode> Strongs = null;
        private IQueryable<BookBase> BaseBooks = null;
        private IQueryable<GrammarCode> GrammarCodes = null;

        public void Build(UnitOfWork uow, string byzPath, string troPath, string pbpwPath) {
            var byzConnection = GetConnection(byzPath);
            var troConnection = GetConnection(troPath);
            var pbpwConnection = GetConnection(pbpwPath);

            var translation = CreateTranslation(uow);

            TroVerses = GetTroVerseInfos(troConnection);
            ByzVerses = GetByzVerses(byzConnection);
            PbpwVerses = GetPbpwVerseInfos(pbpwConnection);
            Strongs = new XPQuery<StrongCode>(uow).Where(x => x.Lang == Language.Greek);
            BaseBooks = new XPQuery<BookBase>(uow).Where(x => x.Status.BiblePart == BiblePart.NewTestament).OrderBy(x => x.NumberOfBook);
            GrammarCodes = new XPQuery<GrammarCode>(uow).OrderBy(x => x.GrammarCodeVariant1);

            foreach (var baseBook in BaseBooks) {
                var chapterCount = ByzVerses.Where(x => x.Book == baseBook.NumberOfBook)
                                            .Max(x => x.Chapter);
                Book book = null;
                if (book.IsNull()) {
                    book = new Book(uow) {
                        BaseBook = baseBook,
                        BookName = baseBook.BookName,
                        BookShortcut = baseBook.BookShortcut,
                        Color = baseBook.Color,
                        NumberOfBook = baseBook.NumberOfBook,
                        NumberOfChapters = chapterCount,
                        ParentTranslation = translation
                    };
                    book.Save();
                    uow.CommitChanges();
                    uow.ReloadChangedObjects();

#if DEBUG
                    System.Diagnostics.Debug.WriteLine("-----------------------------------------------");
                    System.Diagnostics.Debug.Write($"{baseBook.BookTitle}");
#endif
                }

                for (int chapterIndex = 1; chapterIndex <= chapterCount; chapterIndex++) {
                    var versesCount = ByzVerses.Where(x => x.Book == baseBook.NumberOfBook &&
                                                           x.Chapter == chapterIndex)
                                               .Max(x => x.Verse);
                    Chapter chapter = null;
                    if (chapter.IsNull()) {
                        chapter = new Chapter(uow) {
                            NumberOfChapter = chapterIndex,
                            NumberOfVerses = versesCount,
                            ParentBook = book
                        };
                        chapter.Save();
                        uow.CommitChanges();
                        uow.ReloadChangedObjects();
#if DEBUG
                        System.Diagnostics.Debug.WriteLine("");
                        System.Diagnostics.Debug.Write($"Rozdział {chapterIndex}");
#endif
                    }
                    for (int verseIndex = 1; verseIndex <= versesCount; verseIndex++) {
                        Verse verse = null;
                        if (verse.IsNull()) {
                            var pbpwVerse = PbpwVerses.Where(x => x.Book == baseBook.NumberOfBook &&
                                                                  x.Chapter == chapterIndex &&
                                                                  x.Verse == verseIndex).FirstOrDefault();
                            verse = new Verse(uow) {
                                NumberOfVerse = verseIndex,
                                ParentChapter = chapter,
                                Text = pbpwVerse.IsNotNull() ? pbpwVerse.VerseText : String.Empty
                            };
                            verse.Save();
                            uow.CommitChanges();
                            uow.ReloadChangedObjects();
#if DEBUG
                            System.Diagnostics.Debug.WriteLine("");
                            System.Diagnostics.Debug.Write($"{verseIndex}. ");
#endif
                        }

                        var vi = ByzVerses.Where(x => x.Book == baseBook.NumberOfBook &&
                                                      x.Chapter == chapterIndex &&
                                                      x.Verse == verseIndex).FirstOrDefault();
                        if (vi.IsNotNull()) {
                            var ti = GetTroVerseInfo(vi);
                            if (ti.IsNotNull()) {
                                var wordIndex = 1;
                                foreach (var viWord in vi.Words) {
                                    var tiWord = ti.Words.Where(x => x.WordIndex == viWord.WordIndex).FirstOrDefault();
                                    if (tiWord.IsNotNull()) {
                                        if (tiWord.StrongCode != viWord.StrongCode) {

                                            // może kod się różni ale dla pewności porównam transliterację
                                            if (!Compare(viWord, tiWord)) {
                                                // tu trzeba sprawdzić czy to słowo nie występuje gdzieś dalej
                                                // wówczas trzeba przetłumaczyć słowa w oparciu o wcześniej
                                                // zbudowany słownik aż do wystąpienia tego słowa w tekście 
                                                // TRO



                                                tiWord = ti.Words.Where(x => x.StrongCode == viWord.StrongCode &&
                                                                         x.GrammarCode != null &&
                                                                         x.GrammarCode.Contains(viWord.GrammarCode) &&
                                                                         x.WordIndex >= viWord.WordIndex).FirstOrDefault();
                                                if (tiWord.IsNull()) {
                                                    tiWord = ti.Words.Where(x => x.StrongCode == viWord.StrongCode &&
                                                                                 x.GrammarCode != null &&
                                                                                 x.GrammarCode.Contains(viWord.GrammarCode)).FirstOrDefault();
                                                }

                                                if (tiWord.IsNull()) {
                                                    tiWord = new TroVerseWordInfo() {
                                                        GrammarCode = viWord.GrammarCode,
                                                        Text = viWord.Text,
                                                        StrongCode = viWord.StrongCode,
                                                        Translation = viWord.Text.TransliterateAncientGreek(),
                                                        Transliterit = viWord.Text.TransliterateAncientGreek(),
                                                        WordIndex = viWord.WordIndex
                                                    };
                                                }
                                            }
                                        }
                                    }
                                    else {
                                        tiWord = new TroVerseWordInfo() {
                                            GrammarCode = viWord.GrammarCode,
                                            Text = viWord.Text,
                                            StrongCode = viWord.StrongCode,
                                            Translation = EMPTY_TRANSLATION,
                                            Transliterit = viWord.Text.TransliterateAncientGreek(),
                                            WordIndex = viWord.WordIndex
                                        };
                                    }

                                    VerseWord word = null;
                                    if (word.IsNull()) {
                                        word = new VerseWord(uow) {
                                            GrammarCode = GetGrammarCode(uow, viWord.GrammarCode, tiWord.IsNotNull() ? tiWord.GrammarCode : null),
                                            StrongCode = GetStrongCode(viWord.StrongCode),
                                            NumberOfVerseWord = wordIndex,
                                            ParentVerse = verse,
                                            SourceWord = viWord.Text,
                                            Translation = tiWord.IsNotNull() ? tiWord.Translation : String.Empty,
                                            Transliteration = tiWord.IsNotNull() ? tiWord.Transliterit : viWord.Text.TransliterateAncientGreek()
                                        };
                                        word.Save();
                                        uow.CommitChanges();
                                        uow.ReloadChangedObjects();

#if DEBUG
                                        System.Diagnostics.Debug.Write($"{word.Translation} ");
#endif
                                    }
                                    wordIndex++;
                                }
                            }
                        }
                    }
                }
            }
        }

        private bool Compare(ByzVerseWordInfo viWord, TroVerseWordInfo tiWord) {
            var viWordTransliterit = viWord.Text.TransliterateAncientGreek().ToLower();
            var tiWordTransliterit = tiWord.Text.TransliterateAncientGreek().ToLower();
            if (viWordTransliterit.Equals(tiWordTransliterit)) { return true; }

            if ($"h{viWordTransliterit}".Equals(tiWordTransliterit)) { return true; }
            if (viWordTransliterit.Equals($"h{tiWordTransliterit}")) { return true; }
            if (tiWordTransliterit.Length > 1 && viWordTransliterit.Equals($"hy{tiWordTransliterit.Substring(1)}")) { return true; }

            // jeszcze trzeba opracować przypadek hymin / umin

            return default;
        }

        public void Test(string byzPath) {
            var conn = GetConnection(byzPath);
            var result = GetByzVerses(conn);
            if (result != null) {

            }
        }
        private IEnumerable<ByzVerseInfo> GetByzVerses(SqliteConnection conn) {
            var list = new List<ByzVerseInfo>();
            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM verses";
            using (var r = command.ExecuteReader()) {
                while (r.Read()) {
                    if (r.IsDBNull(3)) { continue; }
                    var vi = new ByzVerseInfo(r.GetInt32(0), r.GetInt32(1), r.GetInt32(2), r.GetString(3));
                    list.Add(vi);
                }
            }
            return list;
        }
        private GrammarCode GetGrammarCode(UnitOfWork uow, string grammarCode, string grammarCode2 = null) {
            //XPQuery<GrammarCode> q = new XPQuery<GrammarCode>(uow);
            var result = GrammarCodes.Where(x => x.GrammarCodeVariant1 == grammarCode || x.GrammarCodeVariant1 == grammarCode || x.GrammarCodeVariant3 == grammarCode).FirstOrDefault();
            if (result == null) {
                result = new GrammarCode(uow) {
                    GrammarCodeVariant1 = grammarCode,
                    GrammarCodeVariant2 = grammarCode2
                };
                result.Save();
                uow.CommitChanges();
                uow.ReloadChangedObjects();
            }
            return result;
        }
        private StrongCode GetStrongCode(int code) {
            return Strongs.Where(x => x.Code == code && x.Lang == Language.Greek).FirstOrDefault();
        }

        private IEnumerable<NestleVerseInfo> GetNestleVerses(SqliteConnection nestleConnection) {
            var list = new List<NestleVerseInfo>();
            var nestleVersesCommand = nestleConnection.CreateCommand();
            nestleVersesCommand.CommandText = "SELECT * FROM verses";
            using (var r = nestleVersesCommand.ExecuteReader()) {
                while (r.Read()) {
                    if (r.IsDBNull(3)) { continue; }
                    var vi = new NestleVerseInfo(r.GetInt32(0), r.GetInt32(1), r.GetInt32(2), r.GetString(3));
                    list.Add(vi);
                }
            }
            return list;
        }
        private TroVerseInfo GetTroVerseInfo(ByzVerseInfo vi) {
            if (vi.IsNotNull()) {
                return TroVerses.Where(x => x.Book == vi.Book && x.Chapter == vi.Chapter && x.Verse == vi.Verse).FirstOrDefault();
            }

            return default;
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
        private Translation CreateTranslation(UnitOfWork uow) {
            var result = new XPQuery<Translation>(uow).Where(x => x.Name == NAME).FirstOrDefault();
            if (result.IsNull()) {
                result = new Translation(uow) {
                    Name = NAME,
                    Description = "Nowodworski Grecko-Polski Interlinearny Przekład Pisma Świętego Nowego i Starego Przymierza",
                    ChapterPsalmString = "Psalm",
                    ChapterString = "Rozdział",
                    DetailedInfo = @"W pracach nad przekładem wykorzystano:
<ul>
    <li>Interlinearny Przekład Nowego Testamentu Textus Receptus Oblubienicy</li>
    <li>Przekład Grecki Nestle 1904 Greek New Testament z kodami Stronga</li>
    <li>Przekład NT Popowskiego i Wojciechowskiego</li>
    <li>Analytic Septuagint z kodami Stronga i imieniem Bożym JHWH</li>
</ul>
",
                    Language = Language.Polish,
                    Type = TranslationType.Interlinear,
                    Introduction = ""
                };
                result.Save();
                uow.CommitChanges();
                uow.ReloadChangedObjects();
            }
            return result;
        }
        private SqliteConnection GetConnection(string filePath) {
            var conn = new SqliteConnection($"DataSource=\"{filePath}\"");
            SQLitePCL.Batteries.Init();
            conn.Open();
            return conn;
        }
        private IEnumerable<PbpwVerseInfo> GetPbpwVerseInfos(SqliteConnection conn) {
            var list = new List<PbpwVerseInfo>();
            var command = conn.CreateCommand();
            command.CommandText = $"SELECT * FROM verses";
            using (var r = command.ExecuteReader()) {
                while (r.Read()) {
                    list.Add(new PbpwVerseInfo(r.GetInt32(0), r.GetInt32(1), r.GetInt32(2), r.GetString(3)));
                }
            }
            return list;
        }

    }

    //public class GreekSeptuagintImporter : BaseImporter {
    //    public override void Import(string filePath, UnitOfWork uow) {
    //        if (File.Exists(filePath)) {
    //            using (var conn = new SqliteConnection($"DataSource=\"{filePath}\"")) {
    //                SQLitePCL.Batteries.Init();
    //                conn.Open();

    //                var translation = new Translation(uow) {
    //                    Name = "Grecko-Polski Interlinearny Przekład Pisma Świętego Nowego i Starego Przymierza"
    //                };

    //                // Create translation
    //                {
    //                    var command = conn.CreateCommand();
    //                    command.CommandText = @"SELECT * FROM info";

    //                    using (var reader = command.ExecuteReader()) {
    //                        while (reader.Read()) {
    //                            var name = reader.GetString(0);
    //                            var value = reader.GetString(1);

    //                            if (name == "description") { translation.Description = value; }
    //                            if (name == "detailed_info") { translation.DetailedInfo = value; }
    //                            if (name == "language") { translation.Language = value.GetLanguage(); }
    //                            if (name == "chapter_string") { translation.ChapterString = value; }
    //                            if (name == "chapter_string_ps") { translation.ChapterPsalmString = value; }
    //                        }
    //                    }

    //                    translation.Save();
    //                }

    //                // Add books
    //                {
    //                    var command = conn.CreateCommand();
    //                    command.CommandText = @"SELECT * FROM books";

    //                    using (var reader = command.ExecuteReader()) {
    //                        while (reader.Read()) {
    //                            var numberOfBook = reader.GetInt32(1);
    //                            var shortName = reader.GetString(2);
    //                            var longName = reader.GetString(3);
    //                            var bookColor = reader.GetString(0);

    //                            var book = new Book(uow) {
    //                                BookName = longName,
    //                                BookShortcut = shortName,
    //                                NumberOfBook = numberOfBook,
    //                                Color = bookColor,
    //                                Status = GetBookStatus(uow)
    //                            };

    //                            book.Save();

    //                            // Add chapters
    //                            AddChapters(conn, uow, book);
    //                        }
    //                    }
    //                }
    //                conn.Close();
    //            }
    //        }
    //    }

    //    private void AddChapters(SqliteConnection conn, UnitOfWork uow, Book book) {
    //        var numberOfChapters = 0;

    //        {
    //            var command = conn.CreateCommand();
    //            command.CommandText = $@"select distinct max(chapter) FROM verses where book_number = {book.NumberOfBook}";
    //            using (var reader = command.ExecuteReader()) {
    //                while (reader.Read()) {
    //                    numberOfChapters = reader.GetInt32(0);
    //                }
    //            }
    //        }

    //        book.NumberOfChapters = numberOfChapters;
    //        for (int i = 1; i < numberOfChapters + 1; i++) {
    //            var command = conn.CreateCommand();
    //            command.CommandText = $@"SELECT * FROM verses WHERE book_number = {book.NumberOfBook} AND chapter = {i}";

    //            var chapter = new Chapter(uow) {
    //                NumberOfChapter = i,
    //                ParentBook = book
    //            };

    //            chapter.Save();

    //            using (var reader = command.ExecuteReader()) {
    //                while (reader.Read()) {
    //                    var numberOfVerse = reader.GetInt32(2);
    //                    var text = reader.GetString(3);

    //                    /*
    //                    Ἐν<S>1722</S><m>PREP</m> 
    //                    ἀρχῇ<S>746</S><m>N-DSF</m> 
    //                    ἐποίησεν<S>4160</S><m>V-AAI-3S</m> 
    //                    ὁ<S>3588</S><m>T-NSM</m> 
    //                    θεὸς<S>2316</S><m>N-NSM</m> 
    //                    τὸν<S>3588</S><m>T-ASM</m> 
    //                    οὐρανὸν<S>3772</S><m>N-ASM</m> 
    //                    καὶ<S>2532</S><m>CONJ</m> 
    //                    τὴν<S>3588</S><m>T-ASF</m> 
    //                    γῆν<S>1065</S><m>N-ASF</m>.   

    //                    ἡ<S>3588</S><m>T-NSF</m> 
    //                    δὲ<S>1161</S><m>PRT</m> 
    //                    γῆ<S>1065</S><m>N-NSF</m> 
    //                    ἦν<S>1510</S><m>V-IAI-3S</m> 
    //                    ἀόρατος<S>517</S><m>A-NSM</m> 
    //                    καὶ<S>2532</S><m>CONJ</m> 
    //                    ἀκατασκεύαστος<m>A-NSM</m>, 
    //                    καὶ<S>2532</S><m>CONJ</m> 
    //                    σκότος<S>4655</S><m>N-NSN</m> 
    //                    ἐπάνω<S>1883</S><m>PREP</m> 
    //                    τῆς<S>3588</S><m>T-GSF</m> 
    //                    ἀβύσσου<S>12</S><m>N-GSF</m>, 
    //                    καὶ<S>2532</S><m>CONJ</m> 
    //                    πνεῦμα<S>4151</S><m>N-NSN</m> 
    //                    θεοῦ<S>2316</S><m>N-GSM</m> 
    //                    ἐπεφέρετο<S>2018</S><m>V-IMI-3S</m> 
    //                    ἐπάνω<S>1883</S><m>PREP</m> 
    //                    τοῦ<S>3588</S><m>T-GSN</m> 
    //                    ὕδατος<S>5204</S><m>N-GSN</m>.
    //                    */

    //                    var verse = new Verse(uow) {
    //                        NumberOfVerse = numberOfVerse,
    //                        ParentChapter = chapter
    //                    };

    //                    verse.Save();


    //                    VerseWord word = null;
    //                    int numberOfVerseWord = 0;
    //                    var xml = XElement.Parse($"<verse>{text.Trim()}</verse>");
    //                    foreach (var node in xml.Nodes()) {
    //                        if (node.NodeType == System.Xml.XmlNodeType.Text) {
    //                            numberOfVerseWord++;
    //                            word = new VerseWord(uow) {
    //                                NumberOfVerseWord = numberOfVerseWord,
    //                                SourceWord = (node as XText).Value,
    //                                ParentVerse = verse,
    //                                Transliteration = (node as XText).Value.TransliterateAncientGreek()
    //                            };
    //                            word.Save();
    //                        }
    //                        else if (node.NodeType == System.Xml.XmlNodeType.Element) {
    //                            XElement el = node as XElement;
    //                            if (el.Name.LocalName == "S") {
    //                                var code = Convert.ToInt32(el.Value.Trim());
    //                                word.StrongCode = GetStrongCode(uow, code);
    //                            }
    //                            else if (el.Name.LocalName == "M") {
    //                                word.GrammarCode = GetGrammarCode(uow, el.Value.Trim());
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    private GrammarCode GetGrammarCode(UnitOfWork uow, string grammarCode) {
    //        var result = new XPCollection<GrammarCode>(uow).Where(x => x.GrammarCodeVariant1 == grammarCode || x.GrammarCodeVariant1 == grammarCode || x.GrammarCodeVariant3 == grammarCode).FirstOrDefault();
    //        if (result == null) {
    //            result = new GrammarCode(uow) { GrammarCodeVariant1 = grammarCode };
    //            result.Save();
    //        }
    //        return result;
    //    }
    //    private StrongCode GetStrongCode(UnitOfWork uow, int code) {
    //        return new XPCollection<StrongCode>(uow).Where(x => x.Code == code && x.Lang == Language.Greek).FirstOrDefault();
    //    }
    //    private BookStatus GetBookStatus(UnitOfWork uow, BiblePart part = BiblePart.OldTestament, CanonType canonType = CanonType.Canon) {
    //        var result = new XPCollection<BookStatus>(uow).Where(x => x.CanonType == canonType && x.BiblePart == part).FirstOrDefault();
    //        if (result.IsNull()) {
    //            result = new BookStatus(uow) {
    //                BiblePart = part,
    //                CanonType = canonType
    //            };
    //            result.Save();
    //        }
    //        return result;
    //    }
    //}
}
