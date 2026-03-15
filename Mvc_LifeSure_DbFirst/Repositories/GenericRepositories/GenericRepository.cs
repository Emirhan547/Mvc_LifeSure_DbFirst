using Mvc_LifeSure_DbFirst.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Repositories.GenericRepositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly MvcLifeSureDbEntities _context;
        protected readonly DbSet<TEntity> _table;

        public GenericRepository(MvcLifeSureDbEntities context)
        {
            _context = context;
            _table = _context.Set<TEntity>();
        }
        public List<TEntity> GetAll()
        {
            return _table.ToList();
        }

        public TEntity GetById(int id)
        {
            return _table.Find(id);
        }

        public void Create(TEntity entity)
        {
            _table.Add(entity);
            _context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _table.Remove(entity);
            _context.SaveChanges();
        }
    }
}