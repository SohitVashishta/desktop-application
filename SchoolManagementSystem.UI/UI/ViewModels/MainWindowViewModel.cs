using SchoolManagementSystem.Common.Session;
using SchoolManagementSystem.UI.UI.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        // ================= PRIVATE FIELDS =================
        private string _pageTitle = "Dashboard";
        private string _username = string.Empty;
        private bool _isSidebarExpanded = true;

        private bool _isAdmin;
        private bool _isTeacher;
        private bool _isStudent;
        private bool _isParent;
        private bool _isAccountant;
        private bool _isLibrarian;
        private bool _isHR;

        // ================= CONSTRUCTOR =================
        public MainWindowViewModel()
        {
            ToggleSidebarCommand = new RelayCommand(ToggleSidebar);
            LoadFromSession();
        }

        // ================= SIDEBAR =================
        public bool IsSidebarExpanded
        {
            get => _isSidebarExpanded;
            set
            {
                if (_isSidebarExpanded == value) return;
                _isSidebarExpanded = value;
                OnPropertyChanged();
            }
        }

        public ICommand ToggleSidebarCommand { get; }

        private void ToggleSidebar()
        {
            IsSidebarExpanded = !IsSidebarExpanded;
        }

        // ================= HEADER =================
        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                if (_pageTitle == value) return;
                _pageTitle = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username == value) return;
                _username = value;
                OnPropertyChanged();
            }
        }

        // ================= ROLES =================
        public bool IsAdmin
        {
            get => _isAdmin;
            private set { _isAdmin = value; OnPropertyChanged(); }
        }

        public bool IsTeacher
        {
            get => _isTeacher;
            private set { _isTeacher = value; OnPropertyChanged(); }
        }

        public bool IsStudent
        {
            get => _isStudent;
            private set { _isStudent = value; OnPropertyChanged(); }
        }

        public bool IsParent
        {
            get => _isParent;
            private set { _isParent = value; OnPropertyChanged(); }
        }

        public bool IsAccountant
        {
            get => _isAccountant;
            private set { _isAccountant = value; OnPropertyChanged(); }
        }

        public bool IsLibrarian
        {
            get => _isLibrarian;
            private set { _isLibrarian = value; OnPropertyChanged(); }
        }

        public bool IsHR
        {
            get => _isHR;
            private set { _isHR = value; OnPropertyChanged(); }
        }

        // ================= SESSION LOAD =================
        private void LoadFromSession()
        {
            Username = UserSession.Username;

            IsAdmin = UserSession.IsAdmin;
            IsTeacher = UserSession.IsTeacher;
            IsStudent = UserSession.IsStudent;
            IsParent = UserSession.IsParent;
            IsAccountant = UserSession.IsAccountant;
            IsLibrarian = UserSession.IsLibrarian;
            IsHR = UserSession.IsHR;
        }

        // ================= NOTIFY =================
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
