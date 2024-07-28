using Data.Models.Base;

namespace Data.DTOs.Request
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
    }
}
