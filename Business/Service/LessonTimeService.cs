using Data.Models;
using Data.Repositories;

namespace Business.Services
{
    public interface ILessonTimeService : IRepository<LessonTime>
    {
        Task<LessonTime> GetLessonTimeByIdAsync(int id);
        //Task<IList<LessonTime>> GetAllLessonTimesAsync();
        Task<LessonTime> AddLessonTimeAsync(LessonTime lessonTime);
        Task<LessonTime> UpdateLessonTimeAsync(LessonTime lessonTime);
        Task<LessonTime> DeleteLessonTimeAsync(int id);
    }

    public class LessonTimeService : Repository<LessonTime>, ILessonTimeService
    {
        public LessonTimeService(AppDbContext context) : base(context)
        {
        }

        public async Task<LessonTime> GetLessonTimeByIdAsync(int id)
        {
            var result = await GetByIdAsync(id);
            return result.Data;
        }

        //public async Task<IList<LessonTime>> GetAllLessonTimesAsync()
        //{
        //    var result = await GetAllAsync();
        //    return result.Data;
        //}

        public async Task<LessonTime> AddLessonTimeAsync(LessonTime lessonTime)
        {
            var result = await AddAsync(lessonTime);
            return result.Data;
        }

        public async Task<LessonTime> UpdateLessonTimeAsync(LessonTime lessonTime)
        {
            var result = await UpdateAsync(lessonTime);
            return result.Data;
        }

        public async Task<LessonTime> DeleteLessonTimeAsync(int id)
        {
            var lessonTime = await GetLessonTimeByIdAsync(id);
            if (lessonTime != null)
            {
                var result = await DeleteAsync(lessonTime);
                return result.Data;
            }
            return null;
        }
    }
}
