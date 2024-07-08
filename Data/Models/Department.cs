using Data.Models.Base;

namespace Data.Models
{
    public class Department:BaseModel
    {
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<Class> Classes { get; set; }
    }
}
