using IBE.Translator.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IBE.Translator.Controllers {
    public class TranslatorController {
        public TranslatorSettingsController SettingsController { get; }
        public TranslatorController(string settingsFilePath = null) {
            this.SettingsController = new TranslatorSettingsController();
            this.SettingsController.GetSettings(settingsFilePath);
        }

        public async Task<TranslationsResult[]> Translate(string textToTranslate, TypeText type = TypeText.html, string langFrom = "en", string langTo = "pl") {
            string route = $"/translate?api-version=3.0&from={langFrom}&to={langTo}&textType={type}";
            object[] body = new object[] { new { Text = textToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient()) {
                using (var request = new HttpRequestMessage()) {
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(SettingsController.Settings.EndpointTextUrl + route);
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", SettingsController.Settings.SubscriptionKey1);
                    request.Headers.Add("Ocp-Apim-Subscription-Region", SettingsController.Settings.Region);

                    HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                    string result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TranslationsResult[]>(result);
                }
            }
        }
    }
}
