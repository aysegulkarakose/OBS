using Data.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class User:BaseModel
    {
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
       
    }
}