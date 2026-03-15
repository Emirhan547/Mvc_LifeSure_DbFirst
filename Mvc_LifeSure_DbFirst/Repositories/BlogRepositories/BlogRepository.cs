using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.BlogRepositories
{
    public class BlogRepository : GenericRepository<Blogs>, IBlogRepository
    {
        public BlogRepository(MvcLifeSureDbEntities context) : base(context)
        {
        }
    }
}