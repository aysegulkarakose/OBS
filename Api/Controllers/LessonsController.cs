using Business.Services;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonsController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }
        [HttpGet("check-schedule-conflict")]
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
                var lessons = await _lessonService.GetAllLessonsAsync();
                return Ok(lessons);
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Lesson>> GetLessonById(int id)
            {
                var lesson = await _lessonService.GetLessonByIdAsync(id);
                if (lesson == null)
                {
                    return NotFound();
                }
                return Ok(lesson);
            }

        [HttpPost("check-quota")]
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
            public async Task<ActionResult> AddLesson(Lesson lesson)
            {
                var addedLesson = await _lessonService.AddLessonAsync(lesson);
                return CreatedAtAction(nameof(GetLessonById), new { id = addedLesson.Id }, addedLesson);
            }

            [HttpPut("{id}")]
            public async Task<ActionResult> UpdateLesson(int id, Lesson lesson)
            {
                if (id != lesson.Id)
                {
                    return BadRequest();
                }

                var updatedLesson = await _lessonService.UpdateLessonAsync(lesson);
                return NoContent();
            }

            [HttpDelete("{id}")]
            public async Task<ActionResult> DeleteLesson(int id)
            {
                var deletedLesson = await _lessonService.DeleteLessonAsync(id);
                if (deletedLesson == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
        }
    } 
