using Data.Models;
using Data.Repositories;

namespace Business.Services
{
    public interface ILessonTimeService
    {
        Task<LessonTime> GetLessonTimeByIdAsync(int id);
        Task<IList<LessonTime>> GetAllLessonTimesAsync();
        Task<LessonTime> AddLessonTimeAsync(LessonTime lessonTime);
        Task<LessonTime> UpdateLessonTimeAsync(LessonTime lessonTime);
        Task<LessonTime> DeleteLessonTimeAsync(int id);
    }

    public class LessonTimeService : ILessonTimeService
    {
        private readonly IRepository<LessonTime> _lessonTimeRepository;

        public LessonTimeService(IRepository<LessonTime> lessonTimeRepository)
        {
            _lessonTimeRepository = lessonTimeRepository;
        }

        public async Task<LessonTime> GetLessonTimeByIdAsync(int id)
        {
            var result = await _lessonTimeRepository.GetByIdAsync(id);
            return result.Data;
        }

        public async Task<IList<LessonTime>> GetAllLessonTimesAsync()
        {
            var result = await _lessonTimeRepository.GetAllAsync();
            return result.Data;
        }

        public async Task<LessonTime> AddLessonTimeAsync(LessonTime lessonTime)
        {
            var result = await _lessonTimeRepository.AddAsync(lessonTime);
            return result.Data;
        }

        public async Task<LessonTime> UpdateLessonTimeAsync(LessonTime lessonTime)
        {
            var result = await _lessonTimeRepository.UpdateAsync(lessonTime);
            return result.Data;
        }

        public async Task<LessonTime> DeleteLessonTimeAsync(int id)
        {
            var lessonTime = await GetLessonTimeByIdAsync(id);
            if (lessonTime != null)
            {
                var result = await _lessonTimeRepository.DeleteAsync(lessonTime);
                return result.Data;
            }
            return null;
        }
    }
}
