using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Data.Entities
{
    public class ContactMessage
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public string Category { get; set; } 

        public string AutoReply { get; set; } 

        public bool IsReplied { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}