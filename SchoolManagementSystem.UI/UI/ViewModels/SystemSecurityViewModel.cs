using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.Models.Models
{
    public class SystemSecurityViewModel
    {
        private readonly ISystemService _service = new SystemService();

        public ObservableCollection<AuditLogDto> AuditLogs { get; } = new();
        public ObservableCollection<SystemSettingDto> Settings { get; } = new();

        public ICommand LoadCommand { get; }
        public ICommand RunBackupCommand { get; }

        public SystemSecurityViewModel()
        {
            LoadCommand = new RelayCommand(async () => await LoadAsync());
            RunBackupCommand = new RelayCommand(async () => await _service.RunBackupAsync());
        }

        private async Task LoadAsync()
        {
            AuditLogs.Clear();
            Settings.Clear();

            foreach (var log in await _service.GetAuditLogsAsync())
                AuditLogs.Add(log);

            foreach (var s in await _service.GetSettingsAsync())
                Settings.Add(s);
        }
    }
}
