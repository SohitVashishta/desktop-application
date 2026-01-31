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

        public int TotalUsers { get; private set; }
        public int TotalStudents { get; private set; }
        public int TotalTeachers { get; private set; }
        public int TotalClasses { get; private set; }

        public ObservableCollection<string> RecentActivities { get; } = new();
        public ObservableCollection<string> Announcements { get; } = new();

        public string DatabaseStatus { get; private set; }
        public string ServerStatus { get; private set; }
        public string LastBackupDate { get; private set; }

        public FeesDashboardKpiModel Kpi { get; private set; }

        public ObservableCollection<MonthlyCollectionModel> MonthlyCollections { get; }
            = new();

        public ICommand LoadDashboardCommand { get; }
        public AdminDashboardViewModel(IAdminDashboardService service, IFeeService feeService)
        {
            _service = service;
            _feeService = feeService;
            LoadDashboardCommand = new RelayCommand(async () =>
            {
                Kpi = await _feeService.GetKpiAsync();
                OnPropertyChanged(nameof(Kpi));

                MonthlyCollections.Clear();
                var data = await _feeService.GetMonthlyAsync();
                foreach (var item in data)
                    MonthlyCollections.Add(item);
            });
            _ = LoadAsync();
            
        }

        private async Task LoadAsync()
        {
            var data = await _service.GetDashboardAsync();

            TotalUsers = data.TotalUsers;
            TotalStudents = data.TotalStudents;
            TotalTeachers = data.TotalTeachers;
            TotalClasses = data.TotalClasses;

            RecentActivities.Clear();
            foreach (var item in data.RecentActivities)
                RecentActivities.Add(item);

            Announcements.Clear();
            foreach (var item in data.Announcements)
                Announcements.Add(item);

            DatabaseStatus = data.DatabaseStatus;
            ServerStatus = data.ServerStatus;
            LastBackupDate = data.LastBackupDate;

            RaiseAll();
        }

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

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));



        
    }
}
