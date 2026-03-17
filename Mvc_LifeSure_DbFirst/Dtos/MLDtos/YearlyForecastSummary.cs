using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.MLDtos
{
    public class YearlyForecastSummary
    {
        public string City { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public float PredictedTotalSales { get; set; }
        public float PredictedTotalPremium { get; set; }
        public float MinSales { get; set; }
        public float MaxSales { get; set; }
    }
}