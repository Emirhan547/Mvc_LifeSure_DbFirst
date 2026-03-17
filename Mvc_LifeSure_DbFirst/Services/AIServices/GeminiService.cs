using Mvc_LifeSure_DbFirst.Dtos.AIDtos;
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
    public class GeminiService:IGeminiService
    {
        private readonly string _apiKey = "YOUR_GEMINI_API_KEY"; // appsettings'e taşı
        private readonly string _apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent";

        public async Task<string> GenerateContentAsync(string prompt)
        {
            try
            {
                using (var client = new HttpClient())
                {
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
                            temperature = 0.7,
                            maxOutputTokens = 2048
                        }
                    };

                    var json = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync($"{_apiUrl}?key={_apiKey}", content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    var jObject = JObject.Parse(responseString);
                    return jObject["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString() ?? "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Gemini API hatası: " + ex.Message);
            }
        }

        public async Task<List<GeneratedPolicyDto>> GeneratePoliciesAsync(int count)
        {
            var prompt = $@"
Bir sigorta şirketi için {count} adet örnek poliçe verisi oluştur. 
Her poliçe için aşağıdaki formatta JSON verisi üret:

[
  {{
    ""firstName"": ""Ahmet"",
    ""lastName"": ""Yılmaz"",
    ""city"": ""İstanbul"",
    ""packageName"": ""Trafik Sigortası"",
    ""month"": ""Mayıs"",
    ""year"": 2025,
    ""premiumAmount"": 5000
  }},
  ...
]

Kurallar:
- firstName: Türkçe erkek/kadın isimleri (Ahmet, Mehmet, Ayşe, Fatma vb.)
- lastName: Türkçe soyadları (Yılmaz, Kaya, Demir, Çelik vb.)
- city: Türkiye'nin büyük şehirleri (İstanbul, Ankara, İzmir, Bursa, Antalya, Adana, Konya, Gaziantep, Mersin, Kayseri)
- packageName: Sigorta paketleri (Trafik Sigortası, Kasko, Sağlık Sigortası, Konut Sigortası, Seyahat Sigortası, DASK, Özel Sağlık)
- month: Ocak, Şubat, Mart, Nisan, Mayıs, Haziran, Temmuz, Ağustos, Eylül, Ekim, Kasım, Aralık
- year: 2023, 2024, 2025 (dengeli dağılsın)
- premiumAmount: 1000 TL ile 15000 TL arasında rastgele tutarlar

Sadece JSON formatında yanıt ver, açıklama ekleme.
";

            try
            {
                var result = await GenerateContentAsync(prompt);

                // JSON'ı temizle (markdown formatını kaldır)
                result = result.Replace("```json", "").Replace("```", "").Trim();

                var policies = JsonConvert.DeserializeObject<List<GeneratedPolicyDto>>(result);
                return policies ?? new List<GeneratedPolicyDto>();
            }
            catch (Exception ex)
            {
                // Hata durumunda örnek veri üret
                return GenerateSamplePolicies(count);
            }
        }

        private List<GeneratedPolicyDto> GenerateSamplePolicies(int count)
        {
            var random = new Random();
            var policies = new List<GeneratedPolicyDto>();

            var firstNames = new[] { "Ahmet", "Mehmet", "Ayşe", "Fatma", "Ali", "Veli", "Zeynep", "Mustafa", "Hüseyin", "Elif" };
            var lastNames = new[] { "Yılmaz", "Kaya", "Demir", "Çelik", "Şahin", "Yıldız", "Öztürk", "Aydın", "Özdemir", "Arslan" };
            var cities = new[] { "İstanbul", "Ankara", "İzmir", "Bursa", "Antalya", "Adana", "Konya", "Gaziantep", "Mersin", "Kayseri" };
            var packages = new[] { "Trafik Sigortası", "Kasko", "Sağlık Sigortası", "Konut Sigortası", "Seyahat Sigortası", "DASK", "Özel Sağlık" };
            var months = new[] { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };
            var years = new[] { 2023, 2024, 2025 };

            for (int i = 0; i < count; i++)
            {
                policies.Add(new GeneratedPolicyDto
                {
                    FirstName = firstNames[random.Next(firstNames.Length)],
                    LastName = lastNames[random.Next(lastNames.Length)],
                    City = cities[random.Next(cities.Length)],
                    PackageName = packages[random.Next(packages.Length)],
                    Month = months[random.Next(months.Length)],
                    Year = years[random.Next(years.Length)],
                    PremiumAmount = random.Next(1000, 15001)
                });
            }

            return policies;
        }
    }
}