using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task CreateUserAsync(User user, string password);
        Task UpdateUserAsync(User user);
        Task DeactivateUserAsync(int userId);
        Task ResetPasswordAsync(int userId, string newPassword);
    }
}
