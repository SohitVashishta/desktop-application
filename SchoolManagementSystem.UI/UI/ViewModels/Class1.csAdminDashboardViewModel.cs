using SchoolManagementSystem.Business.Services;

namespace SchoolManagementSystem.UI.UI.ViewModels.Dashboard
{
    public class AdminDashboardViewModel
    {
        private readonly StudentService _studentService = new();
        private readonly TeacherService _teacherService = new();
        private readonly AttendanceService _attendanceService = new();
        private readonly FinanceService _financeService = new();

        public int TotalStudents { get; }
        public int TotalTeachers { get; }
        public int TodayAttendancePercentage { get; }
        public decimal PendingFeesAmount { get; }

        public AdminDashboardViewModel()
        {
            TotalStudents = _studentService.GetStudents().Count;
            TotalTeachers = _teacherService.GetTeachers().Count;
            TodayAttendancePercentage = _attendanceService.GetTodayAttendancePercentage();
            PendingFeesAmount = _financeService.GetPendingFeesAmount();
        }
    }
}
