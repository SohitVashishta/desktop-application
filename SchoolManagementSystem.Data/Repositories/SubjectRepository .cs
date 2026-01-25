using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Models.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        public async Task<List<SubjectModel>> GetAllAsync()
        {
            var list = new List<SubjectModel>();

            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_GetSubjects", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            await ((SqlConnection)con).OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new SubjectModel
                {
                    SubjectId = reader.GetInt32(reader.GetOrdinal("SubjectId")),
                    SubjectName = reader.GetString(reader.GetOrdinal("SubjectName")),
                    GradeId = reader.GetInt32(reader.GetOrdinal("GradeId")),
                    GradeName = reader.GetString(reader.GetOrdinal("GradeName")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                });
            }

            return list;
        }

        public async Task AddAsync(SubjectModel model)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_AddSubject", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SubjectName", model.SubjectName);
            cmd.Parameters.AddWithValue("@GradeId", model.GradeId);
            cmd.Parameters.AddWithValue("@CreatedBy", model.CreatedBy ?? "Admin");

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(SubjectModel model)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_UpdateSubject", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SubjectId", model.SubjectId);
            cmd.Parameters.AddWithValue("@SubjectName", model.SubjectName);
            cmd.Parameters.AddWithValue("@GradeId", model.GradeId);
           

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int subjectId)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_DeleteSubject", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SubjectId", subjectId);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
