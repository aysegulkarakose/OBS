using Data.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class User : BaseModel
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public Role Role { get; set; }
        public string LastName { get; set; }
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        //public int? FacultyId { get; set; }
        //public Faculty Faculty { get; set; }
        public int? Year { get; set; }
        public ICollection<StudentLesson> StudentLessons { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}