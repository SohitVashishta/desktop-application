using SchoolManagementSystem.Common.Session;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _pageTitle = "Dashboard";
        private string _username = "";

        public string PageTitle
        {
            get => _pageTitle;
            set { _pageTitle = value; OnPropertyChanged(); }
        }

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public bool IsAdmin { get; private set; }
        public bool IsTeacher { get; private set; }
        public bool IsStudent { get; private set; }
        public bool IsParent { get; private set; }
        public bool IsAccountant { get; private set; }
        public bool IsLibrarian { get; private set; }
        public bool IsHR { get; private set; }

        public MainWindowViewModel()
        {
            LoadFromSession();
        }

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

            NotifyRoles();
        }

        private void NotifyRoles()
        {
            OnPropertyChanged(nameof(IsAdmin));
            OnPropertyChanged(nameof(IsTeacher));
            OnPropertyChanged(nameof(IsStudent));
            OnPropertyChanged(nameof(IsParent));
            OnPropertyChanged(nameof(IsAccountant));
            OnPropertyChanged(nameof(IsLibrarian));
            OnPropertyChanged(nameof(IsHR));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
