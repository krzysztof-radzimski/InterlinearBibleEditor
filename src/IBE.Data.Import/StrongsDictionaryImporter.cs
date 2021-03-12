/*=====================================================================================

	Interlinear Bible Editor
	.NET Windows Forms Interlinear Bible wysiwyg desktop editor project.
		
    MIT License
    https://github.com/krzysztof-radzimski/InterlinearBibleEditor/blob/main/LICENSE

	Autor: 2009-2021 ITORG Krzysztof Radzimski
	http://itorg.pl

  ===================================================================================*/

using DevExpress.Xpo;
using IBE.Data.Model;
using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace IBE.Data.Import {
    public class StrongsDictionaryImporter : BaseImporter {
        public override void Import(string zipFilePath, UnitOfWork uow) {
            if (File.Exists(zipFilePath)) {
                var fileName = ExtractAndGetFirstArchiveItemFilePath(zipFilePath);
                try {
                    using (var conn = new SqliteConnection($"DataSource=\"{fileName}\"")) {
                        SQLitePCL.Batteries.Init();
                        conn.Open();

                        var command = conn.CreateCommand();
                        command.CommandText = @"SELECT * FROM dictionary";

                        using (var reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                var topic = reader.GetString(0);
                                var lexeme = reader.GetString(1);
                                var transliteration = reader.GetString(2);
                                var pronunciacion = reader.GetString(3);
                                var shortDefinition = reader.GetString(4);
                                var definition = reader.GetString(5);

                                var lang = topic.StartsWith("H") ? Language.Hebrew : Language.Greek;
                                var code = Convert.ToInt32(topic.Substring(1));

                                var item = new StrongCode(uow) {
                                    Code = code,
                                    Definition = definition,
                                    Lang = lang,
                                    Pronunciation = pronunciacion,
                                    ShortDefinition = shortDefinition,
                                    SourceWord = lexeme,
                                    Transliteration = transliteration
                                };

                                item.Save();
                            }
                        }

                        conn.Close();
                    }
                }
                finally {
                    try { File.Delete(fileName); } catch { }
                }
            }
        }
    }
}