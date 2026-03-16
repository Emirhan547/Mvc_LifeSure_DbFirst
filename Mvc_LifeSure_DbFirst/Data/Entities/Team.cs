using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Data.Entities
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Profession { get; set; }

        public string ImageUrl { get; set; }

        public string FacebookUrl { get; set; }

        public string TwitterUrl { get; set; }

        public string LinkedinUrl { get; set; }

        public string InstagramUrl { get; set; }
    }
}