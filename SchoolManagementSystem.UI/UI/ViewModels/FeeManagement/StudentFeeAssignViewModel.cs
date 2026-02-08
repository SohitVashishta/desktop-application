using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.FeeManagement
{
    public class StudentFeeAssignViewModel : NotifyPropertyChangedBase
    {
        private readonly IFeeService _feeService;
        private readonly IStudentService _studentService;
        private readonly IAcademicYearService _academicYearService;
        private readonly IClassService _classService;

        /* ================= STATE ================= */

        public bool IsClassSelected => SelectedClass != null;
        public bool CanLoadFees =>
            SelectedAcademicYear != null &&
            SelectedClass != null &&
            !string.IsNullOrWhiteSpace(SelectedFeeType);

        /* ================= MASTER LISTS ================= */

        public ObservableCollection<AcademicYearModel> AcademicYears { get; } = new();
        public ObservableCollection<ClassModel> Classes { get; } = new();
        public ObservableCollection<StudentModel> Students { get; } = new();
        public ObservableCollection<string> FeeTypes { get; } =
            new() { "Monthly", "Quarterly", "Yearly", "OneTime" };

        /* ================= SELECTIONS ================= */

        private AcademicYearModel _selectedAcademicYear;
        public AcademicYearModel SelectedAcademicYear
        {
            get => _selectedAcademicYear;
            set
            {
                _selectedAcademicYear = value;
                OnPropertyChanged(nameof(SelectedAcademicYear));
                OnPropertyChanged(nameof(CanLoadFees));
                ResetStudentContext();
                _ = LoadStudentsAsync();
            }
        }

        private ClassModel _selectedClass;
        public ClassModel SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
                OnPropertyChanged(nameof(IsClassSelected));
                OnPropertyChanged(nameof(CanLoadFees));
                ResetStudentContext();
                _ = LoadStudentsAsync();
            }
        }

        private StudentModel _selectedStudent;
        public StudentModel SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
                _ = LoadExistingAssignmentAsync();
            }
        }

        private string _selectedFeeType;
        public string SelectedFeeType
        {
            get => _selectedFeeType;
            set
            {
                _selectedFeeType = value;
                OnPropertyChanged(nameof(SelectedFeeType));
                OnPropertyChanged(nameof(CanLoadFees));
            }
        }

        /* ================= FEE DETAILS ================= */

        public ObservableCollection<StudentFeeAssignmentDetailModel> FeeDetails { get; } = new();

        public decimal TotalAmount => FeeDetails.Sum(x => x.FeeAmount);
        public decimal TotalDiscount => FeeDetails.Sum(x => x.DiscountAmount);
        public decimal NetAmount => FeeDetails.Sum(x => x.NetAmount);
        public decimal NetFees => FeeDetails.Sum(x => x.NetAmount);


        private DateTime _dueDate = DateTime.Today.AddMonths(1);
        public DateTime DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(nameof(DueDate)); }
        }

        /* ================= STATUS ================= */

        private bool _isFeeLocked;
        public bool IsFeeLocked
        {
            get => _isFeeLocked;
            private set
            {
                _isFeeLocked = value;
                OnPropertyChanged(nameof(IsFeeLocked));
                OnPropertyChanged(nameof(AssignButtonText));
            }
        }

        private string _assignmentStatus = "Not Assigned";
        public string AssignmentStatus
        {
            get => _assignmentStatus;
            private set
            {
                _assignmentStatus = value;
                OnPropertyChanged(nameof(AssignmentStatus));
            }
        }

        public string AssignButtonText =>
            IsFeeLocked ? "Locked" :
            FeeDetails.Any() ? "Update Fee" : "Assign Fee";

        /* ================= COMMANDS ================= */

        public ICommand LoadFeeStructureCommand { get; }
        public ICommand AssignFeeCommand { get; }
        public ICommand ClearFilterCommand { get; }

        /* ================= CONSTRUCTOR ================= */

        public StudentFeeAssignViewModel(
            IFeeService feeService,
            IStudentService studentService,
            IAcademicYearService academicYearService,
            IClassService classService)
        {
            _feeService = feeService;
            _studentService = studentService;
            _academicYearService = academicYearService;
            _classService = classService;

            LoadFeeStructureCommand = new RelayCommand(OnLoadFees);
            AssignFeeCommand = new RelayCommand(OnAssignFee);
            ClearFilterCommand = new RelayCommand(ClearFilters);

            AttachFeeDetailEvents();
            _ = LoadMastersAsync();
        }

        /* ================= LOADERS ================= */

        private async Task LoadMastersAsync()
        {
            AcademicYears.Clear();
            Classes.Clear();

            foreach (var y in await _academicYearService.GetAllAsync())
                AcademicYears.Add(y);

            foreach (var c in await _classService.GetClassesAsync())
                Classes.Add(c);
        }

        private async Task LoadStudentsAsync()
        {
            Students.Clear();
            SelectedStudent = null;

            if (SelectedAcademicYear == null || SelectedClass == null)
                return;

            var list = await _studentService.GetByClassAsync(
                SelectedAcademicYear.AcademicYearId,
                SelectedClass.ClassId);

            foreach (var s in list)
                Students.Add(s);
        }

        private async Task LoadExistingAssignmentAsync()
        {
            FeeDetails.Clear();

            if (SelectedStudent == null)
                return;

            var assignment =
                await _studentService.GetStudentFeeAssignmentAsync(
                    SelectedStudent.StudentId);

            if (assignment == null)
            {
                AssignmentStatus = "Not Assigned";
                IsFeeLocked = false;
                RaiseTotals();
                return;
            }

            foreach (var d in assignment.Details)
                FeeDetails.Add(d);

            IsFeeLocked = assignment.PaidAmount > 0;
            AssignmentStatus = IsFeeLocked
                ? "Locked (Payment Started)"
                : "Assigned";

            RaiseTotals();
        }

        /* ================= ACTIONS ================= */


        private async void OnLoadFees()
        {
            if (!CanLoadFees)
            {
                MessageBox.Show("Select Academic Year, Class and Fee Type.");
                return;
            }

            FeeDetails.Clear();

            var structure = await _feeService.GetFeeStructureAsync(
                SelectedAcademicYear.AcademicYearId,
                SelectedClass.ClassId,
                SelectedFeeType);

            foreach (var f in structure)
            {
                FeeDetails.Add(new StudentFeeAssignmentDetailModel
                {
                    FeeHeadId = f.FeeHeadId,
                    FeeHeadName = f.FeeHeadName,
                    FeeAmount = f.Amount,     // ✔ triggers calculation
                    DiscountAmount = 0,       // ✔ triggers calculation
                    DueDate = DueDate
                });
            }

            AssignmentStatus = "Not Assigned";
            IsFeeLocked = false;
            RaiseTotals();
        }

        private async void OnAssignFee()
        {
            if (IsFeeLocked)
            {
                MessageBox.Show("Fee is locked (payment already started).");
                return;
            }

            if (SelectedStudent == null)
            {
                MessageBox.Show("Please select a student.");
                return;
            }

            await _feeService.AssignFeeAsync(
                SelectedStudent.StudentId,
                SelectedAcademicYear.AcademicYearId,
                SelectedFeeType,
                TotalAmount,
                TotalDiscount,
                NetAmount,
                FeeDetails.ToList());
            // 🔥 UPDATE UI STATE EXPLICITLY
            AssignmentStatus = "Assigned";
            IsFeeLocked = false;

            OnPropertyChanged(nameof(AssignmentStatus));
            OnPropertyChanged(nameof(AssignButtonText));
            MessageBox.Show("Fee assigned successfully.");
            await LoadExistingAssignmentAsync();
        }

        /* ================= HELPERS ================= */

        private void ClearFilters()
        {
            SelectedAcademicYear = null;
            SelectedClass = null;
            SelectedStudent = null;
            SelectedFeeType = null;
            Students.Clear();
            FeeDetails.Clear();
            AssignmentStatus = "Not Assigned";
            IsFeeLocked = false;
            RaiseTotals();
        }

        private void ResetStudentContext()
        {
            Students.Clear();
            SelectedStudent = null;
            FeeDetails.Clear();
            AssignmentStatus = "Not Assigned";
            IsFeeLocked = false;
            RaiseTotals();
        }

        private void RaiseTotals()
        {
            OnPropertyChanged(nameof(TotalAmount));
            OnPropertyChanged(nameof(TotalDiscount));
            OnPropertyChanged(nameof(NetAmount));
            OnPropertyChanged(nameof(AssignButtonText));
            OnPropertyChanged(nameof(NetFees));
            OnPropertyChanged(nameof(AssignButtonText));
        }

        private void AttachFeeDetailEvents()
        {
            FeeDetails.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                    foreach (StudentFeeAssignmentDetailModel i in e.NewItems)
                        i.PropertyChanged += (_, __) => RaiseTotals();
            };
        }
    }
}
