using Mvc_LifeSure_DbFirst.Dtos.TavilyDtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
namespace Mvc_LifeSure_DbFirst.Services.AIServices
{
    public class TavilyService : ITavilyService
    {

        private readonly string _apiKey;
        private readonly string _apiUrl;

        public TavilyService()
        {
            _apiKey = ConfigurationManager.AppSettings["TavilyApiKey"];
            _apiUrl = ConfigurationManager.AppSettings["TavilyApiUrl"] ?? "https://api.tavily.com/search";

            if (string.IsNullOrEmpty(_apiKey))
                throw new Exception("Tavily API Key bulunamadı! Web.config kontrol et.");
        }

        public async Task<TavilySearchResult> SearchAsync(string query, int maxResults = 5)
        {
            using (var client = new HttpClient())
            {
                var requestBody = new
                {
                    api_key = _apiKey,
                    query = query,
                    search_depth = "advanced",
                    include_answer = true,
                    include_images = false,
                    include_raw_content = false,
                    max_results = maxResults
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_apiUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Tavily API HTTP Hata: {response.StatusCode} - {responseString}");
                }

                // JSON'ı doğrudan sınıfa deserialize et
                var searchResult = JsonConvert.DeserializeObject<TavilySearchResult>(responseString);

                if (searchResult == null)
                {
                    throw new Exception("API yanıtı deserialize edilemedi.");
                }

                // Results null ise boş liste ata
                searchResult.Results = searchResult.Results ?? new List<TavilySearchResultItem>();

                return searchResult;
            }
        }

        public async Task<string> SearchAndGetAnswerAsync(string query)
        {
            try
            {
                var result = await SearchAsync(query);
                return !string.IsNullOrEmpty(result?.Answer)
                    ? result.Answer
                    : "Üzgünüm, sorunuza yanıt bulamadım. Lütfen farklı bir şekilde tekrar deneyin.";
            }
            catch (Exception ex)
            {
                return $"Arama sırasında bir hata oluştu: {ex.Message}";
            }
        }
    }
}