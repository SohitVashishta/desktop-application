using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using SchoolManagementSystem.UI.UI.Helpers;

namespace SchoolManagementSystem.UI.UI.ViewModels.Dashboard
{
    public class TeacherDashboardViewModel
    {
        // KPI
        public int TotalClasses { get; set; } = 5;
        public int TotalStudents { get; set; } = 120;
        public string TodayAttendancePercent { get; set; } = "92%";
        public int PendingTasks { get; set; } = 3;

        // Lists
        public ObservableCollection<string> TodaySchedule { get; set; }
        public ObservableCollection<string> Announcements { get; set; }

        // Commands
        public ICommand MarkAttendanceCommand { get; }
        public ICommand ManageClassesCommand { get; }
        public ICommand AssignmentsCommand { get; }
        public ICommand ReportsCommand { get; }

        public TeacherDashboardViewModel()
        {
            TodaySchedule = new ObservableCollection<string>
            {
                "09:00 - Class 10A (Math)",
                "11:00 - Class 9B (Math)",
                "02:00 - Class 8C (Math)"
            };

            Announcements = new ObservableCollection<string>
            {
                "Staff meeting at 4 PM",
                "Exam schedule updated",
                "Submit grades by Friday"
            };

            MarkAttendanceCommand = new RelayCommand(() =>
                MessageBox.Show("Navigate to Attendance"));

            ManageClassesCommand = new RelayCommand(() =>
                MessageBox.Show("Navigate to Class Management"));

            AssignmentsCommand = new RelayCommand(() =>
                MessageBox.Show("Navigate to Assignments"));

            ReportsCommand = new RelayCommand(() =>
                MessageBox.Show("Navigate to Reports"));
        }
    }
}
