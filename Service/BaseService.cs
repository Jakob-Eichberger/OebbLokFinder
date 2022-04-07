using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BaseService
    {
        public Database Db { get; private set; }

        public BaseService(Database db)
        {
            Db = db;
        }

        public IQueryable<TEntity> GetTable<TEntity>() where TEntity : class => Db.Set<TEntity>();

        protected async Task AddAsync<TEntity>(TEntity obj) where TEntity : class
        {
            await Db.Set<TEntity>().AddAsync(obj);
            await Db.SaveChangesAsync();
        }

        protected void Add<TEntity>(TEntity obj) where TEntity : class
        {
            Db.Set<TEntity>().Add(obj);
            Db.SaveChanges();
        }

        protected async Task DeleteAsync<TEntity>(TEntity obj) where TEntity : class
        {
            Db.Set<TEntity>().Remove(obj);
            await Db.SaveChangesAsync();
        }

        protected void Update<TEntity>(TEntity obj) where TEntity : class
        {
            Db.Set<TEntity>().Update(obj);
            Db.SaveChanges();
        }

        protected async Task UpdateAsync<TEntity>(TEntity obj) where TEntity : class
        {
            Db.Set<TEntity>().Update(obj);
            await Db.SaveChangesAsync();
        }

    }
}
