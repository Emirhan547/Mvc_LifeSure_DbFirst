using Mvc_LifeSure_DbFirst.Dtos.AdminLogDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Services.AdminLogServices
{
    public interface IAdminLogService
    {
        List<ResultAdminLogDto> GetAll();
        ResultAdminLogDto GetById(int id);
        void Create(CreateAdminLogDto createDto);
        List<ResultAdminLogDto> GetLogsByAdmin(string userId);
        List<ResultAdminLogDto> GetLogsByDateRange(DateTime startDate, DateTime endDate);
        List<ResultAdminLogDto> GetLogsByActionType(string actionType);
        List<ResultAdminLogDto> GetLogsByTable(string tableName);
        void LogAdminAction(string userId, string action, string actionType, string tableName, int? recordId = null);
    }
}
