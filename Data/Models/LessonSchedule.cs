using Data.Models.Base;

namespace Data.Models
{
    public class LessonSchedule : BaseModel
    {
        public string LessonNames { get; set; }
        public string TeachersNames { get; set; }
        public string ClassNames { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}
