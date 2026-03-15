using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.ServiceRepositories
{
    public class ServicesRepository : GenericRepository<Models.Services>, IServicesRepository
    {
        public ServicesRepository(MvcLifeSureDbEntities context) : base(context)
        {
        }
    }
}