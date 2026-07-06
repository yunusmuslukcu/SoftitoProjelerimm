using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Data.Abstract
{
    public interface IRepository<T> where T: class
    {
         Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity); 
        void Delete(T entity);
    }
}
