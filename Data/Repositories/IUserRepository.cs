using Data.Models;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IUserRepository
    {
        Task<User> Authenticate(string username, string password);
        Task<User> GetById(int id);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<User> DeleteAsync(User user);
    }
}
