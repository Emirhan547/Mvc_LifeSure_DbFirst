using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Services.AIServices
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly string _apiKey = "YOUR_OPENAI_API_KEY"; // appsettings'e taşı
        private readonly string _apiUrl = "https://api.openai.com/v1/chat/completions";

        public async Task<string> GenerateReplyAsync(string originalMessage, string category, string userName)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                    var prompt = $@"
Bir sigorta şirketinin müşteri hizmetleri temsilcisisiniz. 
Size gelen mesajı kategorize ettik: {category}

Müşteri Adı: {userName}
Müşteri Mesajı: {originalMessage}

Bu müşteriye nazik, profesyonel ve yardımsever bir şekilde otomatik yanıt oluşturun.
Yanıt kısa, öz ve samimi olmalı. Müşterinin sorununa veya talebine uygun bir yanıt verin.
Eğer şikayet ise özür dileyin ve çözeceğinizi belirtin.
Eğer teşekkür ise rica ederim deyin.
Eğer bilgi talebi ise yardımcı olacağınızı belirtin.

Yanıt:";

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

                    var result = JObject.Parse(responseString);
                    var reply = result["choices"][0]["message"]["content"].ToString();

                    return reply.Trim();
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda varsayılan yanıt
                return $@"
Sayın {userName},

Mesajınız tarafımıza ulaşmıştır. En kısa sürede size dönüş yapacağız.

Teşekkür ederiz.
LifeSure Sigorta";
            }
        }
    }
}