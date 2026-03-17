using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Data.Entities
{
    public class PolicySaleData
    {
        [Key]
        public int Id { get; set; }
        public string City { get; set; }

        public DateTime SaleDate { get; set; }

        public int SaleCount { get; set; } 

        public decimal TotalPremium { get; set; }

        public int Month => SaleDate.Month;
        public int Year => SaleDate.Year;
        public string YearMonth => SaleDate.ToString("yyyy-MM");
    }
}