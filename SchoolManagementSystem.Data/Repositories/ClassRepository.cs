using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class ClassRepository : IClassRepository
    {
        public async Task<List<ClassModel>> GetAllAsync()
        {
            var list = new List<ClassModel>();

            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Class_GetAll", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            await ((SqlConnection)con).OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new ClassModel
                {
                    ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                    ClassName = reader.GetString(reader.GetOrdinal("ClassName")),
                    GradeName = reader.GetString(reader.GetOrdinal("GradeName")),
                    Section = reader.GetString(reader.GetOrdinal("Section")),
                    LeadTeacherName = reader.GetString(reader.GetOrdinal("TeacherName")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    
                });
            }

            return list;
        }

        public async Task AddAsync(ClassModel model)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Class_Add", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ClassName", model.ClassName);
            cmd.Parameters.AddWithValue("@GradeId", model.GradeId);
            cmd.Parameters.AddWithValue("@Section", model.Section);
            cmd.Parameters.AddWithValue("@LeadTeacherId", model.LeadTeacherId);
            //.Parameters.AddWithValue("@AssistantTeacherId", model.AssistantTeacherId);
            //.Parameters.AddWithValue("@CreatedBy", model.CreatedBy);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(ClassModel model)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Class_Update", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ClassId", model.ClassId);
            cmd.Parameters.AddWithValue("@ClassName", model.ClassName);
            cmd.Parameters.AddWithValue("@GradeId", model.GradeId);
            cmd.Parameters.AddWithValue("@Section", model.Section);
            cmd.Parameters.AddWithValue("@RoomNumber", model.RoomNumber);
            cmd.Parameters.AddWithValue("@MaxStudents", model.MaxStudents);
            cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
            cmd.Parameters.Add("@RowVersion", SqlDbType.Timestamp).Value = model.RowVersion;

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task ToggleStatusAsync(int classId)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Class_ToggleStatus", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ClassId", classId);

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
