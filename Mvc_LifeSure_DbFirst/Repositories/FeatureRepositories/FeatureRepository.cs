using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.FeatureRepositories
{
    public class FeatureRepository : GenericRepository<Features>, IFeatureRepository
    {
        public FeatureRepository(MvcLifeSureDbEntities context) : base(context)
        {
        }
    }
}