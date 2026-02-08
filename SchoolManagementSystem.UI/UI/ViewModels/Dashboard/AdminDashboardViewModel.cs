using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.Dashboard
{
    public class AdminDashboardViewModel : INotifyPropertyChanged
    {
        private readonly IAdminDashboardService _service;
        private readonly IFeeService _feeService;

        public event PropertyChangedEventHandler? PropertyChanged;


        /* ================= KPI COUNTS ================= */

        private int _totalUsers;
        public int TotalUsers
        {
            get => _totalUsers;
            set { _totalUsers = value; OnPropertyChanged(); }
        }

        private int _totalStudents;
        public int TotalStudents
        {
            get => _totalStudents;
            set { _totalStudents = value; OnPropertyChanged(); }
        }

        private int _totalTeachers;
        public int TotalTeachers
        {
            get => _totalTeachers;
            set { _totalTeachers = value; OnPropertyChanged(); }
        }

        private int _totalClasses;
        public int TotalClasses
        {
            get => _totalClasses;
            set { _totalClasses = value; OnPropertyChanged(); }
        }
        private int _totalFees;
        public int TotalFees
        {
            get => _totalFees;
            set { _totalFees = value; OnPropertyChanged(); }
        }
        private int _totalPendingFees;
        public int TotalPendingFees
        {
            get => _totalPendingFees;
            set { _totalPendingFees = value; OnPropertyChanged(); }
        }

        private int _upcomingExam;
        public int UpcomingExams
        {
            get => _upcomingExam;
            set { _upcomingExam = value; OnPropertyChanged(); }
        }
        private int _totalStaff;
        public int TotalStaff
        {
            get => _totalStaff;
            set { _totalStaff = value; OnPropertyChanged(); }
        }
        /* ================= FEES KPI ================= */

        private FeesDashboardKpiModel _kpi;
        public FeesDashboardKpiModel Kpi
        {
            get => _kpi;
            set { _kpi = value; OnPropertyChanged(); }
        }
        /* ================= COLLECTIONS ================= */

        public ObservableCollection<string> RecentActivities { get; } = new();
        public ObservableCollection<string> Announcements { get; } = new();

        public ObservableCollection<MonthlyCollectionModel> MonthlyCollections { get; }
            = new();

        /* ================= SYSTEM HEALTH ================= */

        private string _databaseStatus;
        public string DatabaseStatus
        {
            get => _databaseStatus;
            set { _databaseStatus = value; OnPropertyChanged(); }
        }

        private string _serverStatus;
        public string ServerStatus
        {
            get => _serverStatus;
            set { _serverStatus = value; OnPropertyChanged(); }
        }

        private string _lastBackupDate;
        public string LastBackupDate
        {
            get => _lastBackupDate;
            set { _lastBackupDate = value; OnPropertyChanged(); }
        }


     
        

        public ICommand LoadDashboardCommand { get; }
        public AdminDashboardViewModel(IAdminDashboardService service, IFeeService feeService)
        {
            _service = service;
            _feeService = feeService;
            RefreshDashboardCommand = new RelayCommand(async () => await LoadAsync());

            // 🔥 Auto-load dashboard
            _ = LoadAsync();

        }
        /* ================= COMMANDS ================= */

        public ICommand RefreshDashboardCommand { get; }

        private async Task LoadAsync()
        {
            // -------- DASHBOARD SUMMARY --------
            var dashboard = await _service.GetDashboardAsync();

            TotalUsers = dashboard.TotalUsers;
            TotalStudents = dashboard.TotalStudents;
            TotalTeachers = dashboard.TotalTeachers;
            TotalClasses = dashboard.TotalClasses;
            TotalFees = dashboard.TotalFees;
            TotalPendingFees = dashboard.TotalPendingFees;
            UpcomingExams = dashboard.UpcomingExams;
            TotalStaff = dashboard.TotalStaff;

            DatabaseStatus = dashboard.DatabaseStatus;
            ServerStatus = dashboard.ServerStatus;
            LastBackupDate = dashboard.LastBackupDate;

            RecentActivities.Clear();
            foreach (var item in dashboard.RecentActivities)
                RecentActivities.Add(item);

            Announcements.Clear();
            foreach (var item in dashboard.Announcements)
                Announcements.Add(item);

            // -------- FEES KPI --------
            Kpi = await _feeService.GetKpiAsync();

            // -------- MONTHLY COLLECTION --------
            MonthlyCollections.Clear();
            var monthly = await _feeService.GetMonthlyAsync();
            foreach (var item in monthly)
                MonthlyCollections.Add(item);
        }
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        private void RaiseAll()
        {
            OnPropertyChanged(nameof(TotalUsers));
            OnPropertyChanged(nameof(TotalStudents));
            OnPropertyChanged(nameof(TotalTeachers));
            OnPropertyChanged(nameof(TotalClasses));
            OnPropertyChanged(nameof(DatabaseStatus));
            OnPropertyChanged(nameof(ServerStatus));
            OnPropertyChanged(nameof(LastBackupDate));
        }

      



        
    }
}
