using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace SchoolManagementSystem.Data.Repositories
{
    public class StudentRepository
    {
        private readonly string _conn =
            "Server=.;Database=SchoolDB;Trusted_Connection=True;TrustServerCertificate=True";

        public List<Student> GetAll()
        {
            using var context = new SchoolDbContext();
            return context.Students.ToList();
        }

        public void Add(Student student)
        {
            using var context = new SchoolDbContext();
            context.Students.Add(student);
            context.SaveChanges();
        }

        public void Update(Student student)
        {
            using var context = new SchoolDbContext();
            context.Students.Update(student);
            context.SaveChanges();
        }

        // ✅ ADD THIS
        public void Delete(int studentId)
        {
            using var context = new SchoolDbContext();
            var student = context.Students.FirstOrDefault(s => s.StudentId == studentId);
            if (student != null)
            {
                context.Students.Remove(student);
                context.SaveChanges();
            }
        }
        public void InsertSingleStudent(Student s)
        {
            using var con = new SqlConnection(_conn);
            con.Open();

            var cmd = new SqlCommand(@"
        INSERT INTO Students
        (FirstName, LastName, Email, DateOfBirth, EnrollmentDate)
        VALUES (@fn, @ln, @em, @dob, @ed)", con);

            cmd.Parameters.AddWithValue("@fn", s.FirstName);
            cmd.Parameters.AddWithValue("@ln", s.LastName);
            cmd.Parameters.AddWithValue("@em", s.Email);
            cmd.Parameters.AddWithValue("@dob", s.DateOfBirth);
            cmd.Parameters.AddWithValue("@ed", s.EnrollmentDate);

            cmd.ExecuteNonQuery();
        }

    }
}
