using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Repositories.PolicySaleDataRepositories
{
    public interface IPolicySaleDataRepository : IRepository<PolicySaleData>
    {
        List<PolicySaleData> GetSalesByCity(string city);
        List<PolicySaleData> GetSalesByDateRange(DateTime startDate, DateTime endDate);
        List<PolicySaleData> GetSalesByYear(int year);
        List<IGrouping<string, PolicySaleData>> GetSalesGroupedByCity();
        List<IGrouping<string, PolicySaleData>> GetSalesGroupedByYearMonth();
    }
}
