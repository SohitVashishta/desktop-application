using SchoolManagementSystem.Common.Session;
using SchoolManagementSystem.UI.UI.ViewModels;
using SchoolManagementSystem.UI.UI.Views.Admin;
using SchoolManagementSystem.UI.UI.Views.Attendance;
using SchoolManagementSystem.UI.UI.Views.Dashboard;
using SchoolManagementSystem.UI.UI.Views.Perent;
using SchoolManagementSystem.UI.UI.Views.Students;
using SchoolManagementSystem.UI.UI.Views.Teachers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SchoolManagementSystem.UI.UI.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainWindowViewModel();
            DataContext = _vm;   // ✅ THIS IS REQUIRED
            LoadDefaultDashboard();
        }

        /* ================= DASHBOARD ================= */

        private void LoadDefaultDashboard()
        {
            if (UserSession.IsAdmin)
                Navigate(new AdminDashboard(), "Admin Dashboard");
            else if (UserSession.IsTeacher)
                Navigate(new TeacherDashboardView(), "Teacher Dashboard");
            else if (UserSession.IsStudent)
                Navigate(new StudentDashboardView(), "Student Dashboard");
            else if (UserSession.IsParent)
                Navigate(new ParentDashboardView(), "Parent Dashboard");
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
            => LoadDefaultDashboard();

        /* ================= ADMIN ================= */

        private void ManageUsers_Click(object sender, RoutedEventArgs e)
            => Navigate(new ManageUsersPage(), "Manage Users");

        private void AssignRoles_Click(object sender, RoutedEventArgs e)
            => Navigate(new AssignRolesPage(), "Assign Roles & Permissions");

        private void ResetPasswords_Click(object sender, RoutedEventArgs e)
            => Navigate(new ResetPasswordsPage(), "Reset Passwords");

        private void PortalAccess_Click(object sender, RoutedEventArgs e)
            => Navigate(new PortalAccessPage(), "Portal Access Control");

        private void StudentManagement_Click(object sender, RoutedEventArgs e)
            => Navigate(new StudentManagementView(), "Student Management");

        private void AttendanceManagement_Click(object sender, RoutedEventArgs e)
            => Navigate(new AttendanceManagementView(), "Attendance Management");

        private void ExaminationGrading_Click(object sender, RoutedEventArgs e)
            => Navigate(new ExaminationGradingView(), "Exams & Grading");

        private void FeeManagement_Click(object sender, RoutedEventArgs e)
            => Navigate(new FeeManagementView(), "Fee Management");

        /* ================= TEACHER ================= */

        private void Students_Click(object sender, RoutedEventArgs e)
            => Navigate(new TeacherStudentsView(), "My Students");

        private void Attendance_Click(object sender, RoutedEventArgs e)
            => Navigate(new TeacherAttendanceView(), "Mark Attendance");

        private void TeacherMarks_Click(object sender, RoutedEventArgs e)
            => Navigate(new TeacherMarksView(), "Marks Entry");

        /* ================= STUDENT ================= */

        private void StudentProfile_Click(object sender, RoutedEventArgs e)
            => Navigate(new StudentProfileView(), "My Profile");

        private void StudentAttendance_Click(object sender, RoutedEventArgs e)
            => Navigate(new StudentAttendanceView(), "Attendance");

        /* ================= PARENT ================= */

        private void ParentDashboard_Click(object sender, RoutedEventArgs e)
            => Navigate(new ParentDashboardView(), "Child Overview");

        private void ParentFees_Click(object sender, RoutedEventArgs e)
            => Navigate(new ParentFeesView(), "Fees & Payments");

        /* ================= LOGOUT ================= */

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            UserSession.End();
            new LoginView().Show();
            Close();
        }

        /* ================= CORE NAVIGATION ================= */

        private void Navigate(UserControl view, string title)
        {
            MainContent.Content = view;
            SetBreadcrumb(title);
        }

        private void SetBreadcrumb(string text)
        {
            if (DataContext is MainWindowViewModel vm)
                vm.PageTitle = text;
        }

        /* ================= UI HELPERS ================= */

        private void SidebarExpander_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is Expander expanded)
            {
                var parent = VisualTreeHelper.GetParent(expanded);
                while (parent != null && parent is not StackPanel)
                    parent = VisualTreeHelper.GetParent(parent);

                if (parent is StackPanel panel)
                {
                    foreach (var child in panel.Children)
                        if (child is Expander exp && exp != expanded)
                            exp.IsExpanded = false;
                }
            }
        }
    }
}
