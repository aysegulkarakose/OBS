using Business.Services;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LessonTimesController : ControllerBase
    {
        private readonly ILessonTimeService _lessonTimeService;

        public LessonTimesController(ILessonTimeService lessonTimeService)
        {
            _lessonTimeService = lessonTimeService;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<LessonTime>>> GetAllLessonTimes()
        //{
        //    var lessonTimes = await _lessonTimeService.GetAllLessonTimesAsync();
        //    return Ok(lessonTimes);
        //}

        [HttpGet]
        public async Task<ActionResult<LessonTime>> GetLessonTimeById(int id)
        {
            var lessonTime = await _lessonTimeService.GetLessonTimeByIdAsync(id);
            if (lessonTime == null)
            {
                return NotFound();
            }
            return Ok(lessonTime);
        }

        [HttpPost]
        public async Task<ActionResult> AddLessonTime(LessonTime lessonTime)
        {
            var addedLessonTime = await _lessonTimeService.AddLessonTimeAsync(lessonTime);
            return CreatedAtAction(nameof(GetLessonTimeById), new { id = addedLessonTime.Id }, addedLessonTime);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateLessonTime(int id, LessonTime lessonTime)
        {
            if (id != lessonTime.Id)
            {
                return BadRequest();
            }

            var updatedLessonTime = await _lessonTimeService.UpdateLessonTimeAsync(lessonTime);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteLessonTime(int id)
        {
            var deletedLessonTime = await _lessonTimeService.DeleteLessonTimeAsync(id);
            if (deletedLessonTime == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
