using Mvc_LifeSure_DbFirst.Data.Context;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.InsurancePackageRepositories
{
    public class InsurancePackageRepository : GenericRepository<InsurancePackage>, IInsurancePackageRepository
    {
        private readonly AppDbContext _context;

        public InsurancePackageRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public InsurancePackage GetPackageWithPolicies(int id)
        {
            return _context.InsurancePackages
                .Include(x => x.Policies)  // Burada Policies navigation property'si olmalı
                .Include(x => x.Policies.Select(p => p.User))  // Poliçelerin kullanıcılarını da getir
                .FirstOrDefault(x => x.Id == id);
        }

        public List<InsurancePackage> GetActivePackages()
        {
            return _context.InsurancePackages
                .Where(x => x.IsActive)
                .ToList();
        }

        public InsurancePackage GetPackageByName(string packageName)
        {
            return _context.InsurancePackages
                .FirstOrDefault(x => x.PackageName == packageName);
        }
    }
}