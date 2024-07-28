namespace Data.Models
{
    public class LessonTime
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Quota { get; set; }  //kontenjan
        public ICollection<User> EnrolledStudents { get; set; }
        public ICollection<DaysOfWeek> DaysOfWeeks { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public int LessonId { get; set; }
        public int DaysOfWeekId { get; set; }
        public Lesson Lesson { get; set; }

    }
}
