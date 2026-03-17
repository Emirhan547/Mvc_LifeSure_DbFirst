using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Data.Entities
{
    public class InsurancePackage
    {
        [Key]
        public int Id { get; set; }
        public string PackageName { get; set; }

        public string Description { get; set; }
        public decimal BasePrice { get; set; }

        public int CoveragePeriodMonths { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<Policy> Policies { get; set; }
    }
}