using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Data.Entities
{
    public class AdminLog
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }
        public string AdminUserName { get; set; }

        public string Action { get; set; } 

        public string ActionType { get; set; } 

        public string TableName { get; set; } 

        public int? RecordId { get; set; } 
        public string IpAddress { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}