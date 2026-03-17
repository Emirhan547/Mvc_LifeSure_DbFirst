using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.AIDtos
{
    public class GeneratedPolicyDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string PackageName { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public decimal PremiumAmount { get; set; }
    }
}