using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace SchoolManagementSystem.Data.Repositories
{
    public class AdminDashboardRepository : IAdminDashboardRepository
    {
        public async Task<AdminDashboardStats> GetDashboardStatsAsync()
        {
            var result = new AdminDashboardStats();

            using var con = (SqlConnection)DbConnectionFactory.Create();
            using var cmd = new SqlCommand("dbo.usp_AdminDashboard_GetStats", con);
            cmd.CommandType = CommandType.StoredProcedure;

            await con.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            // ===== RESULT SET 1 : KPI COUNTS =====
            if (await reader.ReadAsync())
            {
                result.TotalUsers = reader.GetInt32(0);
                result.TotalStudents = reader.GetInt32(1);
                result.TotalTeachers = reader.GetInt32(2);
                result.TotalClasses = reader.GetInt32(3);
                result.TotalFees = reader.GetInt32(4);
            }

            // ===== RESULT SET 2 : RECENT ACTIVITIES =====
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                    result.RecentActivities.Add(reader.GetString(0));
            }

            // ===== RESULT SET 3 : ANNOUNCEMENTS =====
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                    result.Announcements.Add(reader.GetString(0));
            }

            result.DatabaseStatus = "Connected";
            result.ServerStatus = "Running";
            result.LastBackupDate = DateTime.Now.ToString("dd MMM yyyy");

            return result;
        }


    }
}
