using MaterialDesignThemes.Wpf;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.StudentManagement.Academic
{
    public class SubjectMasterViewModel : BaseViewModel
    {
        private readonly ISubjectService _subjectService;
        private readonly IGradeService _gradeService;
        /* ================= PAGINATION ================= */

        private int _pageIndex = 1;
        private const int PageSize = 7;

        public string PageInfo =>
            $"Page {_pageIndex} of {TotalPages}";

        public bool CanGoPrevious => _pageIndex > 1;
        public bool CanGoNext => _pageIndex < TotalPages;

        private int TotalPages =>
            (FilteredCount + PageSize - 1) / PageSize;

        private int FilteredCount;
        public ICommand PrevPageCommand { get; }
        public ICommand NextPageCommand { get; }

        /* ================= DATA ================= */

        public ObservableCollection<SubjectModel> Subjects { get; } = new();
        public ObservableCollection<SubjectModel> FilteredSubjects { get; } = new();
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
                _pageIndex = 1; // 🔥 reset
                OnPropertyChanged();
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
                _pageIndex = 1; // 🔥 reset
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        /* ================= DIALOG ================= */

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set
            {
                _isDialogOpen = value;
                OnPropertyChanged();
            }
        }

        private SubjectModel _editSubject;
        public SubjectModel EditSubject
        {
            get => _editSubject;
            set
            {
                _editSubject = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DialogTitle));
            }
        }

        public string DialogTitle =>
            EditSubject?.SubjectId == 0 ? "Add Subject" : "Edit Subject";

        /* ================= COMMANDS ================= */

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ResetFiltersCommand { get; }

        /* ================= CONSTRUCTOR ================= */

        public SubjectMasterViewModel(
            ISubjectService subjectService,
            IGradeService gradeService)
        {
            _subjectService = subjectService;
            _gradeService = gradeService;

            AddCommand = new RelayCommand(OpenAddDialog);
            EditCommand = new RelayCommand<SubjectModel>(OpenEditDialog);
            DeleteCommand = new RelayCommand<SubjectModel>(DeleteAsync);
            SaveCommand = new RelayCommand(async () => await SaveAsync());
            PrevPageCommand = new RelayCommand(PrevPage);
            NextPageCommand = new RelayCommand(NextPage);

            ResetFiltersCommand = new RelayCommand(ResetFilters);

            _ = LoadAsync();
        }

        /* ================= LOAD ================= */

        private async Task LoadAsync()
        {
            Grades.Clear();
            foreach (var g in await _gradeService.GetActiveGradesAsync())
                Grades.Add(g);

            Subjects.Clear();
            foreach (var s in await _subjectService.GetAllAsync())
                Subjects.Add(s);

            ApplyFilters();
        }

        /* ================= FILTER ================= */

        private void ApplyFilters()
        {
            FilteredSubjects.Clear();

            var query = Subjects.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(x =>
                    x.SubjectName?.Contains(SearchText,
                        System.StringComparison.OrdinalIgnoreCase) == true);
            }

            if (SelectedGrade != null)
            {
                query = query.Where(x => x.GradeId == SelectedGrade.GradeId);
            }

            // 🔥 COUNT AFTER FILTERS
            FilteredCount = query.Count();

            // 🔥 APPLY PAGINATION
            query = query
                .Skip((_pageIndex - 1) * PageSize)
                .Take(PageSize);

            foreach (var s in query)
                FilteredSubjects.Add(s);

            OnPropertyChanged(nameof(PageInfo));
            OnPropertyChanged(nameof(CanGoNext));
            OnPropertyChanged(nameof(CanGoPrevious));
        }


      

        /* ================= DIALOG ================= */

        private void OpenAddDialog()
        {
            EditSubject = new SubjectModel
            {
                IsActive = true
            };

            IsDialogOpen = true;
        }

        private void OpenEditDialog(SubjectModel subject)
        {
            if (subject == null) return;

            EditSubject = new SubjectModel
            {
                SubjectId = subject.SubjectId,
                SubjectName = subject.SubjectName,
                GradeId = subject.GradeId,
                IsActive = subject.IsActive
            };

            IsDialogOpen = true;
        }

        private async Task SaveAsync()
        {
            if (EditSubject == null ||
                string.IsNullOrWhiteSpace(EditSubject.SubjectName) ||
                EditSubject.GradeId <= 0)
                return;

            if (EditSubject.SubjectId == 0)
            {
                await _subjectService.AddAsync(EditSubject);
                SnackbarMessageQueue.Enqueue("Subject added successfully");
            }
            else
            {
                await _subjectService.UpdateAsync(EditSubject);
                SnackbarMessageQueue.Enqueue("Subject updated successfully");
            }

            IsDialogOpen = false;
            await LoadAsync();
        }
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
        private void ResetFilters()
        {
            SearchText = string.Empty;
            SelectedGrade = null;
            _pageIndex = 1;
            ApplyFilters();
        }


        private async void DeleteAsync(SubjectModel subject)
        {
            if (subject == null) return;

            await _subjectService.DeleteAsync(subject.SubjectId);
            SnackbarMessageQueue.Enqueue("Subject deleted");
            await LoadAsync();
        }
    }
}
