using Microsoft.Win32;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.Import
{
    public class ImportStudentsViewModel : BaseViewModel
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
                CommandManager.InvalidateRequerySuggested(); // refresh button state
            }
        }

        public ICommand ImportCommand { get; }

        // ✅ Constructor Injection
        public ImportStudentsViewModel(IImportService importService)
        {
            _importService = importService;

            ImportCommand = new RelayCommand(
                async () => await ImportAsync(),
                () => !IsImporting);
        }

        // =============================
        // IMPORT STUDENTS (ASYNC + PROGRESS)
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
                // ✅ Async service call – no Task.Run
                await _importService.ImportStudentsWithProgressAsync(
                    dlg.FileName,
                    progress);

                MessageBox.Show("Students imported successfully");
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
