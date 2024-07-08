using Data.Models;
using System.Linq;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public interface ILessonService
    {
        Task<Lesson> GetLessonByIdAsync(int id);
        Task<IList<Lesson>> GetAllLessonsAsync();
        Task<Lesson> AddLessonAsync(Lesson lesson);
        Task<Lesson> UpdateLessonAsync(Lesson lesson);
        Task<bool> DeleteLessonAsync(int id);
        Task<bool> CheckScheduleConflict(int studentId, IEnumerable<int> lessonIds);
        Task<bool> CheckQuotaAsync(IEnumerable<int> lessonIds);
        Task<List<Lesson>> GetAvailableLessonsForClassAsync(int classId); //belirtilen sınıf için seçilebilecek dersler
    }

    public class LessonService : ILessonService
    {
        private readonly IRepository<Lesson> _lessonRepository;
        private readonly AppDbContext _context;

        public LessonService(IRepository<Lesson> lessonRepository, AppDbContext context)
        {
            _lessonRepository = lessonRepository;
            _context = context;
        }

        public async Task<Lesson> GetLessonByIdAsync(int id)
        {
            var result = await _lessonRepository.GetByIdAsync(id);
            return result.Data;
        }

        public async Task<IList<Lesson>> GetAllLessonsAsync()
        {
            var result = await _lessonRepository.GetAllAsync();
            return result.Data;
        }

        public async Task<Lesson> AddLessonAsync(Lesson lesson)
        {
            var result = await _lessonRepository.AddAsync(lesson);
            return result.Data;
        }

        public async Task<Lesson> UpdateLessonAsync(Lesson lesson)
        {
            var result = await _lessonRepository.UpdateAsync(lesson);
            return result.Data;
        }

        public async Task<bool> DeleteLessonAsync(int id)
        {
            var lesson = await GetLessonByIdAsync(id);
            if (lesson != null)
            {
                var result = await _lessonRepository.DeleteAsync(lesson);
                return result.Data != null;
            }
            return false;
        }
        public async Task<bool> CheckQuotaAsync(IEnumerable<int> lessonIds)
        {
            // Belirtilen lessonIds içindeki derslerin detaylarını çekiyoruz
            var lessons = await _context.Set<Lesson>()
                 .Where(l => lessonIds.Contains(l.Id))
                 .Include(l => l.EnrolledStudents)
                 .Include(l => l.Quota)
                 .ToListAsync();

            //toplam kayıtlı öğrenci sayısı
            var totalEnrolled = lessons.Sum(l => l.EnrolledStudents);

            // Derslerin Id'lerine göre kontenjanlarını dictionary olarak alıyoruz
            var lessonQuotas = lessons.ToDictionary(l => l.Id, l => l.Quota);

            // Her bir ders için kontenjan kontrolü 
            foreach (var lessonId in lessonIds)
            {               
                if (lessonQuotas.TryGetValue(lessonId, out var quota) && totalEnrolled >= quota)
                {
                    return true; // Kontenjan dolu
                }
            }
            return false; // Kontenjan dolu değil
        }

        //Ders çakışma kontrolü
        public async Task<bool> CheckScheduleConflict(int studentId, IEnumerable<int> lessonIds) 
        {
            var selectedLessons = await GetStudentLessonsAsync(studentId, lessonIds);

            foreach (var lesson in selectedLessons)
            {
                foreach (var otherLesson in selectedLessons)
                {
                    if (lesson.Id != otherLesson.Id)
                    {
                        var lessonTime = lesson.LessonTime;
                        var otherLessonTime = otherLesson.LessonTime;

                    // Gün çakışması kontrolü

                        if (otherLessonTime.DaysOfWeek == lessonTime.DaysOfWeek)

                            {
                    // Saat çakışması kontrolü

                        if (otherLessonTime.StartTime < lessonTime.EndTime && otherLessonTime.EndTime > lessonTime.StartTime)
                        {  
                           return true; // Çakışma var
                            }
                        }
                    }
                }
            }
            return false; // Çakışma yok
        }

        //Belirtilen sınıf için seçilebilecek dersler
        public async Task<List<Lesson>> GetAvailableLessonsForClassAsync(int classId)
        {
            // Sınıfın zorunlu derslerin detaylarını çekiyoruz

            var mandatoryLessons = await _lessonRepository.GetAllAsync(
                   filter: l => l.IsMandatory,
                   includes: [l => l.LessonTime
                    ,l => l.Teachers
                    ,l => l.Department
                    ,l => l.Faculty]
               );
               
            // Sınıfın zorunlu derslerini sıralıyoruz
            var orderedLessons = mandatoryLessons.Data.OrderBy(l => l.Year).ThenBy(l => l.Name).ToList();

            // Sınıfın zorunlu dersleri dışındaki diğer dersleri alıyoruz
            var optionalLessons = await _lessonRepository.GetAllAsync(
                filter: l => !l.IsMandatory && l.ClassId == classId,
                includes: [l => l.LessonTime
                    ,l => l.Teachers
                    ,l => l.Department
                    ,l => l.Faculty]
            );

            // Tüm dersleri birleştirme
           orderedLessons.AddRange(optionalLessons.Data.OrderBy(l => l.Year).ThenBy(l => l.Name));

            return orderedLessons;
        }
        private async Task<IEnumerable<Lesson>> GetStudentLessonsAsync(int studentId, IEnumerable<int> lessonIds)
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
