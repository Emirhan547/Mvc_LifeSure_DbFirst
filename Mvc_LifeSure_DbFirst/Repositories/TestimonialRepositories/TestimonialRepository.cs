using Mvc_LifeSure_DbFirst.Data.Context;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.TestimonialRepositories
{
    public class TestimonialRepository : GenericRepository<Testimonial>, ITestimonialRepository
    {
        public TestimonialRepository(AppDbContext context) : base(context)
        {
        }
    }
}