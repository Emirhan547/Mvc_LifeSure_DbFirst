using Mvc_LifeSure_DbFirst.Dtos.AIDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.AIServices
{
    public interface IGeminiService
    {
        Task<string> GenerateContentAsync(string prompt);
        Task<List<GeneratedPolicyDto>> GeneratePoliciesAsync(int count);
    }
}
