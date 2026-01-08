using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;

namespace SchoolManagementSystem.Data.Repositories
{
    public class UserRepository
    {
        private readonly string _conn =
            "Server=.;Database=SchoolDB;Trusted_Connection=True;TrustServerCertificate=True";

        public User? GetByUsername(string username)
        {
            using var con = new SqlConnection(_conn);
            var cmd = new SqlCommand(
                "SELECT * FROM Users WHERE Username=@u AND IsActive=1", con);

            cmd.Parameters.AddWithValue("@u", username);

            con.Open();
            using var dr = cmd.ExecuteReader();

            if (!dr.Read()) return null;

            return new User
            {
                UserId = (int)dr["UserId"],
                Username = dr["Username"].ToString()!,
                PasswordHash = dr["PasswordHash"].ToString()!,
                Role = dr["Role"].ToString(),
                IsActive = (bool)dr["IsActive"]
            };
        }
    }
}
