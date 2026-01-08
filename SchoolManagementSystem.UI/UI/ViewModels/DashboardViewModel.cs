using SchoolManagementSystem.Business.Services;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class DashboardViewModel
    {
        private readonly StudentService _studentService = new();
        private readonly TeacherService _teacherService = new();
        private readonly AttendanceService _attendanceService = new();

        public int TotalStudents => _studentService.GetStudents().Count;
        public int TotalTeachers => _teacherService.GetTeachers().Count;

        public string TodayAttendancePercentage =>
            _attendanceService.GetTodayAttendancePercentage() + "%";
    }
}
