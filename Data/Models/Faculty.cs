using Data.Models.Base;

namespace Data.Models
{
    public class Faculty :BaseModel
    {
        public ICollection<Department> Departments { get; set; }

    }
}
