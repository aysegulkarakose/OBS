using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Business.Service
{
    public interface IDepartmentService : IRepository<Department>
    {
        Task<Department> AddDepartmentAsync(Department department);
        Task DeleteDepartmentAsync(Department department);
        Task<IEnumerable<Department>> GetDepartmentsAsync();
        Task<IEnumerable<Department>> GetDepartmentsByFacultyAsync(int FacultyId);
        IEnumerable<Faculty> GetFaculties();
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(int id);

    }

        public class DepartmentService : Repository<Department>, IDepartmentService
        {
            private readonly IConfiguration _configuration;
            private readonly AppDbContext _context;

            public DepartmentService(AppDbContext context, IConfiguration configuration) : base(context)
            {
                _context = context;
                _configuration = configuration;
            }


            public async Task<Department> GetDepartmentByIdAsync(int id)
            {
                var result = await GetByIdAsync(id);
                return result.Data;
            }


            public async Task<Department> AddDepartmentAsync(Department department)
            {
                var departmentExists = await _context.Departments.AnyAsync(d => d.Id == department.Id);
                if (!departmentExists)
                {
                    var newDepartment = new Department
                    {
                        Id = department.Id,
                        Name = department.Name,
                        FacultyId = department.FacultyId,
                        Faculty = department.Faculty,
                        IsDeleted = department.IsDeleted,
                        IsActive = department.IsActive
                    };
                    _context.Departments.Add(newDepartment);
                    await _context.SaveChangesAsync();
                    return newDepartment;
                }
                return null;
            }

            public async Task DeleteDepartmentAsync(Department department)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }

            public async Task<IEnumerable<Department>> GetDepartmentsAsync()
            {
                return await _context.Departments.ToListAsync();
            }

            public async Task<IEnumerable<Department>> GetDepartmentsByFacultyAsync(int facultyId)
            {
                return await _context.Departments.Where(d => d.FacultyId == facultyId).ToListAsync();
            }

            public IEnumerable<Faculty> GetFaculties()
            {
                return _context.Faculties.AsNoTracking().ToList();
            }

            public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
            {
                return await _context.Departments.ToListAsync();
            }

            
    }
    }


