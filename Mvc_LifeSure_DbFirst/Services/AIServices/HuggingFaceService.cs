using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Services.AIServices
{
    public class HuggingFaceService : IHuggingFaceService
    {
        private readonly string _apiKey;
        private readonly string _apiUrl = "https://api-inference.huggingface.co/models/facebook/bart-large-mnli";
        public HuggingFaceService()
        {
            _apiKey = ConfigurationManager.AppSettings["HuggingFaceApiKey"];
        }
        public async Task<string> ClassifyMessageAsync(string message)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                    var payload = new
                    {
                        inputs = message,
                        parameters = new
                        {
                            candidate_labels = new[] { "teşekkür", "şikayet", "rica", "bilgi", "destek", "geri bildirim" }
                        }
                    };

                    var json = JsonConvert.SerializeObject(payload);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(_apiUrl, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    var result = JObject.Parse(responseString);
                    var labels = result["labels"].ToObject<string[]>();
                    var scores = result["scores"].ToObject<float[]>();

                    // En yüksek skorlu kategoriyi bul
                    float maxScore = -1;
                    int maxIndex = 0;
                    for (int i = 0; i < scores.Length; i++)
                    {
                        if (scores[i] > maxScore)
                        {
                            maxScore = scores[i];
                            maxIndex = i;
                        }
                    }

                    string category = labels[maxIndex];

                    // If-else ile kategori dönüşümü
                    if (category == "teşekkür") return "Teşekkür";
                    else if (category == "şikayet") return "Şikayet";
                    else if (category == "rica") return "Rica";
                    else if (category == "bilgi") return "Bilgi Talebi";
                    else if (category == "destek") return "Destek";
                    else if (category == "geri bildirim") return "Geri Bildirim";
                    else return "Diğer";
                }
            }
            catch (Exception ex)
            {
                return "Diğer";
            }
        }
    }
}