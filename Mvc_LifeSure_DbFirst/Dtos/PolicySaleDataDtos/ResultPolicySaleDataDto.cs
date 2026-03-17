using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.PolicySaleDataDtos
{
    public class ResultPolicySaleDataDto
    {
        public int Id { get; set; }
        public string City { get; set; }
        public DateTime SaleDate { get; set; }
        public int SaleCount { get; set; }
        public decimal TotalPremium { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string YearMonth { get; set; }
    }
}