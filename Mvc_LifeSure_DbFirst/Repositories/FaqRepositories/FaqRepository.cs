using Mvc_LifeSure_DbFirst.Data.Context;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.FaqRepositories
{
    public class FaqRepository : GenericRepository<Faq>, IFaqRepository
    {
        public FaqRepository(AppDbContext context) : base(context)
        {
        }
    }
}