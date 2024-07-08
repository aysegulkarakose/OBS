using Data.Models;
using Microsoft.EntityFrameworkCore;


namespace Data.Repositories
{
    public class StudentLessonRepository : Repository<StudentLesson>, IStudentLessonRepository
    {
        private readonly AppDbContext _context;

        public StudentLessonRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Lesson>> GetStudentLessonsAsync(int studentId, IEnumerable<int> lessonIds)
        {
            return await _context.StudentLessons
                .Include(sl => sl.Lesson)
                .ThenInclude(l => l.LessonTime)
                .Where(sl => sl.StudentId == studentId && lessonIds.Contains(sl.LessonId))
                .Select(sl => sl.Lesson)
                .ToListAsync();
        }
    }
}
