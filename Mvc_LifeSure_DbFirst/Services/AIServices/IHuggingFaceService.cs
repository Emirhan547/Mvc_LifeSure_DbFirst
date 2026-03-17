using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.AIServices
{
    public interface IHuggingFaceService
    {
        Task<string> ClassifyMessageAsync(string message);
    }
}
