using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBE.Translator.Model {
    public class TranslationsResult {
        [JsonProperty("translations")]
        public TranslationResult[] Translations { get; set; }
    }
    public class TranslationResult {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("to")]
        public string Language { get; set; }
    }
}
