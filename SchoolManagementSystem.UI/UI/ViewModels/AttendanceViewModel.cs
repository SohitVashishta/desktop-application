using SchoolManagementSystem.Business;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class AttendanceViewModel
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceViewModel(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        public async Task LoadAttendance(DateTime date)
        {
            var attendance = await _attendanceService.GetAttendanceByDateAsync(date);
            // Bind to UI
        }

        public async Task SaveAttendance(DateTime date, List<Attendance> records)
        {
            await _attendanceService.MarkBulkAttendanceAsync(date, records);
        }
    }

}
