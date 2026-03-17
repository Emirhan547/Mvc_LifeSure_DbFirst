using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.MLDtos
{
    public class CityForecastResult
    {
        public string City { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string Period { get; set; }
        public float PredictedSales { get; set; }
        public float PredictedPremium { get; set; }
        public float ConfidenceLower { get; set; }
        public float ConfidenceUpper { get; set; }
    }
}