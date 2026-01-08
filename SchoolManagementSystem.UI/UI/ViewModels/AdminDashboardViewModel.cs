using SchoolManagementSystem.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class AdminDashboardViewModel
    {
        // =========================
        // SERVICES (same pattern)
        // =========================
        private readonly StudentService _studentService = new();
        private readonly TeacherService _teacherService = new();
        private readonly AttendanceService _attendanceService = new();
        private readonly FinanceService _financeService = new();

        // =========================
        // DASHBOARD PROPERTIES
        // =========================
        public int TotalStudents { get; }
        public int TotalTeachers { get; }
        public int TodayAttendancePercentage { get; }
        public decimal PendingFeesAmount { get; }

        // =========================
        // CONSTRUCTOR
        // =========================
        public AdminDashboardViewModel()
        {
            // Simple, synchronous, same style as rest of your app
            TotalStudents = _studentService.GetStudents().Count;
            TotalTeachers = _teacherService.GetTeachers().Count;
            TodayAttendancePercentage = _attendanceService.GetTodayAttendancePercentage();
            PendingFeesAmount = _financeService.GetPendingFeesAmount();
        }
    }
}
