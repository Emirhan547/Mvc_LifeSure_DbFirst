using Mvc_LifeSure_DbFirst.Dtos.InsurancePackageDtos;
using Mvc_LifeSure_DbFirst.Dtos.PolicyDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.PolicyManagementDtos
{
    public class PolicyManagementIndexViewModel
    {
        public List<ResultPolicyDto> Policies { get; set; }
        public List<string> Cities { get; set; }
        public List<ResultInsurancePackageDto> Packages { get; set; }
        public PolicyManagementFilterDto Filters { get; set; }
        public PolicySummaryDto Summary { get; set; }
    }
}