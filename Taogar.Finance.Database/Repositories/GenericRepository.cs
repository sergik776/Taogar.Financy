using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Taogar.Finance.Database.Interfaces;
using Taogar.Finance.Domain.Interfaces;
using Taogar.Finance.Infrastructure;

namespace Taogar.Finance.Database.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity, IUserEntity, new()
    {
        private readonly FinancyDBContext DBContext;

        public GenericRepository(FinancyDBContext dBContext)
        {
            this.DBContext = dBContext;
            DBContext.InitializeDatabase();
        }

        public async Task<TEntity> Create(TEntity entity)
        {
            var res = await DBContext.Set<TEntity>().AddAsync(entity);
            await DBContext.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            var entityToDelete = new TEntity { Id = id };
            var res = DBContext.Set<TEntity>().Remove(entityToDelete);
            await DBContext.SaveChangesAsync();
            if (res.State == EntityState.Detached)
                return true;
            else return false;
        }

        public async Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate == null)
                return DBContext.Set<TEntity>().AsNoTracking().AsQueryable();
            else
                return DBContext.Set<TEntity>().Where(predicate).AsNoTracking().AsQueryable();
        }

        public async Task<TEntity?> GetById(int id)
        {
            return await DBContext.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> IsEntityExist(int id, Expression<Func<TEntity, bool>>? predicate = null)
        {
            if(predicate == null)
                return await DBContext.Set<TEntity>().AsNoTracking().Where(x => x.Id == id).AnyAsync();
            else
                return await DBContext.Set<TEntity>().AsNoTracking().Where(predicate).Where(x => x.Id == id).AnyAsync();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            var res = DBContext.Set<TEntity>().Update(entity);
            await DBContext.SaveChangesAsync();
            return res.Entity ?? entity;
        }
    }
}
