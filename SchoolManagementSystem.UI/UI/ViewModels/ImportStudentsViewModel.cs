using Microsoft.Win32;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.Import
{
    public class ImportStudentsViewModel : INotifyPropertyChanged
    {
        private readonly ImportService _service = new();

        private int _progress;
        private bool _isImporting;

        public int Progress
        {
            get => _progress;
            set { _progress = value; OnPropertyChanged(nameof(Progress)); }
        }

        public bool IsImporting
        {
            get => _isImporting;
            set { _isImporting = value; OnPropertyChanged(nameof(IsImporting)); }
        }

        public ICommand ImportCommand { get; }

        public ImportStudentsViewModel()
        {
            ImportCommand = new RelayCommand(async () => await ImportAsync(), () => !IsImporting);
        }

        public async Task ImportAsync()
        {
            var dlg = new OpenFileDialog { Filter = "Excel (*.xlsx)|*.xlsx" };
            if (dlg.ShowDialog() != true) return;

            Progress = 0;
            IsImporting = true;

            var progress = new Progress<int>(p => Progress = p);

            try
            {
                await Task.Run(() =>
                    _service.ImportStudentsWithProgress(dlg.FileName, progress));

                MessageBox.Show("Students imported successfully");
            }
            finally
            {
                IsImporting = false;
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
