using Mvc_LifeSure_DbFirst.Data.Context;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.AdminLogRepositories
{
    public class AdminLogRepository : GenericRepository<AdminLog>, IAdminLogRepository
    {
        private readonly AppDbContext _context;

        public AdminLogRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public List<AdminLog> GetLogsByAdmin(string adminUserName)
        {
            return _context.AdminLogs
                .Where(x => x.AdminUserName == adminUserName)
                .OrderByDescending(x => x.Timestamp)
                .ToList();
        }

        public List<AdminLog> GetLogsByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.AdminLogs
                .Where(x => x.Timestamp >= startDate && x.Timestamp <= endDate)
                .OrderByDescending(x => x.Timestamp)
                .ToList();
        }

        public List<AdminLog> GetLogsByActionType(string actionType)
        {
            return _context.AdminLogs
                .Where(x => x.ActionType == actionType)
                .OrderByDescending(x => x.Timestamp)
                .ToList();
        }

        public List<AdminLog> GetLogsByTable(string tableName)
        {
            return _context.AdminLogs
                .Where(x => x.TableName == tableName)
                .OrderByDescending(x => x.Timestamp)
                .ToList();
        }
    }
}