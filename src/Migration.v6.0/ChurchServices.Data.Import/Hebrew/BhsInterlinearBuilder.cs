using ChurchServices.Data.Model;
using ChurchServices.Extensions;
using DevExpress.Xpo;
using Microsoft.Data.Sqlite;

namespace ChurchServices.Data.Import.Hebrew {
    public class BHSInterlinearBuilder {
        const string NAME = "BHS+";
        const string EMPTY_TRANSLATION = "---";

        private List<StrongCode> Strongs = null;
        private List<BookBase> BaseBooks = null;
        private List<GrammarCode> GrammarCodes = null;

        public void Build(UnitOfWork uow, string bhsPath) {
            var bhsConnection = GetConnection(bhsPath);
            var bhsVerses = GetBhsVerses(bhsConnection);

            var translation = CreateTranslation(uow);
            Strongs = new XPQuery<StrongCode>(uow).Where(x => x.Lang == Language.Hebrew).ToList();
            BaseBooks = new XPQuery<BookBase>(uow).Where(x => x.Status.BiblePart == BiblePart.OldTestament).OrderBy(x => x.NumberOfBook).ToList();
            GrammarCodes = new XPQuery<GrammarCode>(uow).OrderBy(x => x.GrammarCodeVariant1).ToList();

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
                        Index = $"BHS.{baseBook.NumberOfBook}.{chapterIndex}"
                    };
                    chapter.Save();

                    for (int verseIndex = 1; verseIndex <= versesCount; verseIndex++) {
                        var bhsVerse = bhsVerses.Where(x => x.Book == baseBook.NumberOfBook &&
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
                                Index = $"BHS.{baseBook.NumberOfBook}.{chapterIndex}.{verseIndex}"
                            };
                            verse.Save();

                            var wordIndex = 1;
                            foreach (var word in bhsVerse.Words) {
                                var strong = GetStrongCode(word.StrongCode);
                                var grammar = GetGrammarCode(uow, word.GrammarCode);
                                //var transliteration = HebrewTransliterator.Transliterate(word.Text);
                                var verseWord = new VerseWord(uow) {
                                    NumberOfVerseWord = wordIndex,
                                    ParentVerse = verse,
                                    SourceWord = word.Text,
                                    GrammarCode = grammar,
                                    Translation = "",
                                    Transliteration = "",
                                    StrongCode = strong,

                                };

                                verseWord.Save();
                                wordIndex++;
                            }
                        }
                    }
                }
            }

            uow.CommitChanges();
        }

        private StrongCode GetStrongCode(int code) {
            if (code == 0) { return default; }
            return Strongs.Where(x => x.Code == code).FirstOrDefault();
        }
        private GrammarCode GetGrammarCode(UnitOfWork uow, string grammarCode, string grammarCode2 = null) {
            if (grammarCode.IsNotNullOrEmpty()) {
                var grammarCode1 = grammarCode.Replace(".", "-").ToUpper();
                var result = GrammarCodes.Where(x => x.GrammarCodeVariant1 == grammarCode1).FirstOrDefault();
                return result;
            }
            return default;
        }
        private IEnumerable<BhsVerseInfo> GetBhsVerses(SqliteConnection conn) {
            var list = new List<BhsVerseInfo>();
            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM verses";
            using (var r = command.ExecuteReader()) {
                while (r.Read()) {
                    if (r.IsDBNull(3)) { continue; }
                    var vi = new BhsVerseInfo(r.GetInt32(0), r.GetInt32(1), r.GetInt32(2), r.GetString(3));
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
                    Description = "Biblia Hebraica Stuttgartensia",
                    ChapterPsalmString = "Psalm",
                    ChapterString = "Rozdział",
                    DetailedInfo = "",
                    Language = Language.Polish,
                    Type = TranslationType.Interlinear,
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


    class HebrewTransliterator {
        private static readonly Dictionary<char, string> HebrewToLatinMap = new Dictionary<char, string>
        {
            {'א', "a"}, {'ב', "b"}, {'ג', "g"}, {'ד', "d"}, {'ה', "h"},
            {'ו', "w"}, {'ז', "z"}, {'ח', "ch"}, {'ט', "t"}, {'י', "y"},
            {'כ', "k"}, {'ל', "l"}, {'מ', "m"}, {'נ', "n"}, {'ס', "s"},
            {'ע', "e"}, {'פ', "p"}, {'צ', "tz"}, {'ק', "k"}, {'ר', "r"},
            {'ש', "sz"}, {'ת', "t"}, {'ך', "k"}, {'ם', "m"}, {'ן', "n"},
            {'ף', "f"}, {'ץ', "c"},
            {'ָ', "a"}, {'ַ', "a"}, {'ֶ', "e"}, {'ֵ', "e"}, {'ִ', "i"},
            {'ֹ', "o"}, {'ֻ', "u"}, {'ּ', "u"}, {'ְ', "e"},
            {'ֲ', "a"}, {'ֱ', "e"}, {'ֳ', "o"}, {'ֽ', "e"}, {'ׂ', "s"}, {'ׁ', "sz"}, {'שׁ',"sz"}
        };

        public static string Transliterate(string hebrewText) {
            if (string.IsNullOrEmpty(hebrewText)) {
                return string.Empty;
            }

            char[] hebrewChars = hebrewText.ToCharArray();
            string result = string.Empty;

            foreach (char hebrewChar in hebrewChars) {
                if (HebrewToLatinMap.ContainsKey(hebrewChar)) {
                    result += HebrewToLatinMap[hebrewChar];
                }
                else {
                    result += hebrewChar; // If character is not in the map, leave it as is.
                }
            }

            return result;
        }
    }
}
