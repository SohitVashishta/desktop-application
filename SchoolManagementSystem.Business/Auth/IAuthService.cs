using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Auth
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(string username, string password);
    }
}
