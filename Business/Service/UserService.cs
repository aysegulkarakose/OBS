using Data.DTOs.Request;
using Data.Models;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace Business.Services
{
    public interface IUserService : IRepository<User>
    {
        Task<User> AuthenticateAsync(LoginRequest loginRequest);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByNameAsync(string username);
        //Task<IList<User>> GetAllUsersAsync();
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<User> DeleteUserAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetProfileAsync(int userId);
        Task<User> RegisterUserAsync(RegisterRequest request);
       
    }

    public class UserService : Repository<User>, IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public UserService(AppDbContext context, IConfiguration configuration) : base(context)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<User> RegisterUserAsync(RegisterRequest request)
        {
            var user = new User
            {
                Name = request.Username,
                Password = request.Password,
                LastName = request.LastName,
                Year = request.Year,
                Role = request.Role
            };

            await AddAsync(user);
            return user;
        }

        public async Task<User> GetUserByNameAsync(string username)
        {
            var result = await GetAsync(u => u.Name == username);
            return result.Data;

        }
        public async Task<User> AuthenticateAsync(LoginRequest loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == loginRequest.UserName && u.Password == loginRequest.Password);
            return user;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var result = await GetByIdAsync(id);
            return result.Data;
        }

        //public async Task<IList<User>> GetAllUsersAsync()
        //{
        //    var result = await GetAllAsync();
        //    return result.Data;
        //}

        public async Task<User> AddUserAsync(User user)
        {
            await AddAsync(user);
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            await UpdateAsync(user);
            return user;
        }

        public async Task<User> DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                await DeleteAsync(user);
                return user;
            }
            return null;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Name == username);
        }

        public async Task<User> GetProfileAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

       
    }
}

