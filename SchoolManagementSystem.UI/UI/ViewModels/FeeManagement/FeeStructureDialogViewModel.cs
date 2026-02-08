using MaterialDesignThemes.Wpf;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.FeeManagement
{
    public class FeeStructureDialogViewModel : NotifyPropertyChangedBase
    {
        private readonly IFeeService _service;

        public ObservableCollection<FeeHeadAmountVM> FeeHeadAmounts { get; private set; }

        public ICommand SaveCommand { get; }

        public FeeStructureDialogViewModel()
        {
            _service = new FeeService();
            FeeHeadAmounts = new ObservableCollection<FeeHeadAmountVM>();

            SaveCommand = new RelayCommand(async () => await SaveAsync());

            _ = LoadFeeHeadsAsync();
        }

        // ================= LOAD =================
        private async Task LoadFeeHeadsAsync()
        {
            var feeHeads = await _service.GetFeeHeadsAsync("OneTime");

            FeeHeadAmounts = new ObservableCollection<FeeHeadAmountVM>(
                feeHeads.Select(h => new FeeHeadAmountVM
                {
                    FeeHeadId = h.FeeHeadId,
                    FeeHeadName = h.FeeHeadName,
                    Amount = 0
                })
            );

            OnPropertyChanged(nameof(FeeHeadAmounts));
        }

        // ================= SAVE =================
        private async Task SaveAsync()
        {
            if (FeeHeadAmounts == null || FeeHeadAmounts.Count == 0)
                return;

            // ✅ Map ViewModel → Domain Model
            var model = new FeeStructureModel
            {
                FeesDetails = FeeHeadAmounts.Select(x => new FeeStructureDetailModel
                {
                    FeeHeadId = x.FeeHeadId,
                    Amount = x.Amount
                }).ToList()
            };

            await _service.SaveFeeStructureAsync(model);

            DialogHost.CloseDialogCommand.Execute(null, null);
        }
    }
}
