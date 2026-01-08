using BCrypt.Net;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
using SchoolManagementSystem.Common.Enums;
using SchoolManagementSystem.Common.Session;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;

namespace SchoolManagementSystem.Business.Services
{
    public class AuthService
    {
        private readonly UserRepository _repo = new();

        public User? Login(string username, string password)
        {
            using var ctx = new SchoolDbContext();         

             var user = ctx.Users.FirstOrDefault(x =>
                x.Username == username && x.IsActive);

            if (user == null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            return user;
        }

    }
}
