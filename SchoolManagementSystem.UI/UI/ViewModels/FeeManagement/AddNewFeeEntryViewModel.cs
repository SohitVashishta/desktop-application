using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.FeeManagement
{
    public class AddNewFeeEntryViewModel : NotifyPropertyChangedBase
    {
        private readonly IFeeService _feeService;
        private readonly IClassService _classService;

        public AddNewFeeEntryViewModel(
            IFeeService feeService,
            IClassService classService)
        {
            _feeService = feeService;
            _classService = classService;

            FrequencyList = new ObservableCollection<string>
            {
                "Monthly",
                "Quarterly",
                "Half-Yearly",
                "Yearly",
                "OneTime"
            };

            SaveCommand = new RelayCommand(async () => await SaveAsync());

            _ = LoadClassesAsync();
        }

        /* ================= FIELDS ================= */

        private string _feeType;
        public string FeeType
        {
            get => _feeType;
            set { _feeType = value; OnPropertyChanged(nameof(FeeType)); }
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set { _amount = value; OnPropertyChanged(nameof(Amount)); }
        }

        private string _selectedFrequency;
        public string SelectedFrequency
        {
            get => _selectedFrequency;
            set { _selectedFrequency = value; OnPropertyChanged(nameof(SelectedFrequency)); }
        }

        private ClassModel _selectedClass;
        public ClassModel SelectedClass
        {
            get => _selectedClass;
            set { _selectedClass = value; OnPropertyChanged(nameof(SelectedClass)); }
        }

        private DateTime? _dueDate;
        public DateTime? DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(nameof(DueDate)); }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        /* ================= DROPDOWNS ================= */

        public ObservableCollection<string> FrequencyList { get; }

        public ObservableCollection<ClassModel> ClassList { get; }
            = new ObservableCollection<ClassModel>();

        /* ================= COMMAND ================= */

        public ICommand SaveCommand { get; }

        /* ================= LOAD ================= */

        private async Task LoadClassesAsync()
        {
            ClassList.Clear();
            var classes = await _classService.GetClassesAsync();

            foreach (var c in classes)
                ClassList.Add(c);

        }

        /* ================= SAVE ================= */

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(FeeType) ||
                Amount <= 0 ||
                SelectedClass == null ||
                string.IsNullOrWhiteSpace(SelectedFrequency))
                return;

            var model = new FeeHeadModel
            {
                FeeHeadName = FeeType,
                Amount = Amount,
                Frequency = SelectedFrequency,
                ClassId = SelectedClass.ClassId,
                DueDate = Convert.ToDateTime(DueDate),
                Description = Description,
                IsActive = true
            };

            await _feeService.SaveFeeHeadAsync(model);

            // Close dialog safely
            MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
