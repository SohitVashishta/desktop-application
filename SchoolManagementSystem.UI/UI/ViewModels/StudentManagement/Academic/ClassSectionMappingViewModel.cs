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
    public class ClassSectionMappingViewModel : BaseViewModel
    {
        private readonly IClassService _classService;
        private readonly ISectionService _sectionService;
        private readonly IAcademicYearService _academicYearService;
        private readonly IClassSectionService _classSectionService;

        /* ================= DATA ================= */

        public ObservableCollection<ClassModel> Classes { get; } = new();
        public ObservableCollection<SectionModel> Sections { get; } = new();
        public ObservableCollection<AcademicYearModel> AcademicYears { get; } = new();
        public ObservableCollection<ClassSectionModel> ClassSections { get; } = new();

        public ISnackbarMessageQueue SnackbarMessageQueue { get; }
            = new SnackbarMessageQueue();

        /* ================= SELECTIONS ================= */

        private ClassModel _selectedClass;
        public ClassModel SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged();
                _ = LoadClassSectionsAsync();
            }
        }

        private AcademicYearModel _selectedAcademicYear;
        public AcademicYearModel SelectedAcademicYear
        {
            get => _selectedAcademicYear;
            set
            {
                _selectedAcademicYear = value;
                OnPropertyChanged();
                _ = LoadClassSectionsAsync();
            }
        }

        private SectionModel _selectedSection;
        public SectionModel SelectedSection
        {
            get => _selectedSection;
            set { _selectedSection = value; OnPropertyChanged(); }
        }

        /* ================= COMMANDS ================= */

        public ICommand AddMappingCommand { get; }
        public ICommand ToggleStatusCommand { get; }

        /* ================= CONSTRUCTOR ================= */

        public ClassSectionMappingViewModel(
            IClassService classService,
            ISectionService sectionService,
            IAcademicYearService academicYearService,
            IClassSectionService classSectionService)
        {
            _classService = classService;
            _sectionService = sectionService;
            _academicYearService = academicYearService;
            _classSectionService = classSectionService;

            AddMappingCommand = new RelayCommand(async () => await AddMappingAsync());
            ToggleStatusCommand = new RelayCommand<ClassSectionModel>(ToggleStatusAsync);

            _ = LoadMastersAsync();
        }

        /* ================= LOAD ================= */

        private async Task LoadMastersAsync()
        {
            Classes.Clear();
            foreach (var c in await _classService.GetClassesAsync())
                Classes.Add(c);

            Sections.Clear();
            foreach (var s in await _sectionService.GetAllAsync())
                Sections.Add(s);

            AcademicYears.Clear();
            foreach (var y in await _academicYearService.GetAllAsync())
                AcademicYears.Add(y);

            SelectedAcademicYear = AcademicYears.FirstOrDefault(x => x.IsCurrent);
        }

        private async Task LoadClassSectionsAsync()
        {
            ClassSections.Clear();

            if (SelectedClass == null || SelectedAcademicYear == null)
                return;

            foreach (var cs in await _classSectionService
                         .GetByClassAsync(SelectedClass.ClassId, SelectedAcademicYear.AcademicYearId))
            {
                ClassSections.Add(cs);
            }
        }

        /* ================= ACTIONS ================= */

        private async Task AddMappingAsync()
        {
            if (SelectedClass == null ||
                SelectedAcademicYear == null ||
                SelectedSection == null)
                return;

            await _classSectionService.AddAsync(new ClassSectionModel
            {
                ClassId = SelectedClass.ClassId,
                SectionId = SelectedSection.SectionId,
                AcademicYearId = SelectedAcademicYear.AcademicYearId
            });

            SnackbarMessageQueue.Enqueue("Section mapped to class successfully");

            SelectedSection = null;
            await LoadClassSectionsAsync();
        }

        private async void ToggleStatusAsync(ClassSectionModel model)
        {
            if (model == null) return;

            await _classSectionService.ToggleStatusAsync(model.ClassSectionId);
            SnackbarMessageQueue.Enqueue("Section status updated");

            await LoadClassSectionsAsync();
        }
    }
}
