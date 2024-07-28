using Business.Service;
using Business.Services;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        private readonly AppDbContext _context;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpGet]
        public async Task<IActionResult> GetClassById(int id)
        {
            var @class = await _classService.GetClassByIdAsync(id);
            if (@class == null)
            {
                return NotFound();
            }
            return Ok(@class);
        }


        [HttpPost]
        public async Task<IActionResult> AddClass([FromBody] Class @class)
        {
            if (@class == null)
            {
                return BadRequest("Class is null.");
            }

            var addedClass = await _classService.AddClassAsync(@class);
            if (addedClass == null)
            {
                return Conflict("Class already exists or could not be added.");
            }

            return CreatedAtAction(
                nameof(GetClassById),
                new { id = addedClass.Id },
                addedClass
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetClassesByDepartment(int departmentId)
        {
            var classes = await _classService.GetClassesByDepartmentAsync(departmentId);
            if (classes == null || !classes.Any())
            {
                return NotFound();
            }
            return Ok(classes);
        }
    }
}