using System.Windows;
using System.Windows.Controls;
using SchoolManagementSystem.Common.Session;
using SchoolManagementSystem.UI.UI.Views.Attendance;
using SchoolManagementSystem.UI.UI.Views.Dashboard;
using SchoolManagementSystem.UI.UI.Views.Import;
using SchoolManagementSystem.UI.UI.Views.Students;
using SchoolManagementSystem.UI.UI.Views.Teachers;

namespace SchoolManagementSystem.UI.UI.Views
{
    public partial class MainWindow : Window
    {
        public string Username => UserSession.Username;
        public bool IsAdmin => UserSession.IsAdmin;
        public bool IsTeacherOrAdmin => UserSession.IsTeacher || UserSession.IsAdmin;
        private AdminDashboardView _adminDashboard;
        private TeacherDashboardView _teacherDashboard;
        private StudentDashboardView _studentDashboard;
        // Cached views (lazy loading)
        private DashboardView _dashboard;
        private StudentListView _students;
        private TeacherListView _teachers;
        private AttendanceView _attendance;
        private AttendanceReportView _reports;
        private TeacherClassesView _teacherClassesView;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            LoadDefaultDashboard();
        }

        /* ================= LOADERS ================= */
        private void LoadDefaultDashboard()
        {
            if (UserSession.IsAdmin)
                LoadAdminDashboard();
            else if (UserSession.IsTeacher)
                LoadTeacherDashboard();
            else
                LoadStudentDashboard();
        }
        private void LoadAdminDashboard()
        {
            _adminDashboard ??= new AdminDashboardView();
            MainContent.Content = _adminDashboard;
            Activate(BtnDashboard, "Admin Dashboard");
        }

        private void LoadTeacherDashboard()
        {
            _teacherDashboard ??= new TeacherDashboardView();
            MainContent.Content = _teacherDashboard;
            Activate(BtnDashboard, "Teacher Dashboard");
        }

        private void LoadStudentDashboard()
        {
            _studentDashboard ??= new StudentDashboardView();
            MainContent.Content = _studentDashboard;
            Activate(BtnDashboard, "Student Dashboard");
        }
        private void LoadDashboard()
        {
            _dashboard ??= new DashboardView();
            MainContent.Content = _dashboard;
            Activate(BtnDashboard, "Dashboard");
        }

        private void LoadStudents()
        {
            _students ??= new StudentListView();
            MainContent.Content = _students;
            Activate(BtnStudents, "Students");
        }

        private void LoadTeachers()
        {
            _teachers ??= new TeacherListView();
            MainContent.Content = _teachers;
            Activate(BtnTeachers, "Teachers");
        }

        private void LoadAttendance()
        {
            _attendance ??= new AttendanceView();
            MainContent.Content = _attendance;
            Activate(BtnAttendance, "Attendance");
        }

        private void LoadReports()
        {
            _reports ??= new AttendanceReportView();
            MainContent.Content = _reports;
            Activate(BtnReports, "Reports");
        }

        /* ================= ACTIVE MENU ================= */

        private void Activate(Button btn, string breadcrumb)
        {
            BtnDashboard.Tag =
            BtnStudents.Tag =
            BtnTeachers.Tag =
            BtnAttendance.Tag =
            BtnMyClasses.Tag =
            BtnReports.Tag = null;

            btn.Tag = "Active";
            BreadcrumbText.Text = breadcrumb;
        }

        /* ================= EVENTS ================= */

        private void Dashboard_Click(object sender, RoutedEventArgs e) => LoadDashboard();
        private void Students_Click(object sender, RoutedEventArgs e) => LoadStudents();
        private void Teachers_Click(object sender, RoutedEventArgs e) => LoadTeachers();
        private void Attendance_Click(object sender, RoutedEventArgs e) => LoadAttendance();
        private void Reports_Click(object sender, RoutedEventArgs e) => LoadReports();
        private void MyClasses_Click(object sender, RoutedEventArgs e) => LoadMyClasses();


        private void LoadMyClasses()
        {
            if (!UserSession.IsTeacher)
            {
                MessageBox.Show("Access denied");
                return;
            }

            _teacherClassesView ??= new TeacherClassesView();

            MainContent.Content = _teacherClassesView;

            Activate(BtnMyClasses, "My Classes");
        }

        private void ImportStudents_Click(object sender, RoutedEventArgs e)
            => MainContent.Content = new ImportStudentsView();

        private void ImportTeachers_Click(object sender, RoutedEventArgs e)
            => MainContent.Content = new ImportTeachersView();

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            UserSession.Clear();
            new LoginView().Show();
            Close();
        }
    }
}
