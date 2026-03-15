using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.ServiceRepositories
{
    public class ServiceRepository : GenericRepository<Services>, IServiceRepository
    {
        public ServiceRepository(MvcLifeSureDbEntities context) : base(context)
        {
        }
    }
}