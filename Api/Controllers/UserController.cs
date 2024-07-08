using Api.DTOs.Request;
using Api.DTOs.Response;
using Business.Services;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var user = await _userService.AuthenticateAsync(loginRequest.UserName, loginRequest.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = _tokenService.GenerateJwtToken(user.UserName, user.Role);

            return Ok(new LoginResponse { Token = token });
        }
    }
}
