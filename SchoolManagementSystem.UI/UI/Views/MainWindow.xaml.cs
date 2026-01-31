using SchoolManagementSystem.Common.Session;
using SchoolManagementSystem.UI.UI.ViewModels;
using SchoolManagementSystem.UI.UI.ViewModels.StudentManagement.Academic;
using SchoolManagementSystem.UI.UI.Views.Admin;
using SchoolManagementSystem.UI.UI.Views.Attendance;
using SchoolManagementSystem.UI.UI.Views.Dashboard;
using SchoolManagementSystem.UI.UI.Views.FeeManagement;
using SchoolManagementSystem.UI.UI.Views.Perent;
using SchoolManagementSystem.UI.UI.Views.StudentManagement;
using SchoolManagementSystem.UI.UI.Views.StudentManagement.Academic;
using SchoolManagementSystem.UI.UI.Views.Students;
using SchoolManagementSystem.UI.UI.Views.Teachers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SchoolManagementSystem.UI.UI.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();

            _vm = new MainWindowViewModel();
            DataContext = _vm;

            LoadDefaultDashboard();
        }

        /* ================= SIDEBAR ================= */

        private void ToggleSidebar_Click(object sender, RoutedEventArgs e)
        {
            // Toggle ONLY in ViewModel
            _vm.IsSidebarExpanded = !_vm.IsSidebarExpanded;

            // Animate sidebar width
            var animation = new DoubleAnimation
            {
                To = _vm.IsSidebarExpanded ? 240 : 64,
                Duration = TimeSpan.FromMilliseconds(250),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            //SidebarBorder.BeginAnimation(WidthProperty, animation);
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
            => Navigate(new ControlPortalAccessPage(), "Portal Access Control");

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
        private void ExaminationGrading_Click(object sender, RoutedEventArgs e)
            => Navigate(new ExaminationGradingView(), "Fees & Payments");
        private void FeeManagement_Click(object sender, RoutedEventArgs e)
           => Navigate(new FeeStructurePage(), "Fees & Payments");
        private void FeeDiscount_Click(object sender, RoutedEventArgs e)
            => Navigate(new FeeDiscountPage(), "Fee Discounts");
        private void AddNewFeeEntry_Click(object sender, RoutedEventArgs e)
            => Navigate(new AddNewFeeEntry(), "Add New Fee Entry");
        // Academic Management
        private void OpenAcademicYearPage_Click(object sender, RoutedEventArgs e)
         => Navigate(new AcademicYearMasterPage(), "Academic Year");

        private void OpenClassMasterPage_Click(object sender, RoutedEventArgs e)
         => Navigate(new ClassMasterPage(), "Classes");

        private void OpenSectionPage_Click(object sender, RoutedEventArgs e)
         => Navigate(new SectionMasterPage(), "Sections");

        private void OpenSubjectPage_Click(object sender, RoutedEventArgs e)
         => Navigate(new SubjectMasterPage(), "Subjects");

        private void OpenClassSectionMappingPage_Click(object sender, RoutedEventArgs e)
         => Navigate(new ClassSectionMappingPage(), "Class-Section Mapping");

        private void OpenClassSubjectMappingPage_Click(object sender, RoutedEventArgs e)
         => Navigate(new ClassSubjectMappingPage(), "Class-Subject Mapping");
        private void OpenStudentAdmissionPage_Click(object sender, RoutedEventArgs e)
        => Navigate(new StudentAdmissionPage(), "Student Admission");

        private void OpenStudentProfiles_Click(object sender, RoutedEventArgs e)
        => Navigate(new StudentListPage(), "Student Profiles");

        private void OpenClassSectionAssignment_Click(object sender, RoutedEventArgs e)
        => Navigate(new StudentClassAssignmentPage(), "Class/Section Assignment");

        private void OpenAcademicYearMapping_Click(object sender, RoutedEventArgs e)
        => Navigate(new StudentAcademicYearMappingPage(), "Academic Year Mapping");

        private void OpenEnrollmentTransfers_Click(object sender, RoutedEventArgs e)
        => Navigate(new StudentTransferPage(), "Enrollment &amp; Transfers");

        private void DocumentUploads_Click(object sender, RoutedEventArgs e)
        => Navigate(new StudentDocumentsPage(), "Document Uploads");


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
            _vm.PageTitle = title;
        }
        private void NavigatePage(Page view, string title)
        {
            MainContent.Content = view;
            _vm.PageTitle = title;
        }

        /* ================= UI HELPERS ================= */

        private void SidebarExpander_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is not Expander expanded) return;

            var parent = VisualTreeHelper.GetParent(expanded);
            while (parent != null && parent is not StackPanel)
                parent = VisualTreeHelper.GetParent(parent);

            if (parent is StackPanel panel)
            {
                foreach (var child in panel.Children)
                {
                    if (child is Expander exp && exp != expanded)
                        exp.IsExpanded = false;
                }
            }
        }

    }
}
