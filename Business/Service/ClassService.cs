using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Business.Service
{
    public interface IClassService : IRepository<Class>
    {
        Task<Class> AddClassAsync(Class @class);
        Task<IEnumerable<Class>> GetClassesAsync();
        Task DeleteClassAsync(Class @class);
        Task<IEnumerable<Class>> GetClassesByDepartmentAsync(int departmentId);
        Task<Department> GetClassByIdAsync(int id);
    }   
    
    public class ClassService : Repository<Class>, IClassService
     {
            private readonly IConfiguration _configuration;
            private readonly AppDbContext _context;

        
        public ClassService(AppDbContext context, IConfiguration configuration) : base(context)
        {
                _context = context;
                _configuration = configuration;
        }

        public async Task<Class> AddClassAsync(Class @class)
        {
            var classExists = await _context.Classes.AnyAsync(c => c.Id == @class.Id);
            if (!classExists)
            {
                var newClass = new Class
                {
                    Name = @class.Name,
                    DepartmentId = @class.DepartmentId
                };
                _context.Classes.Add(newClass);
                await _context.SaveChangesAsync();
                return newClass;
            }
            return null;
        }

        public async Task DeleteClassAsync(Class @class)
        {
            _context.Classes.Remove(@class);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Class>> GetClassesAsync()
        {
            return await _context.Classes.ToListAsync();
        }

        public async Task<IEnumerable<Class>> GetClassesByDepartmentAsync(int departmentId)
        {
            return await _context.Classes.Where(c => c.DepartmentId == departmentId).ToListAsync();
        }
        public async Task<Class> GetClassByIdAsync(int id)
        {
            var result = await GetByIdAsync(id);
            return result.Data;
        }

        Task<Department> IClassService.GetClassByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }

}
