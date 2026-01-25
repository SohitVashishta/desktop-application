using SchoolManagementSystem.Business;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.UI.UI.Helpers;
using System.Threading.Tasks;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly IAttendanceService _attendanceService;

        private int _totalStudents;
        private int _totalTeachers;
        private string _todayAttendancePercentage = "0%";

        public int TotalStudents
        {
            get => _totalStudents;
            private set
            {
                _totalStudents = value;
                OnPropertyChanged();
            }
        }

        public int TotalTeachers
        {
            get => _totalTeachers;
            private set
            {
                _totalTeachers = value;
                OnPropertyChanged();
            }
        }

        public string TodayAttendancePercentage
        {
            get => _todayAttendancePercentage;
            private set
            {
                _todayAttendancePercentage = value;
                OnPropertyChanged();
            }
        }

        // ✅ Proper DI constructor
        public DashboardViewModel(
            IStudentService studentService,
            ITeacherService teacherService,
            IAttendanceService attendanceService)
        {
            _studentService = studentService;
            _teacherService = teacherService;
            _attendanceService = attendanceService;

            _ = LoadDashboardAsync();
        }

        private async Task LoadDashboardAsync()
        {
            var students = await _studentService.GetAllAsync();
            var attendance = await _attendanceService.GetAttendanceByDateAsync(DateTime.Now.Date).ConfigureAwait(false) ?? new();

            TotalStudents = students.Count;
            TotalTeachers = _teacherService.GetTeachersAsync().Result.Count;
            TodayAttendancePercentage = $"{attendance.Count}%";
        }
    }
}
