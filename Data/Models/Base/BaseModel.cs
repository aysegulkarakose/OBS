namespace Data.Models.Base
{
    public class BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;

    }
    public enum Role
    {
        Student=1,
        Teacher=2,
        Admin=3

    }
}
