using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.TavilyDtos
{
    public class TavilySearchResult
    {
        public string Answer { get; set; }
        public List<TavilySearchResultItem> Results { get; set; }
        public double ResponseTime { get; set; }
    }
}