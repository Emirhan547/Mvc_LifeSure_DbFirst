using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.MLDtos
{
    public class PolicySalesData
    {
        public string City { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public float SalesCount { get; set; }
        public float TotalPremium { get; set; }
    }
}