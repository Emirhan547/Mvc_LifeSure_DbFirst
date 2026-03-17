using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.MLDtos
{
    public class ModelTrainingResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public float RScore { get; set; }
        public float MeanAbsoluteError { get; set; }
        public float RootMeanSquaredError { get; set; }
        public int DataPointCount { get; set; }
    }
}