using Azure.Core;
using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace Business.Services
{
    public interface ILessonService : IRepository<Lesson>
    {
        Task<Lesson> GetLessonByIdAsync(int id);
        Task<IEnumerable<Lesson>> GetLessonsByClassIdAsync(int classId);
        Task<IList<Lesson>> GetAllLessonsAsync();
        Task<Lesson> AddLessonAsync(AddLessonRequest addLessonRequest);
        Task<Lesson> UpdateLessonAsync(Lesson lesson);
        Task<bool> DeleteLessonAsync(int id);
        Task<bool> CheckScheduleConflict(int studentId, IEnumerable<int> lessonIds);
        Task<bool> CheckQuotaAsync(IEnumerable<int> lessonIds);
        //Task<List<Lesson>> GetAvailableLessonsForClassAsync(int classId); //belirtilen sınıf için seçilebilecek dersler
        Task<int> GetCreditRequirementsByYearAsync(int yearId);
        Task<bool> EnrollStudentInLessonAsync(int studentId, int lessonId);
        Task<List<LessonTime>> GetLessonScheduleAsync(int studentId, int daysOfWeekId);
        Task<IEnumerable<Lesson>> GetStudentLessonsAsync(int studentId, IEnumerable<int> lessonIds);
        Task<IList<Lesson>> GetLessonsForStudentAsync(int classId);
    }

    public class LessonService : Repository<Lesson>, ILessonService
    {

        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        public LessonService(AppDbContext context, IConfiguration configuration) : base(context)
        {
            _context = context;
            _configuration = configuration;
        }
      
        public async Task<IEnumerable<Lesson>> GetLessonsByClassIdAsync(int classId)
        {
            return await _context.Lessons
                                 .Where(l => l.ClassId == classId)
                                 .ToListAsync();
        }

        public async Task<Lesson> GetLessonByIdAsync(int id)
        {
            var result = await GetByIdAsync(id);
            return result.Data;
        }

        public async Task<IList<Lesson>> GetAllLessonsAsync()
        {
            var result = await GetAllAsync<Lesson>(
                filter: null,
                asNoTracking: true,
                deletedData: false
            );

            if (result == null)
            {
                Console.WriteLine("GetAllLessonsAsync: ApiResult is null.");
                throw new Exception("GetAllLessonsAsync: ApiResult is null.");
            }

            if (result.Data == null)
            {
                Console.WriteLine("GetAllLessonsAsync: ApiResult Data is null.");
                throw new Exception("GetAllLessonsAsync: ApiResult Data is null.");
            }

            if (!result.Data.Any())
            {
                Console.WriteLine("GetAllLessonsAsync: No lessons found in ApiResult Data.");
                throw new Exception("GetAllLessonsAsync: No lessons found in ApiResult Data.");
            }

            Console.WriteLine($"GetAllLessonsAsync: Retrieved {result.Data.Count} lessons.");

            return result.Data;
        }





        public async Task<Lesson> AddLessonAsync(AddLessonRequest addLessonRequest)
        {
            var lesson = new Lesson
            {
                Name = addLessonRequest.Name,
                IsActive = addLessonRequest.IsActive,
                Credits = addLessonRequest.Credits,
                IsMandatory = addLessonRequest.IsMandatory,
            };

            // Log the lesson details before adding
            Console.WriteLine($"Attempting to add lesson: {addLessonRequest.Name}");

            await AddAsync(lesson);

         

            // Log after saving changes
            Console.WriteLine("Lesson added to the database.");

            return lesson;
        }



        public async Task<Lesson> UpdateLessonAsync(Lesson lesson)
        {
            var result = await UpdateAsync(lesson);
            return result.Data;
        }

        public async Task<bool> DeleteLessonAsync(int id)
        {
            var lesson = await GetLessonByIdAsync(id);
            if (lesson != null)
            {
                var result = await DeleteAsync(lesson);
                return result.Data != null;
            }
            return false;
        }

        public async Task<bool> CheckScheduleConflict(int studentId, IEnumerable<int> lessonIds)
        {
            var selectedLessons = await GetStudentLessonsAsync(studentId, lessonIds);

            foreach (var lesson in selectedLessons)
            {
                foreach (var otherLesson in selectedLessons)
                {
                    if (lesson.Id != otherLesson.Id)
                    {
                        foreach (var lessonTime in lesson.LessonsTime)
                        {
                            foreach (var otherLessonTime in otherLesson.LessonsTime)
                            {
                                // Gün çakışması kontrolü
                                foreach (var day in lessonTime.DaysOfWeeks)
                                {
                                    if (otherLessonTime.DaysOfWeeks.Any(d => d.Id == day.Id))
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
                    }
                }
            }
            return false; // Çakışma yok
        }

        public async Task<bool> CheckQuotaAsync(IEnumerable<int> lessonIds)
        {
            // Belirtilen lessonIds içindeki derslerin detaylarını çekiyoruz
            var lessonTimes = await _context.LessonsTime
                .Where(lt => lessonIds.Contains(lt.LessonId))
                .ToListAsync();

            // Her bir ders zamanı için kontenjan kontrolü 
            foreach (var lessonTime in lessonTimes)
            {
                // Kayıtlı öğrenci sayısını veritabanından çekiyoruz
                var enrolledStudentsCount = await _context.Users
                   .CountAsync(u => u.StudentLessons.Any(sl => sl.LessonId == lessonTime.LessonId));

                var quota = lessonTime.Quota;

                if (enrolledStudentsCount >= quota)
                {
                    return true; // kontenjan dolu
                }
            }
            return false; // kontenjan boş
        }

        //public async Task<List<Lesson>> GetAvailableLessonsForClassAsync(int classId)
        //{
        //    //// Sınıfın zorunlu derslerin detaylarını çekiyoruz

        //    //var mandatoryLessons = await GetAllAsync(
        //    //       filter: l => l.IsMandatory,
        //    //       includes: [l => l.Users]
        //    //   );

        //    //// Sınıfın zorunlu derslerini sıralıyoruz
        //    //var orderedLessons = mandatoryLessons.Data.OrderBy(l => l.Year).ThenBy(l => l.Name).ToList();

        //    //// Sınıfın zorunlu dersleri dışındaki diğer dersleri alıyoruz
        //    //var optionalLessons = await GetAllAsync(
        //    //    filter: l => !l.IsMandatory && l.ClassId == classId,
        //    //    includes: [l => l.Users]
        //    //);

        //    //// Tüm dersleri birleştirme
        //    //orderedLessons.AddRange(optionalLessons.Data.OrderBy(l => l.Year).ThenBy(l => l.Name));

        //    //return orderedLessons;
        //}

        public async Task<int> GetCreditRequirementsByYearAsync(int yearId)
        {
            var _year = await _context.Classes.FirstOrDefaultAsync(y => y.Id == yearId);
            if (_year != null)
            {
                return _year.RequiredCredits;
            }
            return 0;
        }

        public async Task<bool> EnrollStudentInLessonAsync(int studentId, int lessonId)
        {
            var studentLesson = new StudentLesson { StudentId = studentId, LessonId = lessonId };
            _context.StudentLessons.Add(studentLesson);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<LessonTime>> GetLessonScheduleAsync(int studentId, int daysOfWeekId)
        {
            var classSchedule = await _context.LessonsTime
                .Where(lt => lt.DaysOfWeekId == daysOfWeekId)
                .OrderBy(lt => lt.StartTime)
                .ToListAsync();

            foreach (var lessonTime in classSchedule)
            {
                lessonTime.DaysOfWeeks = await _context.DaysOfWeeks
                    .Where(d => d.Id == lessonTime.DaysOfWeekId && d.ClassId == studentId)
                    .ToListAsync();
            }

            return classSchedule.Where(lt => lt.DaysOfWeeks.Any()).ToList();
        }

        public async Task<IEnumerable<Lesson>> GetStudentLessonsAsync(int studentId, IEnumerable<int> lessonIds)
        {
            var lessons = await _context.Set<StudentLesson>()
                .Where(sl => sl.StudentId == studentId && lessonIds.Contains(sl.LessonId))
                .Select(sl => sl.Lesson)
                .ToListAsync();

            return lessons;
        }
        public async Task<IList<Lesson>> GetLessonsForStudentAsync(int classId)
        {
            var lessons = await _context.Lessons.Where(l => l.ClassId == classId).ToListAsync();
            return lessons;
        }

        
    }
}
 