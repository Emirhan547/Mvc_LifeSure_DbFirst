using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Data.Entities
{
    public class Faq
    {
        [Key]
        public int Id { get; set; }

       
        public string Question { get; set; }

        
        public string Answer { get; set; }

        public string ImageUrl { get; set; }
    }
}