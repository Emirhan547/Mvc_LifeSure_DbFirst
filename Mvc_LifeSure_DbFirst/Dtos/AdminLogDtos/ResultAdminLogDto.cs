using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.AdminLogDtos
{
    public class ResultAdminLogDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public string ActionType { get; set; }
        public string TableName { get; set; }
        public int? RecordId { get; set; }
        public string IpAddress { get; set; }
        public DateTime Timestamp { get; set; }
    }
}