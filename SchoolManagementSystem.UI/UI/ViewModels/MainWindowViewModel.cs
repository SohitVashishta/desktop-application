using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _pageTitle = "Dashboard";
        private string _username = "Admin";

        // ===== HEADER =====
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

        // ===== ROLE FLAGS (USED BY SIDEBAR) =====
        public bool IsAdmin { get; set; }
        public bool IsTeacher { get; set; }
        public bool IsStudent { get; set; }
        public bool IsParent { get; set; }
        public bool IsAccountant { get; set; }
        public bool IsLibrarian { get; set; }
        public bool IsHR { get; set; }

        public MainWindowViewModel()
        {
            // TEMPORARY – replace with UserSession later
            IsAdmin = true;
            IsTeacher = false;
            IsStudent = false;
            IsParent = false;
            IsAccountant = false;
            IsLibrarian = false;
            IsHR = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
