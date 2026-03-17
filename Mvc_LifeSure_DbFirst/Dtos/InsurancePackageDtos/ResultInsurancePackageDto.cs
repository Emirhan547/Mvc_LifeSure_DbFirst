using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.InsurancePackageDtos
{
    public class ResultInsurancePackageDto
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public int CoveragePeriodMonths { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PolicyCount { get; set; }
    }
}