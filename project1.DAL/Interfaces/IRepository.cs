using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace project1.DAL.Interfaces
{
    public interface IRepository <TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> FindAllWhere(Expression <Func<TEntity, Boolean>> predicate);
        TEntity Get(int id);
        TEntity GetFirstOrDefaultWhere(Expression <Func<TEntity, Boolean>> predicate);

        void Add(TEntity item);
        void Update(TEntity item);
        void Delete(int id);



        //<-------------------------- Задел на будущее! -------------------------->
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAllWhereAsync(Expression<Func<TEntity, Boolean>> predicate);
        Task<TEntity> GetAsync(int id);
        Task<TEntity> GetFirstOrDefaultWhereAsync(Expression<Func<TEntity, Boolean>> predicate);



    }
}
