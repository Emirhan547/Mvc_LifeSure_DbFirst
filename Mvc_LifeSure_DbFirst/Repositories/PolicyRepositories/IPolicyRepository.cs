using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.PolicyRepositories
{
    public interface IPolicyRepository : IRepository<Policy>
    {
        List<Policy> GetAllWithDetails();
        Policy GetPolicyWithDetails(int id);
        List<Policy> GetPoliciesByUser(string userId);                    // string oldu
        List<Policy> GetPoliciesByPackage(int packageId);
        List<Policy> GetPoliciesByDateRange(DateTime startDate, DateTime endDate);
        List<Policy> GetPoliciesByCity(string city);
        List<IGrouping<string, Policy>> GetPoliciesGroupedByCity();
        int GetPolicyCountByCity(string city);
        decimal GetTotalPremiumByCity(string city);
    }
}