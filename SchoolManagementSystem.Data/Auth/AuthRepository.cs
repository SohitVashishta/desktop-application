using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using SchoolManagementSystem.Models.Models;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly string _connectionString;

        public AuthRepository()
        {
            var cs = ConfigurationManager.ConnectionStrings["SchoolDb"]
                     ?? throw new InvalidOperationException(
                         "Connection string 'SchoolDb' not found in App.config");

            _connectionString = cs.ConnectionString;
        }

        public async Task<User?> GetUserAsync(string username, string passwordHash)
        {
            using var con = new SqlConnection(_connectionString);

            var sql = @"
                SELECT UserId, Username, Role
                FROM Users
                WHERE Username = @Username
                  AND PasswordHash = @PasswordHash
                  AND IsActive = 1";

            return await con.QueryFirstOrDefaultAsync<User>(
                sql,
                new { Username = username, PasswordHash = passwordHash });
        }
    }
}
