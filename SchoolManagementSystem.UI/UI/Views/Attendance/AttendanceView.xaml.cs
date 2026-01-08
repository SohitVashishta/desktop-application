using SchoolManagementSystem.UI.UI.ViewModels;
using SchoolManagementSystem.UI.UI.ViewModels.Attendances;
using System.Windows.Controls;

namespace SchoolManagementSystem.UI.UI.Views.Attendance
{
    public partial class AttendanceView : UserControl
    {
        public AttendanceView()
        {
            InitializeComponent();
            DataContext = new MarkAttendanceViewModel();
        }
    }
}
