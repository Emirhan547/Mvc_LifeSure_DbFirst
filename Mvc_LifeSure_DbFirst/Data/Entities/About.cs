using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Data.Entities
{
    public class About
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Description1 { get; set; }

        public string Description2 { get; set; }

        public string ImageUrl { get; set; }
    }
}