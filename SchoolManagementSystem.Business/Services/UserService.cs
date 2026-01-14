using SchoolManagementSystem.Common.Security;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;

namespace SchoolManagementSystem.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _repo.GetUsersAsync();
        }

        // ============================
        // ADD USER
        // ============================
        public async Task CreateUserAsync(User user, string password)
        {
            user.PasswordHash = PasswordHasher.Hash(password);
            user.IsActive = true;

            await _repo.AddUserAsync(user);
        }

        // ============================
        // UPDATE USER (NO PASSWORD)
        // ============================
        public async Task UpdateUserAsync(User user)
        {
            if (user.UserId <= 0)
                throw new ArgumentException("Invalid user");

            ValidateUser(user);
            await _repo.UpdateUserAsync(user);
        }

        // ============================
        // RESET PASSWORD
        // ============================
        public async Task ResetPasswordAsync(int userId, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Password required");

            var hash = PasswordHasher.Hash(newPassword);
            await _repo.UpdatePasswordAsync(userId, hash);
        }

        public async Task DeactivateUserAsync(int userId)
        {
            await _repo.DeactivateUserAsync(userId);
        }

        private void ValidateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username required");

            if (user.Role==null)
                throw new ArgumentException("Role required");
        }
    }
}
