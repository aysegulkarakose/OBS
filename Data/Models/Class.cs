using Data.Models.Base;

namespace Data.Models
{
    public class Class:BaseModel
    {
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<LessonTime> LessonsTime { get; set; }
        public int RequiredCredits { get; set; }
    }
}