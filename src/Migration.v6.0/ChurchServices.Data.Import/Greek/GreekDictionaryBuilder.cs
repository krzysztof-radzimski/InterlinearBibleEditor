using DevExpress.Xpo;
using ChurchServices.Extensions;
using ChurchServices.Data.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ChurchServices.Data.Import.Greek {
    public class GreekDictionaryBuilder : IDisposable {
        private AncientDictionary Dictionary = null;
        //private IEnumerable<NestleVerseInfo> NestleVerses = null;
        private IEnumerable<TroVerseInfo> TroVerses = null;
        private IQueryable<StrongCode> Strongs = null;
        private IEnumerable<ByzVerseInfo> ByzVerses = null;

        public void Build(UnitOfWork uow, string byzPath, string troPath) {
            var dic = GetDictionary(uow);
            var byzConnection = GetConnection(byzPath);
            var troConnection = GetConnection(troPath);

            TroVerses = GetTroVerseInfos(troConnection);
            Strongs = new XPQuery<StrongCode>(uow).Where(x => x.Lang == Language.Greek);
            ByzVerses = GetByzVerses(byzConnection);

#if DEBUG
            System.Diagnostics.Debug.WriteLine($"Budowanie modelu...");
#endif
            var info = GetDictionaryItemInfos();
            foreach (var item in info) {
                var word = new AncientDictionaryItem(uow) {
                    Dictionary = this.Dictionary,
                    GrammarCode = GetGrammarCode(uow, item.GrammarCode, item.GrammarCode2),
                    StrongCode = GetStrongCode(item.StrongCode),
                    Word = item.Text,
                    Translation = item.Translation,
                    Transliteration = item.Transliterit
                };
                foreach (var reference in item.References) {
                    word.VersesReferences.Add(new VerseInfo(uow) {
                        NumberOfBook = reference.Book,
                        NumberOfChapter = reference.Chapter,
                        NumberOfVerse = reference.Verse,
                        BaseBook = new XPQuery<BookBase>(uow).Where(x => x.NumberOfBook == reference.Book).FirstOrDefault()
                    });
                }
                word.Save();               

#if DEBUG
                System.Diagnostics.Debug.WriteLine($"{item.Text}");
#endif
            }


            //            foreach (var vi in ByzVerses) {
            //                var ti = GetTroVerseInfo(vi);
            //                if (ti.IsNotNull()) {
            //                    var verseInfo = new XPQuery<VerseInfo>(uow).Where(x => x.NumberOfBook == vi.Book && x.NumberOfChapter == vi.Chapter && x.NumberOfVerse == vi.Verse).FirstOrDefault();
            //                    if (verseInfo.IsNull()) {
            //                        verseInfo = new VerseInfo(uow) {
            //                            NumberOfBook = vi.Book,
            //                            NumberOfChapter = vi.Chapter,
            //                            NumberOfVerse = vi.Verse,
            //                            BaseBook = new XPQuery<BookBase>(uow).Where(x => x.NumberOfBook == vi.Book).FirstOrDefault()
            //                        };
            //                        verseInfo.Save();
            //                        uow.CommitChanges();
            //                        uow.ReloadChangedObjects();
            //                    }

            //                    foreach (var word in vi.Words) {
            //                        var tiWord = ti.Words.Where(x => x.StrongCode == word.StrongCode && x.GrammarCode != null && x.GrammarCode.Contains(word.GrammarCode)).FirstOrDefault();
            //                        if (tiWord.IsNotNull()) {
            //                            if (AddDictionaryItem(uow, word, tiWord, verseInfo)) {
            //#if DEBUG
            //                                System.Diagnostics.Debug.WriteLine($"Dodano słowo {word.Text} do słownika");
            //#endif
            //                            }
            //                        }
            //                    }
            //                }
            //            }

            System.Diagnostics.Debug.WriteLine($"Zapisywanie słownika");

            uow.CommitChanges();
            uow.ReloadChangedObjects();

            System.Diagnostics.Debug.WriteLine($"Zakończono budowanie słownika");


            /*
select 
	ancientdictionaryitem.Word as Słowo,
	ancientdictionaryitem.Transliteration as Transliteracja,
	ancientdictionaryitem.Translation as Tłumaczenie,	
	StrongCode.Code as [Kod Stronga],
	StrongCode.Transliteration as [Transliteracja wg Stronga],
	StrongCode.SourceWord as [Źródłosłów],
	StrongCode.ShortDefinition	as [Definicja]
from ancientdictionaryitem, StrongCode  
where 1=1 
AND StrongCode.Lang = 2 
AND ancientdictionaryitem.StrongCode = StrongCode.OID
order by ancientdictionaryitem.Word             
             */
        }

        private IEnumerable<DictionaryItemInfo> GetDictionaryItemInfos() {
            var list = new List<DictionaryItemInfo>();

            var _troWords = TroVerses.SelectMany(x => x.Words).Where(x => x.Translation.IsNotNullOrEmpty()).OrderBy(x => x.Text);
            var troWords = _troWords.Select(o => new TroTempInfo { Text = o.Text, Translation = o.Translation, Transliterit = o.Transliterit2, GrammarCode = o.GrammarCode }).Distinct().ToList();
            foreach (var vi in ByzVerses) {
                foreach (var word in vi.Words) {
                    var item = list.Where(x => x.Text == word.Text.ToLower()).FirstOrDefault();
                    if (item.IsNotNull()) {
                        item.References.Add(new DictionaryItemInfoVerseReference() {
                            Book = vi.Book,
                            Chapter = vi.Chapter,
                            Verse = vi.Verse
                        });
                    }
                    else {
                        item = new DictionaryItemInfo(word.Text.ToLower(), word.StrongCode, word.GrammarCode);

                        var tro = GetTroVerseWordInfo(troWords, word.Text);
                        if (tro.IsNotNull()) {
                            item.Transliterit = tro.Transliterit;
                            item.Translation = tro.Translation;
                            item.GrammarCode2 = tro.GrammarCode;
                        }
                        else {
                            continue;
                            //item.Transliterit = word.Text.TransliterateAncientGreek().ToLower();
                            //item.Translation = String.Empty;
                        }

                        item.References.Add(new DictionaryItemInfoVerseReference() {
                            Book = vi.Book,
                            Chapter = vi.Chapter,
                            Verse = vi.Verse
                        });
                        list.Add(item);
                    }

                }
            }
            return list.OrderBy(x => x.Text);
        }

        private TroTempInfo GetTroVerseWordInfo(IEnumerable<TroTempInfo> data, string word) {
            var trans1 = word.TransliterateAncientGreek().ToLower();
            var _word = word.ToLower();
            return data.Where(x => CompareGreek(_word, trans1, x.Text, x.Transliterit)).FirstOrDefault();
        }
        private bool CompareGreek(string dicWord, string dicWordTrans, string troWord, string troTrans) {
            if (dicWord.Equals(troWord.ToLower())) { return true; }

            if (dicWordTrans.Equals(troTrans)) { return true; }
            if ($"h{dicWordTrans}".Equals(troTrans)) { return true; }
            if (dicWordTrans.Equals($"h{troTrans}")) { return true; }
            if (troTrans.Length > 1 && dicWordTrans.Equals($"hy{troTrans.Substring(1)}")) { return true; }
            return default;
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
        private SqliteConnection GetConnection(string filePath) {
            var conn = new SqliteConnection($"DataSource=\"{filePath}\"");
            SQLitePCL.Batteries.Init();
            conn.Open();
            return conn;
        }
        private TroVerseInfo GetTroVerseInfo(ByzVerseInfo vi) {
            return TroVerses.Where(x => x.Book == vi.Book && x.Chapter == vi.Chapter && x.Verse == vi.Verse).FirstOrDefault();
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
        private AncientDictionary GetDictionary(UnitOfWork uow) {
            if (Dictionary.IsNull()) {
                XPQuery<AncientDictionary> q = new XPQuery<AncientDictionary>(uow);
                Dictionary = q.Where(x => x.Language == Language.Greek).FirstOrDefault();
                if (Dictionary.IsNull()) {
                    Dictionary = new AncientDictionary(uow) { Language = Language.Greek };
                    Dictionary.Save();
                    uow.CommitChanges();
                    uow.ReloadChangedObjects();
                }
            }
            return Dictionary;
        }
        private bool AddDictionaryItem(UnitOfWork uow, VerseWordInfo nestleVerseWordInfo, TroVerseWordInfo troVerseWordInfo, VerseInfo verseInfo) {
            XPQuery<AncientDictionaryItem> q = new XPQuery<AncientDictionaryItem>(uow);
            if (troVerseWordInfo.Translation.IsNotNullOrEmpty()) {
                var word = q.Where(x => x.Word == nestleVerseWordInfo.Text).FirstOrDefault();
                if (word.IsNull()) {
                    word = new AncientDictionaryItem(uow) {
                        Dictionary = this.Dictionary,
                        GrammarCode = GetGrammarCode(uow, nestleVerseWordInfo.GrammarCode, troVerseWordInfo.GrammarCode),
                        StrongCode = GetStrongCode(nestleVerseWordInfo.StrongCode),
                        Word = nestleVerseWordInfo.Text,
                        Translation = troVerseWordInfo.Translation,
                        Transliteration = troVerseWordInfo.Transliterit
                    };
                    word.VersesReferences.Add(verseInfo);
                    word.Save();
                    uow.CommitChanges();
                    uow.ReloadChangedObjects();

                    return true;
                }
                else {
                    word.VersesReferences.Add(verseInfo);
                    word.Save();
                }
            }
            return default;
        }
        private GrammarCode GetGrammarCode(UnitOfWork uow, string grammarCode, string grammarCode2) {
            XPQuery<GrammarCode> q = new XPQuery<GrammarCode>(uow);
            var result = q.Where(x => x.GrammarCodeVariant1 == grammarCode || x.GrammarCodeVariant2 == grammarCode || x.GrammarCodeVariant3 == grammarCode).FirstOrDefault();
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
        public void Dispose() { }
    }

    class TroTempInfo : IEquatable<TroTempInfo> {
        public string Text { get; set; }
        public string Transliterit { get; set; }
        public string Translation { get; set; }
        public string GrammarCode { get; set; }

        public override int GetHashCode() {
            int hashText = Text == null ? 0 : Text.GetHashCode();
            int hashGrammarCode = GrammarCode == null ? 0 : GrammarCode.GetHashCode();

            return hashText ^ hashGrammarCode;
        }
        public bool Equals(TroTempInfo other) {
            return Text.Equals(other.Text) && GrammarCode.Equals(other.GrammarCode);
        }
    }

    class DictionaryItemInfo {
        public string Text { get; set; }
        public int StrongCode { get; set; }
        public string GrammarCode { get; set; }
        public string GrammarCode2 { get; set; }
        public string Transliterit { get; set; }
        public string Translation { get; set; }
        public List<DictionaryItemInfoVerseReference> References { get; }
        public DictionaryItemInfo(string text, int strongcode, string grammarCode) {
            References = new List<DictionaryItemInfoVerseReference>();
            Text = text;
            StrongCode = strongcode;
            GrammarCode = grammarCode;
        }
    }
    class DictionaryItemInfoVerseReference {
        public int Book { get; set; }
        public int Chapter { get; set; }
        public int Verse { get; set; }
    }
}
