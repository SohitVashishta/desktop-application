using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models.Models;
using System.Diagnostics;

namespace SchoolManagementSystem.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SchoolDbContext _context;

        public UserRepository(SchoolDbContext context)
        {
            _context = context;
        }

        // ================= ADD =================

        public async Task AddUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        // ================= GET ALL =================
        public async Task<List<User>> GetUsersAsync()
        {
            
            var users = await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.UserId)
                .ToListAsync();

            Debug.WriteLine($"Users count = {users.Count}");

            foreach (var u in users)
            {
                Debug.WriteLine(
                    $"UserId={u.UserId}, Username={u.Username}, Role={u.Role}, Active={u.IsActive}");
            }

            return users;
        }

        // ================= UPDATE USER =================

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existing = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == user.UserId);

            if (existing == null)
                throw new InvalidOperationException("User not found");

            existing.Username = user.Username;
            existing.Email = user.Email;
            existing.Role = user.Role;
            existing.IsActive = user.IsActive;

            await _context.SaveChangesAsync();
        }

        // ================= PASSWORD =================

        public async Task UpdatePasswordAsync(int userId, string passwordHash)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
                throw new InvalidOperationException("User not found");

            user.PasswordHash = passwordHash;

            await _context.SaveChangesAsync();
        }

        // ================= DEACTIVATE =================

        public async Task DeactivateUserAsync(int userId)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
                throw new InvalidOperationException("User not found");

            user.IsActive = false;

            await _context.SaveChangesAsync();
        }
    }
}
