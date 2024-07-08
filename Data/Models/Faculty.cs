using Data.Models.Base;

namespace Data.Models
{
    public class Faculty:BaseModel
    {
        public ICollection<Department> Departments { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
        public ICollection<Student> Students { get; set; }

        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<Class> Classes { get; set; }
    }
}
