using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Data.Entities
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string IdentityNumber { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }

        public string City { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Property
        public virtual ICollection<Policy> Policies { get; set; }
    }
}