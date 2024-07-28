namespace Data.Models
{
    public class AddLessonRequest
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsMandatory { get; set; }
        public int? Year { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
        public string TeacherName { get; set; }
        public int Credits { get; set; }
        public int ClassId { get; set; }
        public string? Days { get; set; }
        public string? TimeSlot { get; set; }
        public List<Class> Classes { get; set; }
        public List<User> Users { get; set; }

    }
}
