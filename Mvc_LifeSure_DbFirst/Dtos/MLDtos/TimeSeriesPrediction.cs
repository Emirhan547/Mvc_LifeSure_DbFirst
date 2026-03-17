using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.MLDtos
{
    public class TimeSeriesPrediction
    {
        public float[] ForecastedSales { get; set; }
        public float[] ConfidenceLowerBound { get; set; }
        public float[] ConfidenceUpperBound { get; set; }
    }
}