using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        /* ================= GET ALL ================= */

        public async Task<List<StudentModel>> GetAllAsync()
        {
            var list = new List<StudentModel>();

            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Student_GetAll", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            await ((SqlConnection)con).OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new StudentModel
                {
                    StudentId = reader.GetInt32(reader.GetOrdinal("StudentId")),
                    AdmissionNo = reader.GetString(reader.GetOrdinal("AdmissionNo")),
                    StudentName = reader.GetString(reader.GetOrdinal("StudentName")),
                    Gender = reader.GetString(reader.GetOrdinal("Gender")),
                    ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                    ClassName = reader.GetString(reader.GetOrdinal("ClassName")),
                    SectionId = reader.GetInt32(reader.GetOrdinal("SectionId")),
                    SectionName = reader.GetString(reader.GetOrdinal("SectionName")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                });
            }

            return list;
        }

        /* ================= ADD ================= */

        public async Task<int> AddAsync(StudentModel s)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Student_Add", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@AdmissionNo", s.AdmissionNo);
            cmd.Parameters.AddWithValue("@StudentName", s.StudentName);
            cmd.Parameters.AddWithValue("@Gender", s.Gender);
            cmd.Parameters.AddWithValue("@DateOfBirth", (object?)s.DateOfBirth ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ClassId", s.ClassName);
            cmd.Parameters.AddWithValue("@SectionId", s.SectionName);
            cmd.Parameters.AddWithValue("@AcademicYearId", s.AcademicYearId);
            cmd.Parameters.AddWithValue("@CreatedBy", "System");

            await ((SqlConnection)con).OpenAsync();

            // 🔥 usp_Student_Add MUST RETURN SCOPE_IDENTITY()
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        /* ================= UPDATE ================= */

        public async Task UpdateAsync(StudentModel s)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Student_Update", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentId", s.StudentId);
            cmd.Parameters.AddWithValue("@StudentName", s.StudentName);
            cmd.Parameters.AddWithValue("@Gender", s.Gender);
            cmd.Parameters.AddWithValue("@ClassId", s.ClassId);
            cmd.Parameters.AddWithValue("@SectionId", s.SectionId);
            cmd.Parameters.AddWithValue("@UpdatedBy", "System");

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        /* ================= STATUS ================= */

        public async Task ToggleStatusAsync(int studentId)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Student_ToggleStatus", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentId", studentId);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        /* ================= CLASS HISTORY ================= */

        public async Task InsertClassHistoryAsync(StudentClassHistoryModel h)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Student_ClassHistory_Insert",
                                           (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StudentId", h.StudentId);
            cmd.Parameters.AddWithValue("@AcademicYearId", h.AcademicYearId);
            cmd.Parameters.AddWithValue("@ClassId", h.ClassId);
            cmd.Parameters.AddWithValue("@SectionId", h.SectionId);
            cmd.Parameters.AddWithValue("@IsCurrent", h.IsCurrent);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task MarkHistoryNotCurrentAsync(int studentId)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand(
                @"UPDATE StudentClassHistory
                  SET IsCurrent = 0
                  WHERE StudentId = @StudentId",
                (SqlConnection)con);

            cmd.Parameters.AddWithValue("@StudentId", studentId);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        /* ================= PROMOTION ================= */

        public async Task UpdateClassSectionAsync(int studentId, int classId, int sectionId)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand(
                @"UPDATE StudentMaster
                  SET ClassId = @ClassId,
                      SectionId = @SectionId,
                      UpdatedOn = GETDATE()
                  WHERE StudentId = @StudentId",
                (SqlConnection)con);

            cmd.Parameters.AddWithValue("@StudentId", studentId);
            cmd.Parameters.AddWithValue("@ClassId", classId);
            cmd.Parameters.AddWithValue("@SectionId", sectionId);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
