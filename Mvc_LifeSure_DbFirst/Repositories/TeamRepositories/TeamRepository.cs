using Mvc_LifeSure_DbFirst.Data.Context;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.TeamRepositories
{
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {
        public TeamRepository(AppDbContext context) : base(context)
        {
        }
    }
}