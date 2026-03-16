using Mvc_LifeSure_DbFirst.Data.Context;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;


namespace Mvc_LifeSure_DbFirst.Repositories.AboutRepositories
{
    public class AboutRepository : GenericRepository<About>, IAboutRepository
    {
        public AboutRepository(AppDbContext context) : base(context)
        {
        }
    }
}