using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task UpdatePasswordAsync(int userId, string passwordHash);
        Task DeactivateUserAsync(int userId);
    }

}
