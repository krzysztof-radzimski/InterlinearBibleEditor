using DevExpress.Xpo;
using IBE.Common.Extensions;
using IBE.Data.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IBE.Data.Import.Greek {
    public class GrammarCodesImporter : BaseImporter<object> {
        public override object Import(string zipFilePath, UnitOfWork uow) {
            var list = new List<GrammarCodesDictionaryItems>();
            if (File.Exists(zipFilePath)) {
                var fileName = ExtractAndGetFirstArchiveItemFilePath(zipFilePath);
                try {
                    var conn = new SqliteConnection($"DataSource=\"{fileName}\"");
                    SQLitePCL.Batteries.Init();
                    conn.Open();

                    using (var command = conn.CreateCommand()) {
                        command.CommandText = "select topic, definition, short_definition from dictionary";

                        using (var reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                var topic = reader.GetString(0);
                                var definition = reader.GetString(1);
                                var short_definition = reader.GetString(2);

                                list.Add(new GrammarCodesDictionaryItems() {
                                    Definition = definition,
                                    ShortDefinition = short_definition,
                                    Topic = topic
                                });
                            }
                        }
                    }

                    conn.Close();
                }
                finally {
                    try { File.Delete(fileName); } catch { }
                }
            }

            if (list.Count > 0) {
                foreach (var item in list) {
                    var grammarCode = GetGrammarCode(uow, item.Topic);
                    if (grammarCode.IsNotNull()) {
                        grammarCode.GrammarCodeVariant2 = item.Topic;
                        grammarCode.GrammarCodeDescription = item.Definition;
                        grammarCode.ShortDefinition = item.ShortDefinition;
                        grammarCode.Save();
                    }
                }
                
            }

            return default;
        }

        public GrammarCode GetGrammarCode(UnitOfWork uow, string topic) {
            var _topic = topic.Replace(" ", "").Replace("+", "").Replace("-", "").Replace(")", "").Replace("(", "");
            var view = new XPView(uow, typeof(GrammarCode));
            view.Properties.Add(new ViewProperty("Id", SortDirection.None, "[Oid]", false, true));
            view.CriteriaString = $"Replace([GrammarCodeVariant1],'-','') = '{_topic}'";

            foreach (ViewRecord item in view) {
                var id = item["Id"].ToInt();
                var result = new XPQuery<GrammarCode>(uow).Where(x => x.Oid == id).FirstOrDefault();
                if (result.IsNotNull()) {
                    return result;
                }                
            }

            return default;
        }

        class GrammarCodesDictionaryItems {
            public string Topic { get; set; }
            public string Definition { get; set; }
            public string ShortDefinition { get; set; }
        }
    }
}
