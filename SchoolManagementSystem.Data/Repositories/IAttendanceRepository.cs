using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface IAttendanceRepository
    {
        Task<List<Attendance>> GetByDateAsync(DateTime date);
        Task UpsertAsync(int studentId, DateTime date, bool isPresent);
        Task UpsertBulkAsync(DateTime date, List<Attendance> records);
        Task<int> GetAttendancePercentageByDateAsync(DateTime date);
    }
}
