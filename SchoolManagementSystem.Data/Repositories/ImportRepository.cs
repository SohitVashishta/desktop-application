using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;
using System.Data;

namespace SchoolManagementSystem.Data.Repositories
{
    public class ImportRepository : IImportRepository
    {
        private readonly string _connectionString;

        // ✅ Inject connection string via DI
        public ImportRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SchoolDb")
                ?? throw new ArgumentNullException(nameof(configuration), "Connection string not found");
        }

        // =============================
        // IMPORT STUDENTS (ASYNC + TRANSACTION SAFE)
        // =============================
        public async Task ImportStudentsAsync(List<Student> students)
        {
            if (students == null || students.Count == 0)
                return;

            await using var con = new SqlConnection(_connectionString);
            await con.OpenAsync();

            await using var tran = await con.BeginTransactionAsync();

            try
            {
                foreach (var s in students)
                {
                    await using var cmd = new SqlCommand(@"
                        INSERT INTO Students
                        (FirstName, LastName, Email, DateOfBirth, EnrollmentDate)
                        VALUES
                        (@fn, @ln, @em, @dob, @ed)",
                        con, (SqlTransaction)tran);

                    cmd.Parameters.Add("@fn", SqlDbType.NVarChar).Value = s.FirstName;
                    cmd.Parameters.Add("@ln", SqlDbType.NVarChar).Value = s.LastName;
                    cmd.Parameters.Add("@em", SqlDbType.NVarChar).Value = s.Email;
                    cmd.Parameters.Add("@dob", SqlDbType.Date).Value = s.DateOfBirth;
                    cmd.Parameters.Add("@ed", SqlDbType.Date).Value = s.EnrollmentDate;

                    await cmd.ExecuteNonQueryAsync();
                }

                await tran.CommitAsync();
            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }

        // =============================
        // IMPORT TEACHERS (ASYNC + TRANSACTION SAFE)
        // =============================
        public async Task ImportTeachersAsync(List<Teacher> teachers)
        {
            if (teachers == null || teachers.Count == 0)
                return;

            await using var con = new SqlConnection(_connectionString);
            await con.OpenAsync();

            await using var tran = await con.BeginTransactionAsync();

            try
            {
                foreach (var t in teachers)
                {
                    await using var cmd = new SqlCommand(@"
                        INSERT INTO Teachers
                        (Name, FirstName, LastName, Email, Subject)
                        VALUES
                        (@name, @fn, @ln, @em, @sub)",
                        con, (SqlTransaction)tran);

                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value =
                        $"{t.FirstName} {t.LastName}";
                    cmd.Parameters.Add("@fn", SqlDbType.NVarChar).Value = t.FirstName;
                    cmd.Parameters.Add("@ln", SqlDbType.NVarChar).Value = t.LastName;
                    cmd.Parameters.Add("@em", SqlDbType.NVarChar).Value = t.Email;
                    cmd.Parameters.Add("@sub", SqlDbType.NVarChar).Value = t.Subject;

                    await cmd.ExecuteNonQueryAsync();
                }

                await tran.CommitAsync();
            }
            catch
            {
                await tran.RollbackAsync();
                throw;
            }
        }
    }
}
