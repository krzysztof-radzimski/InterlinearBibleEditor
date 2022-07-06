using DevExpress.Xpo;
using ChurchServices.Extensions;
using ChurchServices.Data.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace ChurchServices.Data.Import.Greek {
    public class DictionaryBuilder {
        public void Build(UnitOfWork uow, string troPath = null) {
            BuildFromExisting(uow);
            if (troPath.IsNotNullOrEmpty()) {
                BuildFromTRO(troPath, uow);
            }
        }

        private void BuildFromExisting(UnitOfWork uow) {
            uow.BeginTransaction();
            AncientDictionary dic = null;
            if (!new XPQuery<AncientDictionary>(uow).Any()) {
                dic = new AncientDictionary(uow) {
                    Language = Language.Greek
                };
                dic.Save();
            }
            else {
                dic = new XPQuery<AncientDictionary>(uow).FirstOrDefault();
            }

            var strongs = new XPQuery<StrongCode>(uow).ToList();
            var grammarCodes = new XPQuery<GrammarCode>(uow).ToList();

            var items = new List<AncientDictionaryItem>();

            var verseView = new XPView(uow, typeof(VerseWord)) {
                CriteriaString = "Translation is not null AND Translation != ''"
            };
            verseView.Properties.Add(new ViewProperty("Oid", SortDirection.None, "[Oid]", false, true));
            verseView.Properties.Add(new ViewProperty("Translation", SortDirection.None, "[Translation]", false, true));
            verseView.Properties.Add(new ViewProperty("Transliteration", SortDirection.None, "[Transliteration]", false, true));
            verseView.Properties.Add(new ViewProperty("SourceWord", SortDirection.None, "[SourceWord]", false, true));
            verseView.Properties.Add(new ViewProperty("GrammarCodeId", SortDirection.None, "[GrammarCode.Oid]", false, true));
            verseView.Properties.Add(new ViewProperty("StrongCodeId", SortDirection.None, "[StrongCode.Oid]", false, true));

            var ancientDictionaryItemView = new XPView(uow, typeof(AncientDictionaryItem)) {
                CriteriaString = "Translation is not null AND Translation != ''"
            };
            ancientDictionaryItemView.Properties.Add(new ViewProperty("Word", SortDirection.None, "[Word]", false, true));
            foreach (ViewRecord item in ancientDictionaryItemView) {
                items.Add(new AncientDictionaryItem() { Word = item["Word"].ToString() });
            }

            foreach (ViewRecord item in verseView) {
                var sourceWord = item["SourceWord"].ToString().RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\"", "?").ToLower();
                if (!items.Where(x => x.Word.Equals(sourceWord)).Any()) {

                    var translationText = item["Translation"].ToString().ToLower();
                    translationText = Regex.Replace(translationText, @"(?<c>\<n\>(\s+)?\[.+\](\s+)?\<\/n\>)", String.Empty, RegexOptions.IgnoreCase);
                    translationText = translationText.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\"", "?").Trim();

                    if (translationText.IsNotNullOrEmpty()) {
                        var transliteration = item["Transliteration"].ToString().RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\"", "?").ToLower();

                        var dicItem = new AncientDictionaryItem(uow) {
                            Word = sourceWord,
                            Dictionary = dic,
                            Translation = translationText,
                            Transliteration = transliteration,
                            GrammarCode = grammarCodes.Where(x => x.Oid == item["GrammarCodeId"].ToInt()).FirstOrDefault(),
                            StrongCode = strongs.Where(x => x.Oid == item["StrongCodeId"].ToInt()).FirstOrDefault()
                        };

                        dicItem.Save();
                        items.Add(dicItem);
                    }
                }
            }

            uow.CommitChanges();
        }

        private void BuildFromTRO(string path, UnitOfWork uow) {
            var conn = GetConnection(path);
            var words = GetTroVerseInfos(conn);
            var c = new GreekTransliterationController();

            uow.BeginTransaction();
            var strongs = new XPQuery<StrongCode>(uow).ToList();
            var grammarCodes = new XPQuery<GrammarCode>(uow).ToList();
            AncientDictionary dic = null;
            if (!new XPQuery<AncientDictionary>(uow).Any()) {
                dic = new AncientDictionary(uow) {
                    Language = Language.Greek
                };
                dic.Save();
            }
            else {
                dic = new XPQuery<AncientDictionary>(uow).FirstOrDefault();
            }

            var ancientDictionaryItemView = new XPView(uow, typeof(AncientDictionaryItem)) {
                CriteriaString = "Translation is not null AND Translation != ''"
            };
            ancientDictionaryItemView.Properties.Add(new ViewProperty("Word", SortDirection.None, "[Word]", false, true));

            var exsistingWords = new List<TroVerseWord>();
            foreach (ViewRecord item in ancientDictionaryItemView) {
                exsistingWords.Add(new TroVerseWord() {
                    SourceWord = item["Word"].ToString()
                });
            }

            foreach (var word in words) {
                if (!exsistingWords.Where(x => x.SourceWord == word.SourceWord).Any()) {
                    var item = new AncientDictionaryItem(uow) {
                        Dictionary = dic,
                        Word = word.SourceWord,
                        Translation = word.Translation,
                        Transliteration = c.TransliterateWord(word.SourceWord),
                        StrongCode = strongs.Where(x => x.Lang == Language.Greek && x.Code == word.StrongCode).FirstOrDefault(),
                        GrammarCode = grammarCodes.Where(x => x.GrammarCodeVariant1 == word.GrammarCode || x.GrammarCodeVariant2 == word.GrammarCode || x.GrammarCodeVariant3 == word.GrammarCode).FirstOrDefault()
                    };
                    item.Save();
                    exsistingWords.Add(new TroVerseWord() { SourceWord = word.SourceWord });
                }
            }

            uow.CommitChanges();
        }

        private void _BuildFromExisting(UnitOfWork uow) {
            uow.BeginTransaction();
            AncientDictionary dic = null;
            if (!new XPQuery<AncientDictionary>(uow).Any()) {
                dic = new AncientDictionary(uow) {
                    Language = Language.Greek
                };
                dic.Save();
            }
            else {
                dic = new XPQuery<AncientDictionary>(uow).FirstOrDefault();
            }

            var c = new GreekTransliterationController();

            var exsistingWords = new List<TroVerseWord>();
            var exsistingItems = new XPQuery<AncientDictionaryItem>(uow);
            foreach (var item in exsistingItems) {
                exsistingWords.Add(new TroVerseWord() {
                    SourceWord = item.Word
                });
            }

            var words = new XPQuery<VerseWord>(uow).Where(x => x.Translation != null && x.Translation != "").ToList();
            foreach (var word in words) {
                var w = c.GetSourceWordWithoutBreathAndAccent(word.SourceWord.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\""), out var isUpper).ToLower();
                if (!exsistingWords.Where(x => x.SourceWord == w).Any()) {
                    var item = new AncientDictionaryItem(uow) {
                        Dictionary = dic,
                        Word = w,
                        Translation = word.Translation.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\""/*, "<n>", "</n>"*/),
                        Transliteration = word.Transliteration.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\""),
                        StrongCode = word.StrongCode,
                        GrammarCode = word.GrammarCode
                    };
                    item.Save();
                    exsistingWords.Add(new TroVerseWord() { SourceWord = w });
                }
            }
            uow.CommitChanges();
        }
        private void _BuildFromTRO(string path, UnitOfWork uow) {
            var conn = GetConnection(path);
            var words = GetTroVerseInfos(conn);
            var c = new GreekTransliterationController();

            uow.BeginTransaction();
            var strongs = new XPQuery<StrongCode>(uow).ToList();
            var grammarCodes = new XPQuery<GrammarCode>(uow).ToList();
            AncientDictionary dic = null;
            if (!new XPQuery<AncientDictionary>(uow).Any()) {
                dic = new AncientDictionary(uow) {
                    Language = Language.Greek
                };
                dic.Save();
            }
            else {
                dic = new XPQuery<AncientDictionary>(uow).FirstOrDefault();
            }

            var exsistingWords = new List<TroVerseWord>();
            var exsistingItems = new XPQuery<AncientDictionaryItem>(uow);
            foreach (var item in exsistingItems) {
                exsistingWords.Add(new TroVerseWord() {
                    SourceWord = item.Word
                });
            }

            foreach (var word in words) {
                if (!exsistingWords.Where(x => x.SourceWord == word.SourceWord).Any()) {
                    var item = new AncientDictionaryItem(uow) {
                        Dictionary = dic,
                        Word = word.SourceWord,
                        Translation = word.Translation,
                        Transliteration = c.TransliterateWord(word.SourceWord),
                        StrongCode = strongs.Where(x => x.Lang == Language.Greek && x.Code == word.StrongCode).FirstOrDefault(),
                        GrammarCode = grammarCodes.Where(x => x.GrammarCodeVariant1 == word.GrammarCode || x.GrammarCodeVariant2 == word.GrammarCode || x.GrammarCodeVariant3 == word.GrammarCode).FirstOrDefault()
                    };
                    item.Save();
                    exsistingWords.Add(new TroVerseWord() { SourceWord = word.SourceWord });
                }
            }

            uow.CommitChanges();
        }
        private SqliteConnection GetConnection(string filePath) {
            var conn = new SqliteConnection($"DataSource=\"{filePath}\"");
            SQLitePCL.Batteries.Init();
            conn.Open();
            return conn;
        }
        private List<TroVerseWord> GetTroVerseInfos(SqliteConnection troConnection) {
            var list = new List<TroVerseWord>();
            var command = troConnection.CreateCommand();
            command.CommandText = $"SELECT text FROM verses";
            using (var r = command.ExecuteReader()) {
                while (r.Read()) {
                    using (var parser = new TroVerseParses(r.GetString(0))) {
                        foreach (var item in parser.Words) {
                            if (!list.Where(x => x.SourceWord == item.SourceWord).Any()) {
                                list.Add(item);
                            }
                        }
                    }
                }
            }
            return list;
        }

        class TroVerseParses : IDisposable {
            public List<TroVerseWord> Words { get; }
            private TroVerseParses() { Words = new List<TroVerseWord>(); }
            /*
             <e>ζοροβαβελ</e> <n>zorobabel</n> Zorobabel – [zrodzony w Babilonie] <S>2216</S> <m>WTN-PRI</m> 
             <e>δε</e> <n>de</n> zaś <S>1161</S> <m>WTCONJ</m> 
             <e>εγεννησεν</e> <n>egennēsen</n> zrodził <S>1080</S> <m>WTV-AAI-3S</m> 
             <e>τον</e> <n>ton</n> <S>3588</S> <m>WTT-ASM</m> <e>αβιουδ</e> <n>abioud</n> Abiuda – [mój ojciec jest władzą] <S>10</S> <m>WTN-PRI</m> <e>αβιουδ</e> <n>abioud</n> Abiud – [mój ojciec jest władzą] <S>10</S> <m>WTN-PRI</m> <e>δε</e> <n>de</n> zaś <S>1161</S> <m>WTCONJ</m> <e>εγεννησεν</e> <n>egennēsen</n> zrodził <S>1080</S> <m>WTV-AAI-3S</m> <e>τον</e> <n>ton</n> <S>3588</S> <m>WTT-ASM</m> <e>ελιακειμ</e> <n>eliakeim</n> Eliakima – [Bóg powołuje] <S>1662</S> <m>WTN-PRI</m> <e>ελιακειμ</e> <n>eliakeim</n> Eliakim – [Bóg powołuje] <S>1662</S> <m>WTN-PRI</m> <e>δε</e> <n>de</n> zaś <S>1161</S> <m>WTCONJ</m> <e>εγεννησεν</e> <n>egennēsen</n> zrodził <S>1080</S> <m>WTV-AAI-3S</m> <e>τον</e> <n>ton</n> <S>3588</S> <m>WTT-ASM</m> <e>αζωρ</e> <n>azōr</n> Azora – [pomocny] <S>107</S> <m>WTN-PRI</m>
             */
            public TroVerseParses(string e) : this() {
                if (e.IsNotNullOrEmpty()) {
                    var xmlText = $"<verse>{e.Replace("<e>", "<word><e>").Replace("</m>", "</m></word>").Replace("</n>", "</n><text>").Replace("<S>", "</text><S>")}</verse>";
                    var xml = XElement.Parse(xmlText);
                    foreach (var item in xml.Elements()) {
                        var itemWord = item.Element("text").Value.Trim();
                        if (itemWord.Contains("–")) {
                            itemWord = itemWord.Substring(0, (itemWord.IndexOf("–") - 1)).Trim();
                        }
                        itemWord = itemWord.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\"");
                        var word = new TroVerseWord() {
                            SourceWord = item.Element("e").Value.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\"").ToLower().Trim(),
                            StrongCode = item.Element("S").Value.Trim().ToInt(),
                            Transliteration = item.Element("n").Value.RemoveAny(".", ":", ",", ";", "·", "—", "-", ")", "(", "]", "[", "’", ";", "\"").ToLower().Trim(),
                            Translation = itemWord.ToLower(),
                            GrammarCode = item.Element("m").Value.Trim()
                        };
                        if (word.Translation.IsNullOrEmpty()) { word.Translation = "―"; }
                        Words.Add(word);
                    }
                }
            }

            public void Dispose() {

            }
        }
        class TroVerseWord {
            public string SourceWord { get; set; }
            public string Transliteration { get; set; }
            public string Translation { get; set; }
            public string GrammarCode { get; set; }
            public int StrongCode { get; set; }
        }
    }
}
