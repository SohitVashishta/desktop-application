using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.FeeManagement
{
    public class FeeStructureViewModel : NotifyPropertyChangedBase
    {
        public readonly IFeeService feeService;
        public FeeStructureViewModel(IFeeService feeService)
        {
            this.feeService = feeService;
            OpenAddFeeDialogCommand = new RelayCommand(OpenAddFeeDialog);
            CloseAddFeeDialogCommand = new RelayCommand(CloseAddFeeDialog);
           
            RefreshKpis();
            _ = LoadAsync();
        }

        // ================= GRID =================
        public ObservableCollection<FeeHeadModel> FeeDetails { get; }
            = new ObservableCollection<FeeHeadModel>();

        // ================= SEARCH =================
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        /* ================= LOAD ================= */

        // ================= LOAD =================
        private async Task LoadAsync()
        {
            FeeDetails.Clear();

            var fees = await feeService.GetAllAsync();

            foreach (var fee in fees)
                FeeDetails.Add(fee);

            RefreshKpis();
        }
        // ================= KPI =================
        public int TotalFeesCount => FeeDetails.Count;
        public int MonthlyFeesCount => 0;
        public int AnnualFeesCount => 0;
        public int TotalClasses => 0;

        // ================= DIALOG =================
        private bool _isAddFeeDialogOpen;
        public bool IsAddFeeDialogOpen
        {
            get => _isAddFeeDialogOpen;
            set
            {
                _isAddFeeDialogOpen = value;
                OnPropertyChanged(nameof(IsAddFeeDialogOpen));
            }
        }
        public void AddFee(FeeHeadModel fee)
        {
            FeeDetails.Add(fee);

            // refresh KPIs
            OnPropertyChanged(nameof(TotalFeesCount));
            OnPropertyChanged(nameof(MonthlyFeesCount));
            OnPropertyChanged(nameof(AnnualFeesCount));

            // close dialog

        }

        public void RefreshKpis()
        {
            OnPropertyChanged(nameof(TotalFeesCount));
            OnPropertyChanged(nameof(MonthlyFeesCount));
            OnPropertyChanged(nameof(AnnualFeesCount));
            OnPropertyChanged(nameof(TotalClasses));
        }

        public ICommand OpenAddFeeDialogCommand { get; }
        public ICommand CloseAddFeeDialogCommand { get; }

        private void OpenAddFeeDialog()
        {
            IsAddFeeDialogOpen = true;
        }

        private void CloseAddFeeDialog()
        {
            IsAddFeeDialogOpen = false;
        }

        // ================= PAGINATION (PLACEHOLDER) =================
        public ICommand PrevPageCommand => new RelayCommand(() => { });
        public ICommand NextPageCommand => new RelayCommand(() => { });
        public string PageInfo => "1 of 1";
        public bool CanGoPrevious => false;
        public bool CanGoNext => false;
    }

    // ================= GRID ROW VM =================
   
}
