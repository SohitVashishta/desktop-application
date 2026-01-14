using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business
{
    public interface IAttendanceService
    {
        Task<List<Attendance>> GetAttendanceByDateAsync(DateTime date);

        Task MarkAttendanceAsync(int studentId, DateTime date, bool isPresent);

        Task MarkBulkAttendanceAsync(DateTime date, List<Attendance> records);

        Task<int> GetAttendancePercentageAsync(DateTime date);
    }
}
