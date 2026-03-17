using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.PolicySaleDataDtos
{
    public class ForecastResultDto
    {
        public string City { get; set; }
        public string Period { get; set; } // Örn: 2026-Q1
        public int PredictedSales { get; set; }
        public decimal PredictedPremium { get; set; }
        public float Confidence { get; set; }
    }
}