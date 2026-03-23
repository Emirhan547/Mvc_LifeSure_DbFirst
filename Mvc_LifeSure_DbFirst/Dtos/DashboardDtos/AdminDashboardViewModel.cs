using Mvc_LifeSure_DbFirst.Dtos.AdminLogDtos;
using Mvc_LifeSure_DbFirst.Dtos.PolicyDtos;
using System.Collections.Generic;

namespace Mvc_LifeSure_DbFirst.Dtos.DashboardDtos
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalPackages { get; set; }
        public int ActivePackages { get; set; }
        public int TotalPolicies { get; set; }
        public int ActivePolicies { get; set; }
        public int ExpiringPolicies { get; set; }
        public decimal TotalPremium { get; set; }
        public decimal AveragePremium { get; set; }
        public Dictionary<string, int> CityDistribution { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, decimal> PremiumDistribution { get; set; } = new Dictionary<string, decimal>();
        public List<ResultPolicySimpleDto> RecentPolicies { get; set; } = new List<ResultPolicySimpleDto>();
        public List<ResultAdminLogDto> RecentLogs { get; set; } = new List<ResultAdminLogDto>();
        public List<ResultPolicyDto> ExpiringPolicyList { get; set; } = new List<ResultPolicyDto>();
    }
}