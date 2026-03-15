using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc_LifeSure_DbFirst.Repositories.GenericRepositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> GetAll();
        TEntity GetById(int id);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Create (TEntity entity);
    }
}
