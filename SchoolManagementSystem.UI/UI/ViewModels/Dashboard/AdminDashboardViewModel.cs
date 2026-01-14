using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;   // ✅ IMPORTANT: use MODEL AdminModuleItem
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
        private readonly IAdminModuleService _service;

        public event PropertyChangedEventHandler? PropertyChanged;

        // ✅ Use MODEL type, not a ViewModel duplicate
        public ObservableCollection<AdminModuleItem> AdminModules { get; }
            = new();

        // ✅ Always expose ICommand
        public ICommand OpenModuleCommand { get; }

        public AdminDashboardViewModel()
        {
            _service = new AdminModuleService();

            // ✅ Correct generic command usage
            OpenModuleCommand = new RelayCommand<AdminModuleItem>(OpenModule);

            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            var modules = await _service.GetAdminModulesAsync();

            AdminModules.Clear();
            foreach (var module in modules)
                AdminModules.Add(module);
        }

        private void OpenModule(AdminModuleItem module)
        {
            if (module == null)
                return;

            // 🔁 Navigation handled elsewhere (MainWindow / NavigationService)
            // Example:
            // NavigationService.Navigate(module.ModuleKey);
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
