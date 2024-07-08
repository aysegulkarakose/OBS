using Data.Models.Base;

namespace Data.Models
{
        public class Teacher : BaseModel
        {
        public int Password { get; set; }
        public string LastName { get; set; }
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public ICollection<LessonTime> LessonsTime { get; set; }
        public ICollection<Registration> Registrations { get; set; }

        
    }
}
