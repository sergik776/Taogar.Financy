using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Taogar.Finance.Domain.Interfaces;

namespace Taogar.Finance.Database.Interfaces
{
    public interface IGenericRepository<T> where T : IEntity
    {
        Task<IQueryable<T>> GetAll(Expression<Func<T, bool>>? predicate = null);
        Task<T?> GetById(int id);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(int id);
        Task<bool> IsEntityExist(int id, Expression<Func<T, bool>>? predicate = null);
    }
}
