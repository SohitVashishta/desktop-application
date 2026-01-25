using MaterialDesignThemes.Wpf;
using SchoolManagementSystem.Business;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.StudentManagement.Academic
{
    public class ClassMasterViewModel : BaseViewModel
    {
        private readonly IClassService _classService;
        private readonly IGradeService _gradeService;
        private readonly ITeacherService _teacherService;
        private int _filteredItemCount;

        /* ================= DATA ================= */

        public ObservableCollection<ClassModel> Classes { get; } = new();
        public ObservableCollection<ClassModel> FilteredClasses { get; } = new();

        public ObservableCollection<GradeModel> Grades { get; } = new();
        public ISnackbarMessageQueue SnackbarMessageQueue { get; }
    = new SnackbarMessageQueue();

        /* ================= FILTERS ================= */

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                _pageIndex = 1;      // 🔥 RESET PAGE
                ApplyFilters();
            }
        }

        private GradeModel _selectedGrade;
        public GradeModel SelectedGrade
        {
            get => _selectedGrade;
            set
            {
                _selectedGrade = value;
                OnPropertyChanged();
                _pageIndex = 1;      // 🔥 RESET PAGE
                ApplyFilters();
            }
        }

        /* ================= DIALOG ================= */

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set { _isDialogOpen = value; OnPropertyChanged(); }
        }

        private AddEditClassDialogViewModel _dialogVM;
        public AddEditClassDialogViewModel DialogVM
        {
            get => _dialogVM;
            set { _dialogVM = value; OnPropertyChanged(); }
        }

        /* ================= COMMANDS ================= */

        public ICommand AddClassCommand { get; }
        public ICommand EditClassCommand { get; }
        public ICommand ResetFiltersCommand { get; }
        public ICommand CloseDialogCommand { get; }
        public ICommand SaveDialogCommand { get; }

        /* ================= PAGINATION (OPTIONAL) ================= */

        private int _pageIndex = 1;
        private const int PageSize = 7;

        public string PageInfo =>
            $"Page {_pageIndex} of {TotalPages}";

        public bool CanGoPrevious => _pageIndex > 1;
        public bool CanGoNext => _pageIndex < TotalPages;

        private int TotalPages =>
    Math.Max(1, (_filteredItemCount + PageSize - 1) / PageSize);

        public ICommand PrevPageCommand { get; }
        public ICommand NextPageCommand { get; }

        /* ================= CONSTRUCTOR ================= */

        public ClassMasterViewModel(
     IClassService classService,
     IGradeService gradeService,
     ITeacherService teacherService)
        {
            _classService = classService;
            _gradeService = gradeService;
            _teacherService = teacherService;

            AddClassCommand = new RelayCommand(OpenAddDialog);
            EditClassCommand = new RelayCommand<ClassModel>(OpenEditDialog);
            ResetFiltersCommand = new RelayCommand(ResetFilters);

            CloseDialogCommand = new RelayCommand(() => IsDialogOpen = false);
            SaveDialogCommand = new RelayCommand(async () => await SaveAsync());

              // 🔥 MISSING COMMAND BINDINGS (THIS IS THE BUG)
    PrevPageCommand = new RelayCommand(PrevPage);
    NextPageCommand = new RelayCommand(NextPage);
            // 🔥 THIS LINE IS REQUIRED
            _ = LoadAsync();
        }


        /* ================= LOAD ================= */

        private async Task LoadAsync()
        {
            var grades = await _gradeService.GetActiveGradesAsync();
            Grades.Clear();
            foreach (var g in grades)
                Grades.Add(g);

            var classes = await _classService.GetClassesAsync();
            Classes.Clear();
            foreach (var c in classes)
                Classes.Add(c);

            ApplyFilters();
        }

        /* ================= FILTERING ================= */

        private void ApplyFilters()
        {
            FilteredClasses.Clear();

            var filteredQuery = Classes.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                filteredQuery = filteredQuery.Where(x =>
                    x.ClassName?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true ||
                    x.Section?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) == true);
            }

            if (SelectedGrade != null)
            {
                filteredQuery = filteredQuery.Where(x => x.GradeId == SelectedGrade.GradeId);
            }

            // 🔥 STORE FILTERED COUNT (CRITICAL)
            _filteredItemCount = filteredQuery.Count();

            // 🔥 AUTO-FIX PAGE INDEX
            if (_pageIndex > TotalPages)
                _pageIndex = TotalPages;

            if (_pageIndex < 1)
                _pageIndex = 1;

            // 🔥 APPLY PAGINATION
            var pagedQuery = filteredQuery
                .Skip((_pageIndex - 1) * PageSize)
                .Take(PageSize);

            foreach (var item in pagedQuery)
                FilteredClasses.Add(item);

            OnPropertyChanged(nameof(PageInfo));
            OnPropertyChanged(nameof(CanGoNext));
            OnPropertyChanged(nameof(CanGoPrevious));
        }





        private void ResetFilters()
        {
            _pageIndex = 1;   // 🔥 REQUIRED
            SearchText = string.Empty;
            SelectedGrade = null;
            ApplyFilters();
        }


        /* ================= PAGINATION ================= */

        private void PrevPage()
        {
            if (!CanGoPrevious) return;
            _pageIndex--;
            ApplyFilters();
        }

        private void NextPage()
        {
            if (!CanGoNext) return;
            _pageIndex++;
            ApplyFilters();
        }


        /* ================= DIALOG ================= */

        private async void OpenAddDialog()
        {
            DialogVM = new AddEditClassDialogViewModel(_gradeService, _teacherService);
            IsDialogOpen = true;
            await DialogVM.LoadDropdownsAsync();
        }

        private async void OpenEditDialog(ClassModel model)
        {
            DialogVM = new AddEditClassDialogViewModel(_gradeService, _teacherService, model);
            IsDialogOpen = true;
            await DialogVM.LoadDropdownsAsync();
        }

        private async Task SaveAsync()
        {
            if (!DialogVM.CanSave)
                return;

            var model = DialogVM.BuildModel();

            if (model.ClassId == 0)
            {
                await _classService.AddClassAsync(model);
                SnackbarMessageQueue.Enqueue("Class added successfully");
            }
            else
            {
                await _classService.UpdateClassAsync(model);
                SnackbarMessageQueue.Enqueue("Class updated successfully");
            }

            await LoadAsync();
            IsDialogOpen = false;
        }

    }
}
