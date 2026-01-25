using MaterialDesignThemes.Wpf;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.StudentManagement.Academic
{
    public class AcademicYearViewModel : BaseViewModel
    {
        private readonly IAcademicYearService _academicYearService;

        /* ================= DATA ================= */

        public ObservableCollection<AcademicYearModel> AcademicYears { get; } = new();

        public ISnackbarMessageQueue SnackbarMessageQueue { get; }
            = new SnackbarMessageQueue();

        /* ================= DIALOG ================= */

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set { _isDialogOpen = value; OnPropertyChanged(); }
        }

        private AcademicYearModel _editYear;
        public AcademicYearModel EditYear
        {
            get => _editYear;
            set
            {
                _editYear = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DialogTitle));
            }
        }

        public string DialogTitle =>
            EditYear?.AcademicYearId == 0 ? "Add Academic Year" : "Edit Academic Year";

        /* ================= COMMANDS ================= */

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveCommand { get; }

        /* ================= CONSTRUCTOR ================= */

        public AcademicYearViewModel(IAcademicYearService academicYearService)
        {
            _academicYearService = academicYearService;

            AddCommand = new RelayCommand(OpenAddDialog);
            EditCommand = new RelayCommand<AcademicYearModel>(OpenEditDialog);
            SaveCommand = new RelayCommand(async () => await SaveAsync());

            _ = LoadAsync();
        }

        /* ================= LOAD ================= */

        private async Task LoadAsync()
        {
            AcademicYears.Clear();

            foreach (var y in await _academicYearService.GetAllAsync())
                AcademicYears.Add(y);
        }

        /* ================= DIALOG ================= */

        private void OpenAddDialog()
        {
            EditYear = new AcademicYearModel
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1),
                IsActive = true
            };

            IsDialogOpen = true;
        }

        private void OpenEditDialog(AcademicYearModel year)
        {
            if (year == null) return;

            EditYear = new AcademicYearModel
            {
                AcademicYearId = year.AcademicYearId,
                YearName = year.YearName,
                StartDate = year.StartDate,
                EndDate = year.EndDate,
                IsCurrent = year.IsCurrent,
                IsActive = year.IsActive
            };

            IsDialogOpen = true;
        }

        private async Task SaveAsync()
        {
            if (EditYear == null ||
                string.IsNullOrWhiteSpace(EditYear.YearName))
                return;

            if (EditYear.AcademicYearId == 0)
            {
                await _academicYearService.AddAsync(EditYear);
                SnackbarMessageQueue.Enqueue("Academic year added successfully");
            }
            else
            {
                await _academicYearService.UpdateAsync(EditYear);
                SnackbarMessageQueue.Enqueue("Academic year updated successfully");
            }

            IsDialogOpen = false;
            await LoadAsync();
        }
    }
}
