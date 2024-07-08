namespace Data.Models
{
    public class Class
    {
        public int Id { get; set; }
        public int Credits { get; set; }
        public string Name { get; set; }
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
        public int DepartmenId { get; set; }
        public Department Department { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<LessonTime> LessonsTime { get; set; }
    }
}
