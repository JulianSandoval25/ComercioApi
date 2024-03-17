using ComercioApi.Context;
using ComercioApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ComercioApi.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private IConfiguration _configuration;
        public UserService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _context.User.Where(u => u.Id == id).Select(u => new User
                         {
                             Id = u.Id,
                             userName = u.userName,
                             password = "",
                             rol = u.rol,
                             
                         })
                         .FirstOrDefaultAsync();
            return user;
        }
        public async Task<bool> UpdateUserAsync(int id, User user)
        {
            var userExisting = await _context.User.FindAsync(id);
            if (userExisting == null)
            {
                return false;
            }

            _context.Entry(userExisting).CurrentValues.SetValues(user);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var users= await _context.User
                         .Select(u => new User
                         {
                             Id = u.Id,
                             userName = u.userName,
                             password = "",
                             rol = u.rol
                         })
                         .ToListAsync();
            return users;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.userName == username && u.password == password);
            if (user == null)
            {
                return null;
            }
            return GenerateJwtToken(user);
        }

        public async Task<string> RegistroUserAsync(User user)
        {
           
            user.rol = "usuario";
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return GenerateJwtToken(user);
      
        }

        public string GenerateJwtToken(User user)
        {
            var jwt = _configuration.GetSection("JWT").Get<Jwt>();
            var claims = new ClaimsIdentity(new Claim[]
            {
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("userName", user.userName),
                new Claim(ClaimTypes.Role, user.rol)

            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var expires = DateTime.UtcNow.AddDays(4);
            var tokenDes = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = expires,
                SigningCredentials = singIn
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDes);

            return tokenHandler.WriteToken(token);
        }

        
    }
}
