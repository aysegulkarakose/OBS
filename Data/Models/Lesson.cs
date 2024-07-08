using Data.Models.Base;

namespace Data.Models
{
    public class Lesson: BaseModel
    {
        public int Credits { get; set; }
        public bool IsMandatory { get; set; }
        public int Year { get; set; }   //hangi senede alınabilir
        public int Quota { get; set; }  //kontenjan
        public int EnrolledStudents { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int FacultyId { get; set; }
        public Faculty Faculty { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
        public int LessonTimeId { get; set; }
        public LessonTime LessonTime { get; set; }
        public ICollection <Class> Classes {  get; set; }
        public ICollection< Teacher > Teachers {get; set; }
        public ICollection<Registration> Registrations { get; set; } // Dersi alan öğrenciler
        public ICollection<LessonTime> LessonsTime { get; set; }
        public ICollection<Student> Students { get; set; }

    }
}
