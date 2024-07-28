using Data.Models;
using Data.Utilities;
using System.Linq.Expressions;

namespace Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<ApiResult<TEntity>> GetByIdAsync(object id);
        Task<ApiResult<TEntity>> GetByIdAsync(object id, params Expression<Func<TEntity, object>>[] includes);
        Task<ApiResult<IList<TEntity>>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null,
                                                bool? asNoTracking = true,
                                                bool? deletedData = false,
                                                params Expression<Func<TEntity, object>>[] includes);
        Task<ApiResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null, bool? asNoTracking = true, bool? deletedData = false, params Expression<Func<TEntity, object>>[] includes);
        Task<ApiResult<IList<TEntity>>> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<ApiResult<TEntity>> AddAsync(TEntity entity);
        Task<ApiResult<IList<TEntity>>> UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task<ApiResult<TEntity>> UpdateAsync(TEntity entity);
        Task<ApiResult<TEntity>> DeleteAsync(TEntity entity);
        Task<ApiResult<IList<TEntity>>> DeleteRangeAsync(IEnumerable<TEntity> entities);
        Task<ApiResult<IList<TEntity>>> DeleteAsync(Expression<Func<TEntity, bool>> filter);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> filter = null);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter);
        void SaveChanges();
        Task<bool> SaveChangesAsync();
        void Dispose();
        Task<User> Authenticate(string username, string password);
        Task<User> CreateUserAsync(User user);
        Task<User> GetById(int id);

    }
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<IEnumerable<Department>> GetDepartmentsByFacultyAsync(Faculty faculty);
    }
}
