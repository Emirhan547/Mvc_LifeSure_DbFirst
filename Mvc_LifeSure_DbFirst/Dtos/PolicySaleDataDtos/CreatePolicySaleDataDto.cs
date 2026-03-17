using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.PolicySaleDataDtos
{
    public class CreatePolicySaleDataDto
    {
        public string City { get; set; }
        public DateTime SaleDate { get; set; }
        public int SaleCount { get; set; }
        public decimal TotalPremium { get; set; }
    }
}