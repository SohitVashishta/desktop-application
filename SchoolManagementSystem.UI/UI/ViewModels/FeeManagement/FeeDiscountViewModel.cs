using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.FeeManagement
{
    public class FeeDiscountViewModel : NotifyPropertyChangedBase
    {
        private readonly FeeService _discountService;

        public FeeDiscountViewModel(
            FeeService discountService
            )
        {
            _discountService = discountService;

            SaveCommand = new RelayCommand(async () => await SaveAsync());
            _ = LoadFeeHeadsAsync();
        }

        public ObservableCollection<FeeDiscountModel> Discounts { get; }
            = new();

        public ObservableCollection<FeeHeadModel> FeeHeads { get; }
            = new();

        public ICommand SaveCommand { get; }

        private async Task LoadFeeHeadsAsync()
        {
            var heads = await _discountService.GetAllAsync();
            foreach (var h in heads)
            {
                Discounts.Add(new FeeDiscountModel
                {
                    FeeHeadId = h.FeeHeadId,
                    DiscountAmount = 0,
                    IsPercentage = false
                });
            }
        }

        private async Task SaveAsync()
        {
            await _discountService.SaveDiscountsAsync(Discounts.ToList());
        }
    }

}
