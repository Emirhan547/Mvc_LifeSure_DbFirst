using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.FaqDtos
{
    public class UpdateFaqDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string ImageUrl { get; set; }
    }
}