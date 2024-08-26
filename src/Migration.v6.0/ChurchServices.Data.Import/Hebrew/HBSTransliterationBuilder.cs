using ChurchServices.Data.Model;
using ChurchServices.Extensions;
using DevExpress.Xpo;
using Microsoft.Data.Sqlite;
using System.Xml.Linq;

namespace ChurchServices.Data.Import.Hebrew {
    public class HBSTransliterationBuilder {
        const string NAME = "HSB+";
        private List<StrongCode> Strongs = null;
        private List<BookBase> BaseBooks = null;

        public void RepairVerseText(UnitOfWork uow) {
            var translation = new XPQuery<Translation>(uow).Where(x => x.Name == NAME).FirstOrDefault();
            if (translation != null) {
                foreach (var book in translation.Books) {
                    foreach (var chapter in book.Chapters) {
                        foreach (var verse in chapter.Verses) {
                            verse.Text = verse.GetSourceText();
                        }
                    }
                }

                uow.CommitChanges();
            }
        }

        public void RepairHNPIStrongCodes(UnitOfWork uow, string path) {
            const string HNPI = "HNPI";
            var translation = new XPQuery<Translation>(uow).Where(x => x.Name == $"{HNPI}+").FirstOrDefault();
            if (translation != null) {
                Strongs = new XPQuery<StrongCode>(uow).Where(x => x.Lang == Language.Hebrew).ToList();
                var connection = GetConnection(path);
                var verses = GetVerses(connection);
                if (translation != null) {
                    foreach (var book in translation.Books) {
                        foreach (var chapter in book.Chapters) {
                            foreach (var verse in chapter.Verses) {
                                var bhsVerse = verses.Where(x => x.Book == book.NumberOfBook &&
                                                            x.Chapter == chapter.NumberOfChapter &&
                                                            x.Verse == verse.NumberOfVerse).FirstOrDefault();

                                var wordIndex = 1;
                                foreach (var bhsWord in bhsVerse.Words) {
                                    var word = verse.VerseWords.Where(x=>x.NumberOfVerseWord == wordIndex).FirstOrDefault();
                                    if (word != null) {
                                        var strong = GetStrongCode(bhsWord.StrongCode);
                                        if (strong != null) {
                                            word.StrongCode = strong;
                                            word.Save();
                                        }
                                    }
                                    wordIndex++;
                                }
                            }
                        }
                    }

                    uow.CommitChanges();
                }
            }
        }


        public void CreateEmptyHB(UnitOfWork uow, string path) {
            const string HNPI = "HNPI";
            var translation = CreateTranslation(uow, $"{HNPI}+", "Hebrajsko-Polski Nowodworski Przekład Interlinearny", Language.Polish, "Rozdział");
            if (translation != null) {
                Strongs = new XPQuery<StrongCode>(uow).Where(x => x.Lang == Language.Hebrew).ToList();
                BaseBooks = new XPQuery<BookBase>(uow).Where(x => x.Status.BiblePart == BiblePart.OldTestament).OrderBy(x => x.NumberOfBook).ToList();
                var connection = GetConnection(path);
                var verses = GetVerses(connection);

                foreach (var baseBook in BaseBooks) {
                    if (verses.Where(x => x.Book == baseBook.NumberOfBook).Count() == 0) {
                        continue;
                    }

                    var chapterCount = verses.Where(x => x.Book == baseBook.NumberOfBook)
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
                        var versesCount = verses.Where(x => x.Book == baseBook.NumberOfBook &&
                                                            x.Chapter == chapterIndex)
                                                .Max(x => x.Verse);
                        var chapter = new Chapter(uow) {
                            NumberOfChapter = chapterIndex,
                            NumberOfVerses = versesCount,
                            ParentBook = book,
                            Index = $"{HNPI}.{baseBook.NumberOfBook}.{chapterIndex}"
                        };
                        chapter.Save();

                        for (int verseIndex = 1; verseIndex <= versesCount; verseIndex++) {
                            var bhsVerse = verses.Where(x => x.Book == baseBook.NumberOfBook &&
                                                             x.Chapter == chapterIndex &&
                                                             x.Verse == verseIndex).FirstOrDefault();

                            var verseText = "";

                            if (bhsVerse != null) {
                                var verse = new Verse(uow) {
                                    NumberOfVerse = verseIndex,
                                    ParentChapter = chapter,
                                    Text = verseText,
                                    Index = $"{HNPI}.{baseBook.NumberOfBook}.{chapterIndex}.{verseIndex}"
                                };
                                verse.Save();

                                var wordIndex = 1;
                                foreach (var word in bhsVerse.Words) {
                                    var strong = GetStrongCode(word.StrongCode);
                                    var verseWord = new VerseWord(uow) {
                                        NumberOfVerseWord = wordIndex,
                                        ParentVerse = verse,
                                        SourceWord = word.Text,
                                        Translation = String.Empty,
                                        Transliteration = word.Transliteration,
                                        StrongCode = strong
                                    };

                                    verseWord.Save();
                                    wordIndex++;
                                }
                            }
                        }
                    }
                }
            }

            uow.CommitChanges();
        }

        public void Build(UnitOfWork uow, string path) {
            var translation = CreateTranslation(uow);
            if (translation != null) {
                Strongs = new XPQuery<StrongCode>(uow).Where(x => x.Lang == Language.Hebrew).ToList();
                BaseBooks = new XPQuery<BookBase>(uow).Where(x => x.Status.BiblePart == BiblePart.OldTestament).OrderBy(x => x.NumberOfBook).ToList();
                var connection = GetConnection(path);
                var verses = GetVerses(connection);

                foreach (var baseBook in BaseBooks) {
                    if (verses.Where(x => x.Book == baseBook.NumberOfBook).Count() == 0) {
                        continue;
                    }

                    var chapterCount = verses.Where(x => x.Book == baseBook.NumberOfBook)
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
                        var versesCount = verses.Where(x => x.Book == baseBook.NumberOfBook &&
                                                            x.Chapter == chapterIndex)
                                                .Max(x => x.Verse);
                        var chapter = new Chapter(uow) {
                            NumberOfChapter = chapterIndex,
                            NumberOfVerses = versesCount,
                            ParentBook = book,
                            Index = $"HSB.{baseBook.NumberOfBook}.{chapterIndex}"
                        };
                        chapter.Save();

                        for (int verseIndex = 1; verseIndex <= versesCount; verseIndex++) {
                            var bhsVerse = verses.Where(x => x.Book == baseBook.NumberOfBook &&
                                                             x.Chapter == chapterIndex &&
                                                             x.Verse == verseIndex).FirstOrDefault();

                            var verseText = "";
                            foreach (var word in bhsVerse.Words) {
                                verseText += word.Text;
                            }

                            if (bhsVerse != null) {
                                var verse = new Verse(uow) {
                                    NumberOfVerse = verseIndex,
                                    ParentChapter = chapter,
                                    Text = verseText,
                                    Index = $"HSB.{baseBook.NumberOfBook}.{chapterIndex}.{verseIndex}"
                                };
                                verse.Save();

                                var wordIndex = 1;
                                foreach (var word in bhsVerse.Words) {
                                    var strong = GetStrongCode(word.StrongCode);
                                    var verseWord = new VerseWord(uow) {
                                        NumberOfVerseWord = wordIndex,
                                        ParentVerse = verse,
                                        SourceWord = word.Text,
                                        Translation = word.Translation,
                                        Transliteration = word.Transliteration,
                                        StrongCode = strong
                                    };

                                    verseWord.Save();
                                    wordIndex++;
                                }
                            }
                        }
                    }
                }
            }

            uow.CommitChanges();
        }
        private StrongCode GetStrongCode(int code) {
            if (code == 0) { return default; }
            return Strongs.Where(x => x.Code == code && x.Lang == Language.Hebrew).FirstOrDefault();
        }
        private List<HBSVerseInfo> GetVerses(SqliteConnection connection) {
            var list = new List<HBSVerseInfo>();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM verses";
            using (var r = command.ExecuteReader()) {
                while (r.Read()) {
                    if (r.IsDBNull(3)) { continue; }
                    var vi = new HBSVerseInfo(r.GetInt32(0), r.GetInt32(1), r.GetInt32(2), r.GetString(3));
                    list.Add(vi);
                }
            }
            return list;
        }

        private SqliteConnection GetConnection(string filePath) {
            var conn = new SqliteConnection($"DataSource=\"{filePath}\"");
            SQLitePCL.Batteries.Init();
            conn.Open();
            return conn;
        }

        private Translation CreateTranslation(UnitOfWork uow, string name = NAME, string description = "Hebrew Study Bible based on Westminster Leningrad Codex", Language lang = Language.English, string chapterString = "Chapter") {
            var result = new XPQuery<Translation>(uow).Where(x => x.Name == name).FirstOrDefault();
            if (result.IsNull()) {
                result = new Translation(uow) {
                    Name = name,
                    Description = description,
                    ChapterPsalmString = "Psalm",
                    ChapterString = chapterString,
                    DetailedInfo = "",
                    Language = lang,
                    Type = TranslationType.Interlinear,
                    BookType = TheBookType.Bible,
                    Introduction = "",
                    Hidden = true
                };
                result.Save();
            }
            return result;
        }
    }

    class HBSVerseInfo {
        /*
           <e>בְּרֵאשִׁ֖ית</e> <S>7225</S> <n>be-re-Shit</n> In the beginning 
           <e>בָּרָ֣א</e> <S>1254</S> <n>ba-Ra</n> created 
           <e>אֱלֹהִ֑ים</e> <S>430</S> <n>E-lo-Him;</n> God 
           <e>אֵ֥ת</e> <S>853</S> <n>'et</n> 
           <e>הַשָּׁמַ֖יִם</e> <S>8064</S> <n>hash-sha-Ma-yim</n> the heaven 
           <e>וְאֵ֥ת</e> <S>853</S> <n>ve-'Et</n> 
           <e>הָאָֽרֶץ׃</e> <S>776</S> <n>ha-'A-retz.</n> the earth     
         */
        public List<HBSVerseWordInfo> Words { get; set; }
        public int Book { get; private set; }
        public int Chapter { get; private set; }
        public int Verse { get; private set; }

        public HBSVerseInfo(int book, int chapter, int verse, string data) {
            Words = new List<HBSVerseWordInfo>();
            Book = book;
            Chapter = chapter;
            Verse = verse;

            if (data.IsNotNullOrEmpty()) {
                var wordIndex = 1;

                data = data.Replace("</n></n>", "</n>")
                           .Replace("</n><n>", "</n>");

                var xmlText = $"<verse>{data}</verse>";
                XElement xml = null;
                try {
                    xml = XElement.Parse(xmlText);
                }
                catch (Exception ex) {
                    if (ex != null) {

                    }
                    xml = null;
                }

                HBSVerseWordInfo word = null;
                foreach (var node in xml.Nodes()) {
                    if (node.NodeType == System.Xml.XmlNodeType.Text) {
                        var text = (node as XText).Value;
                        word.Translation = text;
                    }
                    else if (node.NodeType == System.Xml.XmlNodeType.Element) {
                        XElement el = node as XElement;
                        if (el.Name.LocalName == "S") {
                            var code = Convert.ToInt32(el.Value.Trim());
                            word.StrongCode = code;
                        }
                        else if (el.Name.LocalName == "n") {
                            word.Transliteration = el.Value.ToLower()
                                .Replace("-", "")
                                .Replace("sh", "sz")
                                .Replace("y", "j")
                                .Replace("v", "w")
                                .Replace("ch", "h")
                                .Replace("ph", "f")
                                .Replace("tz", "c");
                        }
                        else if (el.Name.LocalName == "e") {
                            word = new HBSVerseWordInfo() {
                                WordIndex = wordIndex,
                                Text = el.Value
                            };
                            Words.Add(word);
                            wordIndex++;
                        }
                    }
                }
            }
        }
    }

    class HBSVerseWordInfo {
        public string Text { get; set; }
        public int StrongCode { get; set; }
        public int WordIndex { get; set; }
        public string Transliteration { get; set; }
        public string Translation { get; set; }
        public override string ToString() {
            return Text;
        }
    }
}
