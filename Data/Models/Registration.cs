namespace Data.Models
{
    public class Registration
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
    }
}
