using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.TeamRepositories
{
    public class TeamRepository : GenericRepository<Teams>, ITeamRepository
    {
        public TeamRepository(MvcLifeSureDbEntities context) : base(context)
        {
        }
    }
}