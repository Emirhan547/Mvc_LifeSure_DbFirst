using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.AIServices
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly string _apiKey;
        private readonly string _apiUrl = "https://api.openai.com/v1/chat/completions";

        public ChatGPTService()
        {
            _apiKey = ConfigurationManager.AppSettings["OpenAIKey"];
            if (string.IsNullOrEmpty(_apiKey))
                throw new Exception("OpenAI API Key AppSettings içinde tanımlı değil!");
        }

        public async Task<string> GenerateReplyAsync(string originalMessage, string category, string userName)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                    var prompt = $@"
Bir sigorta şirketinin müşteri hizmetleri temsilcisisiniz. 
Mesaj kategorisi: {category}

Müşteri Adı: {userName}
Müşteri Mesajı: {originalMessage}

Bu müşteriye nazik, profesyonel ve yardımsever bir yanıt oluşturun.
Yanıt kısa, öz ve samimi olmalı.";

                    var payload = new
                    {
                        model = "gpt-3.5-turbo",
                        messages = new[]
                        {
                    new { role = "system", content = "Sen profesyonel bir sigorta şirketi müşteri hizmetleri temsilcisisin." },
                    new { role = "user", content = prompt }
                },
                        temperature = 0.7,
                        max_tokens = 300
                    };

                    var json = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(_apiUrl, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    // DEBUG: API cevabını loglamak için
                    System.Diagnostics.Debug.WriteLine("OpenAI Response: " + responseString);

                    var result = JObject.Parse(responseString);
                    var reply = result["choices"]?[0]?["message"]?["content"]?.ToString();

                    if (string.IsNullOrWhiteSpace(reply))
                        throw new Exception("API yanıtı boş döndü.");

                    return reply.Trim();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ChatGPTService Error: " + ex.Message);

                return $@"
Sayın {userName},

Mesajınız tarafımıza ulaşmıştır. En kısa sürede size dönüş yapacağız.

Teşekkür ederiz.
LifeSure Sigorta";
            }
        }
    }
}