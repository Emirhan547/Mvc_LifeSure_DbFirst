using Mvc_LifeSure_DbFirst.Data.Context;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.ServiceRepositories
{
    public class ServicesRepository : GenericRepository<Data.Entities.Services>, IServicesRepository
    {
        public ServicesRepository(AppDbContext context) : base(context)
        {
        }
    }
}