using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.SliderRepositories
{
    public class SliderRepository : GenericRepository<Sliders>, ISliderRepository
    {
        public SliderRepository(MvcLifeSureDbEntities context) : base(context)
        {
        }
    }
}