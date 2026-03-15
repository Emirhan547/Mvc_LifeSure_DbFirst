using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.TestimonialRepositories
{
    public class TestimonialRepository : GenericRepository<Testimonials>, ITestimonialRepository
    {
        public TestimonialRepository(MvcLifeSureDbEntities context) : base(context)
        {
        }
    }
}