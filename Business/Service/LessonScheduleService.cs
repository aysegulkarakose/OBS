using Data.Models;
using Data.Repositories;

namespace Business.Service
{

    public interface ILessonScheduleService : IRepository<LessonSchedule>
    {
        //Task<IEnumerable<LessonSchedule>> GetAllLessonSchedulesAsync();
        Task<LessonSchedule> AddLessonScheduleAsync(LessonSchedule lessonSchedule);
    }
    public class LessonScheduleService : Repository<LessonSchedule>, ILessonScheduleService
    {
        public LessonScheduleService(AppDbContext context) : base(context)
        {

        }

        //public async Task<IEnumerable<LessonSchedule>> GetAllLessonSchedulesAsync()
        //{
        //    var result = await GetAllAsync();
        //    return result.Data;

        //}

        public async Task<LessonSchedule> AddLessonScheduleAsync(LessonSchedule lessonSchedule)
        {
            var result = await AddAsync(lessonSchedule);
            return result.Data;
        }

    }
}
