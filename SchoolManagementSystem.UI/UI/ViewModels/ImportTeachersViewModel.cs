using Microsoft.Win32;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.Import
{
    public class ImportTeachersViewModel : BaseViewModel
    {
        private readonly IImportService _importService;

        private int _progress;
        private bool _isImporting;

        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }

        public bool IsImporting
        {
            get => _isImporting;
            set
            {
                _isImporting = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested(); // ✅ refresh CanExecute
            }
        }

        public ICommand ImportCommand { get; }

        // ✅ Constructor Injection
        public ImportTeachersViewModel(IImportService importService)
        {
            _importService = importService;

            ImportCommand = new RelayCommand(
                async () => await ImportAsync(),
                () => !IsImporting);
        }

        // =============================
        // IMPORT TEACHERS (ASYNC + PROGRESS)
        // =============================
        private async Task ImportAsync()
        {
            var dlg = new OpenFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx"
            };

            if (dlg.ShowDialog() != true)
                return;

            Progress = 0;
            IsImporting = true;

            var progress = new Progress<int>(p => Progress = p);

            try
            {
                // ✅ Service is async – no Task.Run needed
                await _importService.ImportTeachersWithProgressAsync(
                    dlg.FileName,
                    progress);

                MessageBox.Show("Teachers imported successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Import Failed",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsImporting = false;
            }
        }
    }
}
