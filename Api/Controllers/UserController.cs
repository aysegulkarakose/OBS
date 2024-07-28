using Business.Services;
using Data.DTOs.Request;
using Data.DTOs.Response;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Security.Claims;


namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, ITokenService tokenService, IConfiguration configuration)
        {
            _userService = userService;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult DecodeToken(string token)
        {
            string key = _configuration["JwtSettings:Key"];
            string issuer = _configuration["JwtSettings:Issuer"];
            string audience = _configuration["JwtSettings:Audience"];

            TokenHelper.DecodeJwtToken(token, key, issuer, audience);
            return Ok("Token decoded successfully");
        }

        [HttpPost]

        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userService.RegisterUserAsync(request);
                return Ok(user);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var user = await _userService.AuthenticateAsync(loginRequest);
                if (user == null)
                {
                    return Unauthorized();
                }


                var token = _tokenService.GenerateJwtToken(user.Name, user.Role.ToString());
                var refreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtSettings:RefreshTokenExpirationDays"]));
                await _userService.UpdateAsync(user);


                return Ok(new LoginResponse { Token = token });
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenRequest tokenRequest)
        {
            if (ModelState.IsValid)
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(tokenRequest.Token);
                if (principal == null)
                {
                    return BadRequest("Invalid token");
                }

                var username = principal.Identity.Name;
                var user = await _userService.GetUserByNameAsync(username);

                if (user == null || user.RefreshToken != tokenRequest.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    return BadRequest("Invalid token or refresh token");
                }

                var newJwtToken = _tokenService.GenerateJwtToken(user.Name, user.Role.ToString());
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtSettings:RefreshTokenExpirationDays"]));
                await _userService.UpdateAsync(user);

                return Ok(new { Token = newJwtToken, RefreshToken = newRefreshToken });
            }

            return BadRequest(ModelState);
        }


        [HttpGet]
        [Authorize] // Kimlik doğrulaması
        public async Task<IActionResult> GetProfileAsync()
        {
            // Kullanıcı bilgileri alınır
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var user = await _userService.GetUserByNameAsync(username);

            if (user == null)
            {
                return NotFound(); // Kullanıcı bulunamazsa
            }

            var userDto = new User
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Role = user.Role,
                Department = user.Department,
                Year = user.Year,
            };

            return Ok(userDto);
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        //{
        //    var users = await _userService.GetAllUsersAsync();
        //    return Ok(users);
        //}

        [HttpGet]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(User user)
        {
            var addedUser = await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = addedUser.Id }, addedUser);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var updatedUser = await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var deletedUser = await _userService.DeleteUserAsync(id);
            if (deletedUser == null)
            {
                return NotFound();
            }
            return NoContent();
        
    }
    
}}
