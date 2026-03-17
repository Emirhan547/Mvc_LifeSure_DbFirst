using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Repositories.AdminLogRepositories
{
    public interface IAdminLogRepository : IRepository<AdminLog>
    {
        List<AdminLog> GetLogsByAdmin(string adminUserName);
        List<AdminLog> GetLogsByDateRange(DateTime startDate, DateTime endDate);
        List<AdminLog> GetLogsByActionType(string actionType);
        List<AdminLog> GetLogsByTable(string tableName);
    }
}
