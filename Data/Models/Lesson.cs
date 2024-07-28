using Data.Models.Base;

namespace Data.Models
{
    public class Lesson : BaseModel
    {
        public int Credits { get; set; }
        public bool IsMandatory { get; set; }
        public int Year { get; set; }   //hangi senede alınabilir

        public int ClassId { get; set; }
        public Class Class { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<LessonTime> LessonsTime { get; set; }
        public ICollection<StudentLesson> StudentLessons { get; set; }

    }
}
