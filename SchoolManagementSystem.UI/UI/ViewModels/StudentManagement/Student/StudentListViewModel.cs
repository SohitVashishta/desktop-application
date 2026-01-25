using MaterialDesignThemes.Wpf;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using SchoolManagementSystem.UI.UI.ViewModels.StudentManagement.Student;
using SchoolManagementSystem.UI.UI.Views.StudentManagement.Students;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.StudentManagement
{
    public class StudentListViewModel : BaseViewModel
    {
        private readonly IStudentService _studentService;
        private readonly IClassService _classService;
        private readonly ISectionService _sectionService;
        private readonly IAcademicYearService _academicYearService;

        /* ================= DATA ================= */

        public ObservableCollection<StudentModel> Students { get; } = new();
        public ObservableCollection<StudentModel> FilteredStudents { get; } = new();

        public ObservableCollection<AcademicYearModel> AcademicYears { get; } = new();
        public ObservableCollection<ClassModel> Classes { get; } = new();
        public ObservableCollection<SectionModel> Sections { get; } = new();

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
                _pageIndex = 1;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        private AcademicYearModel _selectedAcademicYear;
        public AcademicYearModel SelectedAcademicYear
        {
            get => _selectedAcademicYear;
            set
            {
                _selectedAcademicYear = value;
                _pageIndex = 1;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        private ClassModel _selectedClass;
        public ClassModel SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                _pageIndex = 1;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        private SectionModel _selectedSection;
        public SectionModel SelectedSection
        {
            get => _selectedSection;
            set
            {
                _selectedSection = value;
                _pageIndex = 1;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        /* ================= PAGINATION ================= */

        private int _pageIndex = 1;
        private const int PageSize = 10;
        private int _totalCount;

        public string PageInfo => $"Page {_pageIndex} of {TotalPages}";
        private int TotalPages => _totalCount == 0 ? 1 : (_totalCount + PageSize - 1) / PageSize;

        public bool CanGoPrevious => _pageIndex > 1;
        public bool CanGoNext => _pageIndex < TotalPages;

        public ICommand PrevPageCommand { get; }
        public ICommand NextPageCommand { get; }

        /* ================= COMMANDS ================= */

        public ICommand AddStudentCommand { get; }
        public ICommand BulkUploadCommand { get; }
        public ICommand ViewProfileCommand { get; }
        public ICommand EditStudentCommand { get; }

        /* ================= CONSTRUCTOR ================= */

        public StudentListViewModel(
            IStudentService studentService,
            IClassService classService,
            ISectionService sectionService,
            IAcademicYearService academicYearService)
        {
            _studentService = studentService;
            _classService = classService;
            _sectionService = sectionService;
            _academicYearService = academicYearService;

            PrevPageCommand = new RelayCommand(PrevPage);
            NextPageCommand = new RelayCommand(NextPage);
            AddStudentCommand = new RelayCommand(OpenAddStudentDialog);
            BulkUploadCommand = new RelayCommand(OpenBulkDialog);

            ViewProfileCommand = new RelayCommand<StudentModel>(student =>
            {
                DialogHost.Show(
                    new ViewStudentProfileDialog { DataContext = student },
                    "RootDialog");
            });

            EditStudentCommand = new RelayCommand<StudentModel>(student =>
            {
                DialogHost.Show(
                    new EditStudentDialog
                    {
                        DataContext = new EditStudentViewModel(_studentService, student)
                    },
                    "RootDialog");
            });

            _ = LoadAsync();
        }

        /* ================= LOAD ================= */

        private async Task LoadAsync()
        {
            AcademicYears.Clear();
            foreach (var y in await _academicYearService.GetAllAsync())
                AcademicYears.Add(y);

            SelectedAcademicYear = AcademicYears.FirstOrDefault(x => x.IsCurrent);

            Classes.Clear();
            foreach (var c in await _classService.GetClassesAsync())
                Classes.Add(c);

            Sections.Clear();
            foreach (var s in await _sectionService.GetAllAsync())
                Sections.Add(s);

            Students.Clear();
            foreach (var s in await _studentService.GetAllAsync())
                Students.Add(s);

            ApplyFilters();
        }

        /* ================= FILTER + PAGINATION ================= */

        private void ApplyFilters()
        {
            FilteredStudents.Clear();

            IEnumerable<StudentModel> query = Students;

            if (!string.IsNullOrWhiteSpace(SearchText))
                query = query.Where(x =>
                    x.StudentName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    x.AdmissionNo.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            //if (SelectedAcademicYear != null && SelectedAcademicYear.AcademicYearId > 0)
            // query = query.Where(x => x.AcademicYearId == SelectedAcademicYear.AcademicYearId);
            if (SelectedClass != null && !string.IsNullOrWhiteSpace(SelectedClass.ClassName))
                query = query.Where(x => x.ClassName == SelectedClass.ClassName);

            if (SelectedSection != null && !string.IsNullOrWhiteSpace(SelectedSection.SectionName))
                query = query.Where(x => x.SectionName == SelectedSection.SectionName);


            _totalCount = query.Count();

            if (_totalCount == 0)
            {
                OnPropertyChanged(nameof(PageInfo));
                OnPropertyChanged(nameof(CanGoPrevious));
                OnPropertyChanged(nameof(CanGoNext));
                return;
            }

            var pageData = query
                .Skip((_pageIndex - 1) * PageSize)
                .Take(PageSize);

            foreach (var s in pageData)
                FilteredStudents.Add(s);

            OnPropertyChanged(nameof(PageInfo));
            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
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

        private void OpenAddStudentDialog()
        {
            DialogHost.Show(
                new AddStudentDialog
                {
                    DataContext = new AddStudentViewModel(
                        _studentService,
                        _classService,
                        _sectionService)
                },
                "RootDialog");
        }

        private void OpenBulkDialog()
        {
            DialogHost.Show(new BulkStudentUploadDialog(), "RootDialog");
        }
    }
}
