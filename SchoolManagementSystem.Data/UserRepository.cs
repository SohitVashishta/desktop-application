using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models.Models;
using System.Data;

namespace SchoolManagementSystem.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SchoolDbContext _context;

        public UserRepository(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public Task DeactivateUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }
        public Task UpdatePasswordAsync(int userId, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
