using Data.Models;

namespace Data.Repositories
{
    public interface IStudentLessonRepository : IRepository<StudentLesson>
    {
        Task<IEnumerable<Lesson>> GetStudentLessonsAsync(int studentId, IEnumerable<int> lessonIds);
    }
}
