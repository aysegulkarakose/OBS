namespace Data.Models.Base
{
    public class BaseModel
    {
        public int Id { get; set; }   
        public string Name { get; set; }
        public bool isDeleted { get; set; } = false;
        public bool isActive { get; set; } = true;
    }
}
