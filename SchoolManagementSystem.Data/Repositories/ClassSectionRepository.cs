using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class ClassSectionRepository : IClassSectionRepository
{
    public async Task<List<ClassSectionModel>> GetByClassAsync(int classId, int academicYearId)
    {
        var list = new List<ClassSectionModel>();

        using var con = DbConnectionFactory.Create();
        using var cmd = new SqlCommand("usp_ClassSection_GetByClass", (SqlConnection)con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ClassId", classId);
        cmd.Parameters.AddWithValue("@AcademicYearId", academicYearId);

        await ((SqlConnection)con).OpenAsync();

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new ClassSectionModel
            {
                ClassSectionId = reader.GetInt32(0),
                SectionId = reader.GetInt32(1),
                SectionName = reader.GetString(2),
                IsActive = reader.GetBoolean(3)
            });
        }

        return list;
    }

    public async Task AddAsync(ClassSectionModel model)
    {
        using var con = DbConnectionFactory.Create();
        using var cmd = new SqlCommand("usp_ClassSection_Add", (SqlConnection)con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ClassId", model.ClassId);
        cmd.Parameters.AddWithValue("@SectionId", model.SectionId);
        cmd.Parameters.AddWithValue("@AcademicYearId", model.AcademicYearId);
        cmd.Parameters.AddWithValue("@CreatedBy", "Admin");

        await ((SqlConnection)con).OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task ToggleStatusAsync(int classSectionId)
    {
        using var con = DbConnectionFactory.Create();
        using var cmd = new SqlCommand("usp_ClassSection_ToggleStatus", (SqlConnection)con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@ClassSectionId", classSectionId);
        cmd.Parameters.AddWithValue("@UpdatedBy", "Admin");

        await ((SqlConnection)con).OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}
