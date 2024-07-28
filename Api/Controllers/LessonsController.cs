using Business.Service;
using Business.Services;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        private readonly IUserService _userService;
        private readonly ILessonScheduleService _lessonScheduleService;

        public LessonsController(IUserService userService, ILessonService lessonService, ILessonScheduleService lessonScheduleService)
        {
            _userService = userService;
            _lessonService = lessonService;
            _lessonScheduleService = lessonScheduleService;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<LessonSchedule>>> GetAllLessonSchedules()
        //{
        //    var lessonSchedules = await _lessonScheduleService.GetAllLessonSchedulesAsync();
        //    return Ok(lessonSchedules);
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessonTime>>> GetStudentLessonSchedule(int studentId, int daysOfWeekId)
        {
            var schedule = await _lessonService.GetLessonScheduleAsync(studentId, daysOfWeekId);
            if (schedule == null || !schedule.Any())
            {
                return NotFound();
            }
            return Ok(schedule);
        }


        [HttpGet]
        public async Task<IActionResult> GetCreditRequirementsByClassAsync(int yearId)
        {
            try
            {
                var requiredCredits = await _lessonService.GetCreditRequirementsByYearAsync(yearId);
                return Ok(requiredCredits);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckScheduleConflict(int studentId, [FromQuery] List<int> lessonIds)
        {
            bool hasConflict = await _lessonService.CheckScheduleConflict(studentId, lessonIds);

            if (hasConflict)
            {
                return BadRequest("Seçilen derslerin saatleri çakışıyor.");
            }
            else
            {
                return Ok("Seçilen derslerin saatleri çakışmıyor.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetAllLessons()
        {
            // Lesson'ları al
            var lessons = await _lessonService.GetAllLessonsAsync();

            // Eğer lessons boşsa, 204 No Content döndür
            if (lessons == null || !lessons.Any())
            {
                return NoContent();
            }

            // Listeyi doğrudan döndür
            return Ok(lessons);
        }


        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Lesson>>> GetAvailableLessons([FromQuery] int classId) //fromquery yaparak classıd bilgisini çektik api ile?
        //{
        //    var lessons = await _lessonService.GetAvailableLessonsForClassAsync(classId);
        //    return lessons;
        //}


        [HttpGet]
        public async Task<ActionResult<Lesson>> GetLessonById(int id)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(id);
            if (lesson == null)
            {
                return NotFound();
            }
            return Ok(lesson);
        }

        [HttpPost]
        public async Task<IActionResult> CheckQuota([FromBody] IEnumerable<int> lessonIds)
        {
            var isQuotaFull = await _lessonService.CheckQuotaAsync(lessonIds);

            if (isQuotaFull)
            {
                return Ok("Kontenjan dolmuştur.");
            }
            else
            {
                return Ok("Dersi seçebilirsiniz.");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddLesson([FromBody] AddLessonRequest addLessonRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var addedLesson = await _lessonService.AddLessonAsync(addLessonRequest);
                return CreatedAtAction(nameof(GetLessonById), new { id = addedLesson.Id }, addedLesson);
            }
            catch (Exception ex)
            {
              
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<ActionResult> EnrollStudent(int studentId, int lessonId)
        {
            var result = await _lessonService.EnrollStudentInLessonAsync(studentId, lessonId);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateLesson(int id, Lesson lesson)
        {
            if (id != lesson.Id)
            {
                return BadRequest();
            }

            var updatedLesson = await _lessonService.UpdateLessonAsync(lesson);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteLesson(int id)
        {
            var deletedLesson = await _lessonService.DeleteLessonAsync(id);
            if (deletedLesson == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetLessonsForStudentAsync(int classId)
        {
           
            var lessons = await _lessonService.GetLessonsByClassIdAsync(classId);

            if (lessons == null || !lessons.Any())
            {
                return NotFound(); 
            }

            return Ok(lessons);
        }
    }
}
