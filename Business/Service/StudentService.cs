using Data.Models;
using Data.Repositories;

namespace Business.Services
{
    public interface IStudentService
    {
        Task<Student> GetStudentByIdAsync(int id);
        Task<IList<Student>> GetAllStudentsAsync();
        Task<Student> AddStudentAsync(Student student);
        Task<Student> UpdateStudentAsync(Student student);
        Task<Student> DeleteStudentAsync(int id);
    }

    public class StudentService : IStudentService
    {
        private readonly IRepository<Student> _studentRepository;

        public StudentService(IRepository<Student> studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            var result = await _studentRepository.GetByIdAsync(id);
            return result.Data;
        }

        public async Task<IList<Student>> GetAllStudentsAsync()
        {
            var result = await _studentRepository.GetAllAsync();
            return result.Data;
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            var result = await _studentRepository.AddAsync(student);
            return result.Data;
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            var result = await _studentRepository.UpdateAsync(student);
            return result.Data;
        }

        public async Task<Student> DeleteStudentAsync(int id)
        {
            var student = await GetStudentByIdAsync(id);
            if (student != null)
            {
                var result = await _studentRepository.DeleteAsync(student);
                return result.Data;
            }
            return null;
        }
    }
}
