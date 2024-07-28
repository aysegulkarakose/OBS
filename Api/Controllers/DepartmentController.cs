using Business.Service;
using Business.Services;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly AppDbContext _context;
        private readonly UserService _userService;
   
        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }


        //  {      private static readonly List<Department> Departments = new List<Department>

        //{
        //    new Department { Id = 1, Name = "Fen Bilgisi Öğretmenliği", Faculty = Faculty.EğitimFakültesi },
        //    new Department { Id = 2, Name = "Sınıf Öğretmenliği", Faculty = Faculty.EğitimFakültesi },
        //    new Department { Id = 3, Name = "Okul Öncesi Öğretmenliği", Faculty = Faculty.EğitimFakültesi },
        //    new Department { Id = 4, Name = "Matematik Öğretmenliği", Faculty = Faculty.EğitimFakültesi },

        //    new Department { Id = 5, Name = "Bilgisayar Mühendisliği", Faculty = Faculty.ElektrikElektronik },
        //    new Department { Id = 6, Name = "Elektrik Mühendisliği", Faculty = Faculty.ElektrikElektronik },
        //    new Department { Id = 7, Name = "Elektronik ve Haberleşme Mühendisliği", Faculty = Faculty.ElektrikElektronik },
        //    new Department { Id = 8, Name = "Kontrol ve Otomasyon Mühendisliği", Faculty = Faculty.ElektrikElektronik },

        //    new Department { Id = 9, Name = "Kimya", Faculty = Faculty.FenEdebiyat },
        //    new Department { Id = 10, Name = "Türk Dili ve Edebiyatı", Faculty = Faculty.FenEdebiyat },
        //    new Department { Id = 11, Name = "Fizik", Faculty = Faculty.FenEdebiyat },
        //    new Department { Id = 12, Name = "İstatistik", Faculty = Faculty.FenEdebiyat },

        //    new Department { Id = 13, Name = "Gemi İnşaatı ve Gemi Makineleri Mühendisliği", Faculty = Faculty.Denizcilik },

        //    new Department { Id = 14, Name = "İktisat", Faculty = Faculty.İktisadiveİdariBilimler },
        //    new Department { Id = 15, Name = "İşletme", Faculty = Faculty.İktisadiveİdariBilimler },
        //    new Department { Id = 16, Name = "Siyaset Bilimi ve Uluslararası İlişkiler", Faculty = Faculty.İktisadiveİdariBilimler },

        //    new Department { Id = 17, Name = "İnşaat Mühendisliği", Faculty = Faculty.İnşaat },
        //    new Department { Id = 18, Name = "Çevre Mühendisliği", Faculty = Faculty.İnşaat },

        //    new Department { Id = 19, Name = "Kimya Mühendisliği", Faculty = Faculty.KimyaMetalürji },
        //    new Department { Id = 20, Name = "Metalürji ve Malzeme Mühendisliği", Faculty = Faculty.KimyaMetalürji},

        //    new Department { Id = 21, Name = "Makine Mühendisliği", Faculty = Faculty.Makine },
        //    new Department { Id = 22, Name = "Mekatronik Mühendisliği", Faculty = Faculty.Makine },
        //    new Department { Id = 23, Name = "Endüstri Mühendisliği", Faculty = Faculty.Makine },

        //    new Department { Id = 24, Name = "Mimarlık", Faculty = Faculty.Mimarlık },
        //    new Department { Id = 25, Name = "Şehir ve Bölge Planlama", Faculty = Faculty.Mimarlık },

        //}; }
        
        [HttpGet]
        public IActionResult GetFaculties()
        {
            var faculties = _departmentService.GetFaculties();

            return Ok(faculties);
        }



        [HttpGet]
        public async Task<IActionResult> GetDepartments()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(departments);
        }


        [HttpGet]
        public async Task<IActionResult> GetDepartmentsByFaculty(int facultyId)
        {
            var departments = await _departmentService.GetDepartmentsByFacultyAsync(facultyId);
            return Ok(departments);
        }

       


        [HttpPost]
        public async Task<ActionResult<Department>> AddDepartment([FromBody] Department department)
        {
            if (department == null)
            {
                return BadRequest("Department cannot be null.");
            }

            var addedDepartment = await _departmentService.AddDepartmentAsync(department);

            if (addedDepartment == null)
            {
                return Conflict("Department already exists or could not be added.");
            }

            return CreatedAtAction(
                nameof(GetDepartmentById),
                new { id = addedDepartment.Id },
                addedDepartment
            );
        }

        [HttpGet]
        public async Task<ActionResult<Department>> GetDepartmentById(int id)
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }



        [HttpDelete]
        public async Task<ActionResult> DeleteDepartment(int id)
        {
       
            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
             
                return NotFound();
            }

         
            await _departmentService.DeleteDepartmentAsync(department);

           
            return NoContent();
        }

    }
    }
