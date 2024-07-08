using Data.Models;
using Data.Repositories;

namespace Business.Services
{
    public interface ITeacherService
    {
        Task<Teacher> GetTeacherByIdAsync(int id);
        Task<IList<Teacher>> GetAllTeachersAsync();
        Task<Teacher> AddTeacherAsync(Teacher teacher);
        Task<Teacher> UpdateTeacherAsync(Teacher teacher);
        Task<Teacher> DeleteTeacherAsync(int id);
    }

    public class TeacherService : ITeacherService
    {
        private readonly IRepository<Teacher> _teacherRepository;

        public TeacherService(IRepository<Teacher> teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public async Task<Teacher> GetTeacherByIdAsync(int id)
        {
            var result = await _teacherRepository.GetByIdAsync(id);
            return result.Data;
        }

        public async Task<IList<Teacher>> GetAllTeachersAsync()
        {
            var result = await _teacherRepository.GetAllAsync();
            return result.Data;
        }

        public async Task<Teacher> AddTeacherAsync(Teacher teacher)
        {
            var result = await _teacherRepository.AddAsync(teacher);
            return result.Data;
        }

        public async Task<Teacher> UpdateTeacherAsync(Teacher teacher)
        {
            var result = await _teacherRepository.UpdateAsync(teacher);
            return result.Data;
        }

        public async Task<Teacher> DeleteTeacherAsync(int id)
        {
            var teacher = await GetTeacherByIdAsync(id);
            if (teacher != null)
            {
                var result = await _teacherRepository.DeleteAsync(teacher);
                return result.Data;
            }
            return null;
        }
    }
}
