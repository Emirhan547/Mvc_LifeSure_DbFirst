using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.FaqRepositories
{
    public class FaqRepository : GenericRepository<Faqs>, IFaqRepository
    {
        public FaqRepository(MvcLifeSureDbEntities context) : base(context)
        {
        }
    }
}