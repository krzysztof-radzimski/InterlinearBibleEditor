using IBE.Common.Extensions;
using IBE.Translator.Model;
using System.IO;
using System.Xml.Serialization;

namespace IBE.Translator.Controllers {
    public class TranslatorSettingsController {
        public TranslatorSettings Settings { get; private set; }
        public TranslatorSettings GetSettings(string path = null) {
            if (path.IsNullOrEmpty()) { path = "../../../../db/translator.xml"; }
            var serializer = new XmlSerializer(typeof(TranslatorSettings));
            var result = serializer.Deserialize(new MemoryStream(File.ReadAllBytes(path)));
            Settings = result as TranslatorSettings;
            return Settings;
        }
    }
}
