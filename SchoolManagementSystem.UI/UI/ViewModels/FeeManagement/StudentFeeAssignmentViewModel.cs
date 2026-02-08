using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.Models.Models.ViewModels.FeeManagement;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.FeeManagement
{
    public class StudentFeeAssignmentViewModel : NotifyPropertyChangedBase
    {
        private readonly IFeeService _feeService;

        /* ================= CONSTRUCTOR ================= */

        public StudentFeeAssignmentViewModel(IFeeService feeService)
        {
            _feeService = feeService;

            FeeRows = new ObservableCollection<StudentFeeRowVM>();
            FeeRows.CollectionChanged += (_, __) => RaiseTotals();

            SaveCommand = new RelayCommand(async () => await SaveAsync());
        }

        /* ================= UI ROWS ================= */

        public ObservableCollection<StudentFeeRowVM> FeeRows { get; }

        /* ================= CALCULATED (UI ONLY) ================= */

        public decimal TotalAmount => FeeRows.Sum(x => x.BaseAmount);
        public decimal TotalDiscount => FeeRows.Sum(x => x.DiscountAmount);
        public decimal NetAmount => FeeRows.Sum(x => x.NetAmount);

        /* ================= SELECTIONS ================= */

        private AcademicYearModel _selectedAcademicYear;
        public AcademicYearModel SelectedAcademicYear
        {
            get => _selectedAcademicYear;
            set { _selectedAcademicYear = value; OnPropertyChanged(nameof(SelectedAcademicYear)); }
        }

        private ClassModel _selectedClass;
        public ClassModel SelectedClass
        {
            get => _selectedClass;
            set { _selectedClass = value; OnPropertyChanged(nameof(SelectedClass)); }
        }

        private StudentModel _selectedStudent;
        public StudentModel SelectedStudent
        {
            get => _selectedStudent;
            set { _selectedStudent = value; OnPropertyChanged(nameof(SelectedStudent)); }
        }

        private string _selectedFeeType;
        public string SelectedFeeType
        {
            get => _selectedFeeType;
            set { _selectedFeeType = value; OnPropertyChanged(nameof(SelectedFeeType)); }
        }

        private DateTime _dueDate = DateTime.Today.AddMonths(1);
        public DateTime DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(nameof(DueDate)); }
        }

        /* ================= COMMANDS ================= */

        public ICommand SaveCommand { get; }

        /* ================= SAVE ================= */

        private async Task SaveAsync()
        {
            if (SelectedAcademicYear == null || SelectedStudent == null)
            {
                MessageBox.Show("Please select Academic Year and Student.");
                return;
            }

            if (!FeeRows.Any())
            {
                MessageBox.Show("No fee rows available.");
                return;
            }

            // ✅ MAP UI ROWS → DOMAIN MODEL
            var assignment = new StudentFeeAssignmentModel
            {
                StudentId = SelectedStudent.StudentId,
                AcademicYearId = SelectedAcademicYear.AcademicYearId,
                FeeType = SelectedFeeType,
                DueDate = DueDate,

                Details = FeeRows.Select(r => new StudentFeeAssignmentDetailModel
                {
                    FeeHeadId = r.FeeHeadId,
                    FeeHeadName = r.FeeHeadName,
                    FeeAmount = r.BaseAmount,
                    DiscountAmount = r.DiscountAmount
                    // ❌ NetAmount NOT set
                }).ToList()
            };

            await _feeService.SaveFeeAssignmentAsync(assignment);

            MessageBox.Show("Student fee assigned successfully.");
        }

        /* ================= HELPERS ================= */

        private void RaiseTotals()
        {
            OnPropertyChanged(nameof(TotalAmount));
            OnPropertyChanged(nameof(TotalDiscount));
            OnPropertyChanged(nameof(NetAmount));
        }
    }
}
