using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Data.Repositories
{
    public class ImportRepository
    {
        private readonly string _conn =
            "Server=.;Database=SchoolDB;Trusted_Connection=True;TrustServerCertificate=True";

        public void ImportStudents(List<Student> students)
        {
            using var con = new SqlConnection(_conn);
            con.Open();

            foreach (var s in students)
            {
                var cmd = new SqlCommand(@"
                    INSERT INTO Students (FirstName, LastName, Email, DateOfBirth, EnrollmentDate)
                    VALUES (@fn, @ln, @em, @dob, @ed)", con);

                cmd.Parameters.AddWithValue("@fn", s.FirstName);
                cmd.Parameters.AddWithValue("@ln", s.LastName);
                cmd.Parameters.AddWithValue("@em", s.Email);
                cmd.Parameters.AddWithValue("@dob", s.DateOfBirth);
                cmd.Parameters.AddWithValue("@ed", s.EnrollmentDate);

                cmd.ExecuteNonQuery();
            }
        }

        public void ImportTeachers(List<Teacher> teachers)
        {
            using var con = new SqlConnection(_conn);
            con.Open();

            foreach (var t in teachers)
            {
                var cmd = new SqlCommand(@"
                    INSERT INTO Teachers (Name,FirstName, LastName, Email, Subject)
                    VALUES (@fln,@fn, @ln, @em, @sub)", con);
                cmd.Parameters.AddWithValue("@fln", t.FirstName+" "+t.LastName);
                cmd.Parameters.AddWithValue("@fn", t.FirstName);
                cmd.Parameters.AddWithValue("@ln", t.LastName);
                cmd.Parameters.AddWithValue("@em", t.Email);
                cmd.Parameters.AddWithValue("@sub", t.Subject);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
