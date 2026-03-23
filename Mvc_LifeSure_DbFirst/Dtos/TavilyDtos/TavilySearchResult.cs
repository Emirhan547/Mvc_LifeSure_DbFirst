using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.TavilyDtos
{
    public class TavilySearchResult
    {
        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("response_time")]
        public double ResponseTime { get; set; }

        [JsonProperty("results")]
        public List<TavilySearchResultItem> Results { get; set; }

        public TavilySearchResult()
        {
            Results = new List<TavilySearchResultItem>();
        }
    }
}