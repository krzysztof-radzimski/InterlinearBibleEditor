using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace IBE.Data.Import.Greek {
    public class DictionaryBuilder {
        public void BuildFromTRO(string path, UnitOfWork uow) {
            var conn = GetConnection(path);
            var words = GetTroVerseInfos(conn);
            var c = new GreekTransliterationController();

            uow.BeginTransaction();
            var strongs = new XPQuery<StrongCode>(uow).ToList();
            var grammarCodes = new XPQuery<GrammarCode>(uow).ToList();
            var dic = new AncientDictionary(uow) {
                Language = Language.Greek
            };
            dic.Save();

            foreach (var word in words) {
                var item = new AncientDictionaryItem(uow) {
                    Dictionary = dic,
                    Word = word.SourceWord,
                    Translation = word.Translation,
                    Transliteration = c.TransliterateWord(word.SourceWord),
                    StrongCode = strongs.Where(x => x.Lang == Language.Greek && x.Code == word.StrongCode).FirstOrDefault(),
                    GrammarCode = grammarCodes.Where(x => x.GrammarCodeVariant1 == word.GrammarCode || x.GrammarCodeVariant2 == word.GrammarCode || x.GrammarCodeVariant3 == word.GrammarCode).FirstOrDefault()
                };
                item.Save();
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
