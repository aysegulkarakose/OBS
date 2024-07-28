using Data.Models.Base;

namespace Data.Models
{
    public class RegisterRequest
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public int? Year { get; set; }
        public string Department { get; set; }
        public int FacultyId { get; set; }
        public Role Role { get; set; } // student, teacher, admin

    }
}
