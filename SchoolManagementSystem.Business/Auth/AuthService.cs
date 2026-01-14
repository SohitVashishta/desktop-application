using SchoolManagementSystem.Data.Auth;
using SchoolManagementSystem.Models.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;

        public AuthService(IAuthRepository repository)
        {
            _repository = repository;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var hash = HashPassword(password);
            return await _repository.GetUserAsync(username, hash);
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
