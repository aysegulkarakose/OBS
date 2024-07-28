using Data.Models.Base;

namespace Data.Models
{
    public class StudentLesson : BaseModel
    {
        public int StudentId { get; set; }
        public User Student { get; set; }

        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }


        public DateTime EnrollmentDate { get; set; }
    }
}
