using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        public async Task<List<GradeModel>> GetActiveGradesAsync()
        {
            var list = new List<GradeModel>();

            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand(
                "SELECT GradeId, GradeName FROM GradeMaster WHERE IsActive = 1",
                (SqlConnection)con);

            await ((SqlConnection)con).OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new GradeModel
                {
                    GradeId = reader.GetInt32(0),
                    GradeName = reader.GetString(1)
                });
            }

            return list;
        }
    }
}
