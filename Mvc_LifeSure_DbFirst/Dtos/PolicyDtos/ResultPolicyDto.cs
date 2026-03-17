using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.PolicyDtos
{
    public class ResultPolicyDto
    {
        public int Id { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PremiumAmount { get; set; }
        public DateTime CreatedAt { get; set; }

        // Müşteri bilgileri
        public string UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string UserCity { get; set; }

        // Paket bilgileri
        public int InsurancePackageId { get; set; }
        public string PackageName { get; set; }
        public decimal PackageBasePrice { get; set; }
    }
}