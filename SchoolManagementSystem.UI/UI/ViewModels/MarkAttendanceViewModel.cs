using SchoolManagementSystem.Business;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.Attendances
{
    public class MarkAttendanceViewModel : BaseViewModel
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IStudentService _studentService;

        private DateTime _selectedDate = DateTime.Today;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<StudentAttendanceViewModel> _students;
        public ObservableCollection<StudentAttendanceViewModel> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; }

        // ✅ Constructor Injection
        public MarkAttendanceViewModel(
            IAttendanceService attendanceService,
            IStudentService studentService)
        {
            _attendanceService = attendanceService;
            _studentService = studentService;

            Students = new ObservableCollection<StudentAttendanceViewModel>();

            SaveCommand = new RelayCommand(async () => await SaveAsync());

            // Load students asynchronously
            _ = LoadStudentsAsync();
        }

        // =========================================
        // LOAD STUDENTS
        // =========================================
        private async Task LoadStudentsAsync()
        {
            var students = await _studentService.GetAllAsync();

            Students.Clear();

            foreach (var s in students)
            {
                Students.Add(new StudentAttendanceViewModel
                {
                    StudentId = s.StudentId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    IsPresent = true // default present
                });
            }
        }

        // =========================================
        // SAVE ATTENDANCE (BULK STANDARD)
        // =========================================
        private async Task SaveAsync()
        {
            var records = Students.Select(s => new Attendance
            {
                StudentId = s.StudentId,
                AttendanceDate = SelectedDate.Date,
                IsPresent = s.IsPresent
            }).ToList();

           
        }
    }
}
