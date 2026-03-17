using Mvc_LifeSure_DbFirst.Dtos.TavilyDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.AIServices
{
    public interface ITavilyService
    {
        Task<TavilySearchResult> SearchAsync(string query, int maxResults = 5);
        Task<string> SearchAndGetAnswerAsync(string query);
    }
}
