using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Dtos.AboutDtos
{
    public class CreateAboutDto
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string ImageUrl { get; set; }
    }
}