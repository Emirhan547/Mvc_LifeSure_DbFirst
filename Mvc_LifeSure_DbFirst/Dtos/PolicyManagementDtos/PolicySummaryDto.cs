using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.PolicyManagementDtos
{
    public class PolicySummaryDto
    {
        public int TotalPolicyCount { get; set; }
        public int ActivePolicyCount { get; set; }
        public int ExpiredPolicyCount { get; set; }
        public int ExpiringSoonCount { get; set; }
        public decimal TotalPremium { get; set; }
    }
}