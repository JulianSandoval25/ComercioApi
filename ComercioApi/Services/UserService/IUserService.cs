using ComercioApi.Models;

namespace ComercioApi.Services.UserService
{
    public interface IUserService
    {
        public Task<User> GetUserByIdAsync(int id);
        public Task<bool> UpdateUserAsync(int id, User user);
        public Task<bool> DeleteUserAsync(int id);
        public Task<IEnumerable<User>> GetUsersAsync();
        public Task<string> LoginAsync(string username, string password);
        public Task<string> RegistroUserAsync(User user);
        public string GenerateJwtToken(User user);
    }
}
