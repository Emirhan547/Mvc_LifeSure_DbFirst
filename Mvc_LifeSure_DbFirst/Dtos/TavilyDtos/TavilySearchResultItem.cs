using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.TavilyDtos
{
    public class TavilySearchResultItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public double Score { get; set; }
    }
}