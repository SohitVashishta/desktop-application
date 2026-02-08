using MaterialDesignThemes.Wpf;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.FeeManagement
{
    public class FeeStructureViewModel : NotifyPropertyChangedBase
    {
        private readonly IFeeService _feeService;
        private readonly IClassService _classService;
        private readonly ISectionService _sectionService;
        private readonly IAcademicYearService _academicYearService;
        private bool _feeHeadsLoaded;

        public FeeStructureViewModel(
            IClassService classService,
            ISectionService sectionService,
            IAcademicYearService academicYearService)
        {
            _feeService = new FeeService();
            _classService = classService;
            _sectionService = sectionService;
            _academicYearService = academicYearService;

            AcademicYears = new ObservableCollection<AcademicYearModel>();
            Classes = new ObservableCollection<ClassModel>();
            Sections = new ObservableCollection<SectionModel>();
            FeeHeadAmounts = new ObservableCollection<FeeHeadAmountVM>();

            FeeTypes = new ObservableCollection<string>
        {
            "Monthly", "Quarterly", "Yearly", "OneTime"
        };

            SaveCommand = new RelayCommand(async () => await SaveAsync(null));

            LoadAcademicYears();
            LoadClasses();
            LoadSections();
            LoadFeeHeads();
        }

        // ================= DROPDOWNS =================
        public ObservableCollection<AcademicYearModel> AcademicYears { get; }
        public ObservableCollection<ClassModel> Classes { get; }
        public ObservableCollection<SectionModel> Sections { get; }

        public ObservableCollection<string> FeeTypes { get; }

        private AcademicYearModel _selectedAcademicYear;
        public AcademicYearModel SelectedAcademicYear
        {
            get => _selectedAcademicYear;
            set { _selectedAcademicYear = value; OnPropertyChanged(nameof(SelectedAcademicYear));
                ReloadGrid();
            }
        }

        private ClassModel _selectedClass;
        public ClassModel SelectedClass
        {
            get => _selectedClass;
            set { _selectedClass = value; OnPropertyChanged(nameof(SelectedClass));
                ReloadGrid();
            }
        }

        private SectionModel _selectedSection;
        public SectionModel SelectedSection
        {
            get => _selectedSection;
            set { _selectedSection = value; OnPropertyChanged(nameof(SelectedSection)); }
        }

        private string _selectedFeeType;
        public string SelectedFeeType
        {
            get => _selectedFeeType;
            set { _selectedFeeType = value; OnPropertyChanged(nameof(SelectedFeeType)); ReloadGrid(); }
        }

        // ================= FEE HEAD GRID =================
        public ObservableCollection<FeeHeadAmountVM> FeeHeadAmounts { get; }

        public ICommand SaveCommand { get; }

        // ================= LOADERS =================
        private async void ReloadGrid()
        {
            if (SelectedAcademicYear == null ||
                SelectedClass == null ||
                string.IsNullOrEmpty(SelectedFeeType))
                return;

            _feeHeadsLoaded = false;

            // Reload fee heads FIRST
            FeeHeadAmounts.Clear();

            var heads = await _feeService.GetFeeHeadsAsync(SelectedFeeType);
            foreach (var h in heads)
            {
                FeeHeadAmounts.Add(new FeeHeadAmountVM
                {
                    FeeHeadId = h.FeeHeadId,
                    FeeHeadName = h.FeeHeadName,
                    Amount = 0
                });
            }

            _feeHeadsLoaded = true;

            // THEN load saved structure
            TryLoadFeeStructure();
        }

        private async void LoadAcademicYears()
        {
            AcademicYears.Clear();
            foreach (var y in await _academicYearService.GetAllAsync())
                AcademicYears.Add(y);
        }

        private async void LoadClasses()
        {
            Classes.Clear();

            var classes = (await _classService.GetClassesAsync())
                .GroupBy(x => x.ClassName)
                .Select(g => g.First())
                .ToList();

            foreach (var c in classes)
                Classes.Add(c);
        }


        private async void LoadSections()
        {
            Sections.Clear();
            foreach (var s in await _sectionService.GetAllAsync())
                Sections.Add(s);
        }

        private async void LoadFeeHeads()
        {
            FeeHeadAmounts.Clear();

            var heads = await _feeService.GetFeeHeadsAsync("OneTime");

            foreach (var h in heads)
            {
                FeeHeadAmounts.Add(new FeeHeadAmountVM
                {
                    FeeHeadId = h.FeeHeadId,
                    FeeHeadName = h.FeeHeadName,
                    Amount = 0
                });
            }

            _feeHeadsLoaded = true;

            // 🔥 IMPORTANT: retry load after fee heads exist
            TryLoadFeeStructure();
        }


        // ================= SAVE =================
        private async Task SaveAsync(object parameter)
        {
            if (parameter is DataGrid grid)
            {
                grid.CommitEdit(DataGridEditingUnit.Cell, true);
                grid.CommitEdit(DataGridEditingUnit.Row, true);
            }

            if (SelectedAcademicYear == null || SelectedClass == null)
            {
                MessageBox.Show("Please select Academic Year and Class.");
                return;
            }

            if (!FeeHeadAmounts.Any(x => x.Amount > 0))
            {
                MessageBox.Show("Please enter at least one fee amount.");
                return;
            }

            var model = new FeeStructureModel
            {
                AcademicYearId = SelectedAcademicYear.AcademicYearId,
                ClassId = SelectedClass.ClassId,
                SectionId = SelectedSection?.SectionId,
                FeeType = SelectedFeeType,
                FeesDetails = FeeHeadAmounts
                    .Where(x => x.Amount > 0)
                    .Select(x => new FeeStructureDetailModel
                    {
                        FeeHeadId = x.FeeHeadId,
                        Amount = x.Amount
                    }).ToList()
            };

            await _feeService.SaveFeeStructureAsync(model);

            MessageBox.Show("Fee Structure saved successfully.");
            
          
        }
        private void LoadSavedStructure(List<FeeStructureDetailModel> saved)
        {
            foreach (var item in FeeHeadAmounts)
            {
                var match = saved.FirstOrDefault(x => x.FeeHeadId == item.FeeHeadId);
                item.Amount = match?.Amount ?? 0;
            }
        }
        private void LoadEmptyStructure()
        {
            foreach (var item in FeeHeadAmounts)
            {
                item.Amount = 0;
            }
        }

        private async void TryLoadFeeStructure()
        {
            if (!_feeHeadsLoaded)
                return;

            if (SelectedAcademicYear == null ||
                SelectedClass == null ||
                string.IsNullOrEmpty(SelectedFeeType))
                return;

            var saved = await _feeService.GetFeeStructureAsync(
                SelectedAcademicYear.AcademicYearId,
                SelectedClass.ClassId,
                SelectedFeeType
            );

            if (saved.Any())
                LoadSavedStructure(saved);
            else
                LoadEmptyStructure();
        }




    }



}
