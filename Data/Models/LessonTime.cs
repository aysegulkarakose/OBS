namespace Data.Models
{
    public class LessonTime
    {   
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string DaysOfWeek { get; set; }
        public int LessonId { get; set; } 
        public Lesson Lesson { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<Class> Classes { get; set; }

    }
}
