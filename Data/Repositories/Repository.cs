using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Data.Utilities;

namespace Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<TEntity>> GetByIdAsync(object id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            return entity != null ? new ApiResult<TEntity>(AppMessages.GetByIdSuccess, entity) : new ApiResult<TEntity>(AppMessages.GetByIdError, null);
        }

        public async Task<ApiResult<TEntity>> GetByIdAsync(object id, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            var entity = await query.SingleOrDefaultAsync(e => e == id);
            return entity != null ? new ApiResult<TEntity>(AppMessages.GetByIdSuccess, entity) : new ApiResult<TEntity>(AppMessages.GetByIdError, null);
        }

        public async Task<ApiResult<IList<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, bool? asNoTracking = true, bool? deletedData = false, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (asNoTracking == true)
            {
                query = query.AsNoTracking();
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var entities = await query.ToListAsync();
            return new ApiResult<IList<TEntity>>(AppMessages.GetAllSuccess, entities);
        }

        public async Task<ApiResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, bool? asNoTracking = true, bool? deletedData = false, params Expression<Func<TEntity, object>>[] includes)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (asNoTracking == true)
            {
                query = query.AsNoTracking();
            }

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var entity = await query.SingleOrDefaultAsync();
            return entity != null ? new ApiResult<TEntity>(AppMessages.GetAllSuccess, entity) : new ApiResult<TEntity>(AppMessages.GetAllError, null);
        }

        public async Task<ApiResult<IList<TEntity>>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            await SaveChangesAsync();
            return new ApiResult<IList<TEntity>>(AppMessages.AddSuccess, entities.ToList());
        }

        public async Task<ApiResult<TEntity>> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await SaveChangesAsync();
            return new ApiResult<TEntity>(AppMessages.AddSuccess, entity);
        }

        public async Task<ApiResult<IList<TEntity>>> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
            await SaveChangesAsync();
            return new ApiResult<IList<TEntity>>(AppMessages.UpdateSuccess, entities.ToList());
        }

        public async Task<ApiResult<TEntity>> UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();
            return new ApiResult<TEntity>(AppMessages.UpdateSuccess, entity);
        }

        public async Task<ApiResult<TEntity>> DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await SaveChangesAsync();
            return new ApiResult<TEntity>(AppMessages.DeleteSuccess, entity);
        }

        public async Task<ApiResult<IList<TEntity>>> DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);
            await SaveChangesAsync();
            return new ApiResult<IList<TEntity>>(AppMessages.DeleteSuccess, entities.ToList());
        }

        public async Task<ApiResult<IList<TEntity>>> DeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            var entities = await _context.Set<TEntity>().Where(filter).ToListAsync();
            _context.Set<TEntity>().RemoveRange(entities);
            await SaveChangesAsync();
            return new ApiResult<IList<TEntity>>(AppMessages.DeleteSuccess, entities);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            return filter == null ? await _context.Set<TEntity>().CountAsync() : await _context.Set<TEntity>().CountAsync(filter);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _context.Set<TEntity>().AnyAsync(filter);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
