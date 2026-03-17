using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.PolicyDtos
{
    public class ResultPolicySimpleDto
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime StartDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public string UserFullName { get; set; }
        public string PackageName { get; set; }
    }
}