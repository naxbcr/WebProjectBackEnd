using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using project1.DAL.Interfaces;
using System.Data.Entity;
using System.Threading.Tasks;

namespace project1.DAL.Repository
{
    public class EFGenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal db_KulakovEntities Context;
        internal DbSet<TEntity> DbSet;

        public EFGenericRepository(db_KulakovEntities context)
        {
            this.Context = context;
            this.DbSet = context.Set<TEntity>();
        }

        public virtual void Add(TEntity item)
        {
            DbSet.Add(item);
        }

        public virtual void Delete(int id)
        {
            TEntity entityToDelete = DbSet.Find(id);    
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual IEnumerable<TEntity> FindAllWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).ToList();
        }

        public virtual TEntity GetFirstOrDefaultWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).FirstOrDefault();
        }

        public virtual TEntity Get(int id)
        {
            return DbSet.Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        public virtual void Update(TEntity item)
        {
            DbSet.Attach(item);
            Context.Entry(item).State = EntityState.Modified;
        }


  
        /////////////////////////////// Async
    

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<TEntity> GetAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<TEntity> GetFirstOrDefaultWhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).FirstOrDefaultAsync();
        }


    }
}