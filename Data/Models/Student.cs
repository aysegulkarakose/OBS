using Data.Models.Base;

namespace Data.Models
{
    public class Student : BaseModel
    {
        public int Password { get; set; }
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
        public int DepartmenId { get; set; }
        public Department Department { get; set; }
        public string LastName { get; set; }
        public int Year { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
        public ICollection<LessonTime> LessonsTime { get; set; }
        public  ICollection <Class> Classes { get; set; }
        public ICollection<Registration> Registrations { get; set; }
    }
}
