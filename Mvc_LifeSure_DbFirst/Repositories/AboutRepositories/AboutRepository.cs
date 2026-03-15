using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.AboutRepositories
{
    public class AboutRepository : GenericRepository<Abouts>, IAboutRepository
    {
        public AboutRepository(MvcLifeSureDbEntities context) : base(context)
        {
        }
    }
}