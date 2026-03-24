using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.PolicyManagementDtos
{
    public class PolicyManagementFilterDto
    {
        public string SearchTerm { get; set; }
        public string City { get; set; }
        public string Status { get; set; }
        public int? PackageId { get; set; }
    }
}