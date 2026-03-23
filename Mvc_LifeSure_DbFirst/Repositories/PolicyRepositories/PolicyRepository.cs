using Mvc_LifeSure_DbFirst.Data.Context;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.PolicyRepositories
{
    public class PolicyRepository : GenericRepository<Policy>, IPolicyRepository
    {
        private readonly AppDbContext _context;

        public PolicyRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public List<Policy> GetAllWithDetails()
        {
            return _context.Policies
                .Include(x => x.User)
                .Include(x => x.InsurancePackage)
                .ToList();
        }
        public Policy GetPolicyWithDetails(int id)
        {
            return _context.Policies
                .Include(x => x.User)                    // Customer yerine User
                .Include(x => x.InsurancePackage)
                .FirstOrDefault(x => x.Id == id);
        }

        public List<Policy> GetPoliciesByUser(string userId)    // Parameter type string oldu
        {
            return _context.Policies
                .Include(x => x.InsurancePackage)
                .Include(x => x.User)
                .Where(x => x.UserId == userId)                 // CustomerId yerine UserId
                .ToList();
        }

        public List<Policy> GetPoliciesByPackage(int packageId)
        {
            return _context.Policies
                .Include(x => x.User)                          // Customer yerine User
                .Where(x => x.InsurancePackageId == packageId)
                .ToList();
        }

        public List<Policy> GetPoliciesByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.Policies
                .Include(x => x.User)
                .Include(x => x.InsurancePackage)
                .Where(x => x.StartDate >= startDate && x.StartDate <= endDate)
                .ToList();
        }

        public List<Policy> GetPoliciesByCity(string city)
        {
            return _context.Policies
                .Include(x => x.User)                          // Customer yerine User
                .Include(x => x.InsurancePackage)
                .Where(x => x.User.City == city)              // Customer.City yerine User.City
                .ToList();
        }

        public List<IGrouping<string, Policy>> GetPoliciesGroupedByCity()
        {
            return _context.Policies
                .Include(x => x.User)
                .ToList()                                      // Önce veriyi çek
                .GroupBy(x => x.User?.City ?? "Bilinmiyor")   // Sonra grupla
                .ToList();
        }

        public int GetPolicyCountByCity(string city)
        {
            return _context.Policies
                .Include(x => x.User)
                .Count(x => x.User.City == city);            // Customer.City yerine User.City
        }

        public decimal GetTotalPremiumByCity(string city)
        {
            return _context.Policies
                .Include(x => x.User)
                .Where(x => x.User.City == city)             // Customer.City yerine User.City
                .Sum(x => x.PremiumAmount);
        }
    }
}