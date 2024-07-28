namespace Data.DTOs.Request
{
    public class TokenRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
