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

namespace Mvc_LifeSure_DbFirst.Services.AIServices
{
    public class TavilyService : ITavilyService
    {
        private readonly string _apiKey = "YOUR_TAVILY_API_KEY"; // appsettings'e taşı
        private readonly string _apiUrl = "https://api.tavily.com/search";

        public async Task<TavilySearchResult> SearchAsync(string query, int maxResults = 5)
        {
            try
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

                    var result = JObject.Parse(responseString);

                    var searchResult = new TavilySearchResult
                    {
                        Answer = result["answer"]?.ToString(),
                        ResponseTime = result["response_time"]?.Value<double>() ?? 0,
                        Results = new List<TavilySearchResultItem>()
                    };

                    if (result["results"] != null)
                    {
                        foreach (var item in result["results"])
                        {
                            searchResult.Results.Add(new TavilySearchResultItem
                            {
                                Title = item["title"]?.ToString(),
                                Url = item["url"]?.ToString(),
                                Content = item["content"]?.ToString(),
                                Score = item["score"]?.Value<double>() ?? 0
                            });
                        }
                    }

                    return searchResult;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Tavily API hatası: " + ex.Message);
            }
        }

        public async Task<string> SearchAndGetAnswerAsync(string query)
        {
            try
            {
                var result = await SearchAsync(query);
                return !string.IsNullOrEmpty(result.Answer)
                    ? result.Answer
                    : "Üzgünüm, sorunuza yanıt bulamadım. Lütfen farklı bir şekilde tekrar deneyin.";
            }
            catch (Exception ex)
            {
                return "Arama sırasında bir hata oluştu: " + ex.Message;
            }
        }
    }
}