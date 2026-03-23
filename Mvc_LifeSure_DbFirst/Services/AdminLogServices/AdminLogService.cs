using Mapster;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Dtos.AdminLogDtos;
using Mvc_LifeSure_DbFirst.Repositories.AdminLogRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Services.AdminLogServices
{
    public class AdminLogService : IAdminLogService
    {
        private readonly IAdminLogRepository _logRepository;

        public AdminLogService(IAdminLogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public List<ResultAdminLogDto> GetAll()
        {
            var logs = _logRepository.GetAll();
            return logs.Adapt<List<ResultAdminLogDto>>();
        }

        public ResultAdminLogDto GetById(int id)
        {
            var log = _logRepository.GetById(id);
            if (log == null)
                throw new KeyNotFoundException("Log kaydı bulunamadı");

            return log.Adapt<ResultAdminLogDto>();
        }

        public void Create(CreateAdminLogDto createDto)
        {
            var log = createDto.Adapt<AdminLog>();
            log.Timestamp = DateTime.Now;

            _logRepository.Create(log);
        }

        public List<ResultAdminLogDto> GetLogsByAdmin(string userId)
        {
            var logs = _logRepository.GetLogsByAdmin(userId);
            return logs.Adapt<List<ResultAdminLogDto>>();
        }

        public List<ResultAdminLogDto> GetLogsByDateRange(DateTime startDate, DateTime endDate)
        {
            var logs = _logRepository.GetLogsByDateRange(startDate, endDate);
            return logs.Adapt<List<ResultAdminLogDto>>();
        }

        public List<ResultAdminLogDto> GetLogsByActionType(string actionType)
        {
            var logs = _logRepository.GetLogsByActionType(actionType);
            return logs.Adapt<List<ResultAdminLogDto>>();
        }

        public List<ResultAdminLogDto> GetLogsByTable(string tableName)
        {
            var logs = _logRepository.GetLogsByTable(tableName);
            return logs.Adapt<List<ResultAdminLogDto>>();
        }

        public void LogAdminAction(string userId, string action, string actionType, string tableName, int? recordId = null)
        {
            if (string.IsNullOrEmpty(userId))
                return;
            var log = new CreateAdminLogDto
            {
                UserId = userId,
                AdminUserName = HttpContext.Current?.User?.Identity?.Name,
                Action = action,
                ActionType = actionType,
                TableName = tableName,
                RecordId = recordId,
                IpAddress = HttpContext.Current?.Request?.UserHostAddress
            };

            Create(log);
        }
    }
}