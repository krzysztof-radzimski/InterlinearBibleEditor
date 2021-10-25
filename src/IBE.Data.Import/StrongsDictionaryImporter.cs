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
using IBE.Translator.Controllers;
using Microsoft.Data.Sqlite;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IBE.Data.Import {
    public class StrongsDictionaryImporter : BaseImporter<object> {
        private async Task<string> GetTranslation(TranslatorController controller, string text) {
            var _translation = await controller.Translate(text);
            if (_translation.IsNotNull()) {
                if (_translation.Length > 0) {
                    if (_translation.First().Translations.Length > 0) {
                        return _translation.First().Translations.First().Text;
                    }
                }
            }
            return String.Empty;
        }
        private async Task _Import(string zipFilePath, UnitOfWork uow) {
            if (File.Exists(zipFilePath)) {

                var controller = new TranslatorController();
                var fileName = ExtractAndGetFirstArchiveItemFilePath(zipFilePath);
                try {
                    using (var conn = new SqliteConnection($"DataSource=\"{fileName}\"")) {
                        SQLitePCL.Batteries.Init();
                        conn.Open();

                        var command = conn.CreateCommand();
                        var sql = @"SELECT topic, lexeme, transliteration, pronunciation, short_definition, definition FROM dictionary WHERE CAST(substr(topic,2) as INTEGER) > 4031 AND topic like ""G%""";
                        command.CommandText = sql; //@"SELECT topic, lexeme, transliteration, pronunciation, short_definition, definition FROM dictionary";

                        using (var reader = command.ExecuteReader()) {
                            while (reader.Read()) {
                                var topic = reader.GetString(0);
                                var lexeme = reader.GetString(1);
                                var transliteration = reader.GetString(2);
                                var pronunciacion = reader.GetString(3);
                                var shortDefinition = reader.GetString(4);
                                var definition = reader.GetString(5);

                                var lang = topic.StartsWith("H") ? Language.Hebrew : Language.Greek;
                                var _code = topic.Substring(1);
                                var code = Convert.ToInt32(_code);

                                var short_definition_translation = await GetTranslation(controller, shortDefinition);
                                var definition_translation = await GetTranslation(controller, definition);

                                var item = new XPQuery<StrongCode>(uow).Where(x => x.Lang == lang && x.Code == code).FirstOrDefault();
                                if (item.IsNull()) {
                                    item = new StrongCode(uow) {
                                        Code = code,
                                        Definition = definition_translation,
                                        Lang = lang,
                                        Pronunciation = pronunciacion,
                                        ShortDefinition = short_definition_translation,
                                        SourceWord = lexeme,
                                        Transliteration = transliteration
                                    };
                                }
                                else {
                                    item.Definition = definition_translation;
                                    item.Pronunciation = pronunciacion;
                                    item.ShortDefinition = short_definition_translation;
                                    item.SourceWord = lexeme;
                                    item.Transliteration = transliteration;
                                }

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

        public override object Import(string zipFilePath, UnitOfWork uow) {
            var task = _Import(zipFilePath, uow);
            var _wait = true;
            //task.Start();
            task.ContinueWith(x => {
                _wait = false;
            });

            while (_wait) {
                System.Threading.Thread.Sleep(50);
            }

            return default;
        }
    }
}