using Mvc_LifeSure_DbFirst.Dtos.AIDtos;
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
    public class GeminiService : IGeminiService
    {
        private readonly string _apiKey = ConfigurationManager.AppSettings["GeminiApiKey"];
        private readonly string _apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";

        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(60)
        };

        public async Task<string> GenerateContentAsync(string prompt)
        {
            if (string.IsNullOrEmpty(_apiKey))
                throw new Exception("Gemini API KEY boş");

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.4,
                    maxOutputTokens = 4096
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_apiUrl}?key={_apiKey}", content);

            var responseString = await response.Content.ReadAsStringAsync();

            Log("REQUEST:\n" + prompt);
            Log("RESPONSE:\n" + responseString);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Gemini API HTTP Error: " + response.StatusCode);

            var jObject = JObject.Parse(responseString);

            var text = jObject["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

            if (string.IsNullOrEmpty(text))
                throw new Exception("Gemini boş response döndürdü");

            return text;
        }

        public async Task<List<GeneratedPolicyDto>> GeneratePoliciesAsync(int count)
        {
            var prompt = $@"
{count} adet sigorta poliçesi üret.

SADECE JSON ARRAY döndür.
Açıklama yazma.

Format:
[
  {{
    ""firstName"": ""Ahmet"",
    ""lastName"": ""Yılmaz"",
    ""city"": ""İstanbul"",
    ""packageName"": ""Kasko"",
    ""month"": ""Mayıs"",
    ""year"": 20266,
    ""premiumAmount"": 5000
  }}
]
";

            int retry = 3;

            for (int i = 0; i < retry; i++)
            {
                try
                {
                    var result = await GenerateContentAsync(prompt);

                    var parsed = SafeParsePolicies(result);

                    if (parsed != null && parsed.Any())
                        return parsed;
                }
                catch (Exception ex)
                {
                    Log($"RETRY {i + 1} ERROR: {ex.Message}");
                }

                await Task.Delay(1000);
            }

            return GenerateSamplePolicies(count);
        }

        private List<GeneratedPolicyDto> SafeParsePolicies(string result)
        {
            try
            {
                result = result
                    .Replace("```json", "")
                    .Replace("```", "")
                    .Trim();

                var start = result.IndexOf("[");
                var end = result.LastIndexOf("]");

                if (start < 0 || end < start)
                    throw new Exception("JSON array bulunamadı");

                var json = result.Substring(start, end - start + 1);

                return JsonConvert.DeserializeObject<List<GeneratedPolicyDto>>(json);
            }
            catch (Exception ex)
            {
                Log("PARSE ERROR: " + ex.Message + "\nRAW:\n" + result);
                return null;
            }
        }

        private void Log(string message)
        {
            try
            {
                var path = HttpContext.Current.Server.MapPath("~/logs/gemini.txt");
                System.IO.File.AppendAllText(path,
                    $"[{DateTime.Now}] {message}\n\n");
            }
            catch { }
        }

        private List<GeneratedPolicyDto> GenerateSamplePolicies(int count)
        {
            var random = new Random();
            var list = new List<GeneratedPolicyDto>();

            var cities = new[] { "İstanbul", "Ankara", "İzmir", "Bursa", "Antalya" };
            var months = new[] { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs" };

            for (int i = 0; i < count; i++)
            {
                list.Add(new GeneratedPolicyDto
                {
                    FirstName = "Test",
                    LastName = "User",
                    City = cities[random.Next(cities.Length)],
                    PackageName = "Kasko",
                    Month = months[random.Next(months.Length)],
                    Year = 2026,
                    PremiumAmount = random.Next(1000, 10000)
                });
            }

            return list;
        }
    }
}