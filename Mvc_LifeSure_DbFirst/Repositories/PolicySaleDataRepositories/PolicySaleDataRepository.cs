using Mvc_LifeSure_DbFirst.Data.Context;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.PolicySaleDataRepositories
{
    public class PolicySaleDataRepository : GenericRepository<PolicySaleData>, IPolicySaleDataRepository
    {
        private readonly AppDbContext _context;

        public PolicySaleDataRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public List<PolicySaleData> GetSalesByCity(string city)
        {
            return _context.PolicySaleDatas
                .Where(x => x.City == city)
                .OrderBy(x => x.SaleDate)
                .ToList();
        }

        public List<PolicySaleData> GetSalesByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.PolicySaleDatas
                .Where(x => x.SaleDate >= startDate && x.SaleDate <= endDate)
                .OrderBy(x => x.SaleDate)
                .ToList();
        }

        public List<PolicySaleData> GetSalesByYear(int year)
        {
            return _context.PolicySaleDatas
                .Where(x => x.SaleDate.Year == year)
                .OrderBy(x => x.SaleDate)
                .ToList();
        }

        public List<IGrouping<string, PolicySaleData>> GetSalesGroupedByCity()
        {
            return _context.PolicySaleDatas
                .GroupBy(x => x.City)
                .ToList();
        }

        public List<IGrouping<string, PolicySaleData>> GetSalesGroupedByYearMonth()
        {
            return _context.PolicySaleDatas
                .GroupBy(x => x.YearMonth)
                .ToList();
        }
    }
}