using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Auth
{
    public interface IAuthRepository
    {
        Task<User?> GetUserAsync(string username, string passwordHash);
    }

}
