namespace Data.Models
{
    public class DaysOfWeek
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ClassId { get; set; }
        public LessonSchedule Lessons { get; set; }
    }
}

