using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Models.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        public async Task<List<SectionModel>> GetAllAsync()
        {
            var list = new List<SectionModel>();

            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Section_GetAll", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            await ((SqlConnection)con).OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new SectionModel
                {
                    SectionId = reader.GetInt32(reader.GetOrdinal("SectionId")),
                    SectionName = reader.GetString(reader.GetOrdinal("SectionName")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                });
            }

            return list;
        }

        public async Task AddAsync(SectionModel model)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Section_Add", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SectionName", model.SectionName);
            cmd.Parameters.AddWithValue("@CreatedBy", "Admin");

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(SectionModel model)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Section_Update", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@SectionId", model.SectionId);
            cmd.Parameters.AddWithValue("@SectionName", model.SectionName);
            cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
            cmd.Parameters.AddWithValue("@UpdatedBy", "Admin");

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
