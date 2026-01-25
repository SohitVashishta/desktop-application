using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Models.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class AcademicYearRepository : IAcademicYearRepository
    {
        public async Task<List<AcademicYearModel>> GetAllAsync()
        {
            var list = new List<AcademicYearModel>();

            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_AcademicYear_GetAll", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            await ((SqlConnection)con).OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new AcademicYearModel
                {
                    AcademicYearId = reader.GetInt32(reader.GetOrdinal("AcademicYearId")),
                    YearName = reader.GetString(reader.GetOrdinal("YearName")),
                    StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                    EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                    IsCurrent = reader.GetBoolean(reader.GetOrdinal("IsCurrent")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                });
            }

            return list;
        }

        public async Task AddAsync(AcademicYearModel model)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_AcademicYear_Add", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@YearName", model.YearName);
            cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
            cmd.Parameters.AddWithValue("@IsCurrent", model.IsCurrent);
            cmd.Parameters.AddWithValue("@CreatedBy", "Admin");

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(AcademicYearModel model)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_AcademicYear_Update", (SqlConnection)con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@AcademicYearId", model.AcademicYearId);
            cmd.Parameters.AddWithValue("@YearName", model.YearName);
            cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
            cmd.Parameters.AddWithValue("@IsCurrent", model.IsCurrent);
            cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
            cmd.Parameters.AddWithValue("@UpdatedBy", "Admin");

            await ((SqlConnection)con).OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
