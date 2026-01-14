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

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class FeeManagementViewModel
    {
        private readonly IFeeService _service = new FeeService();

        public ObservableCollection<FeeDto> Fees { get; } = new();

        public ICommand LoadCommand { get; }
        public ICommand RecordPaymentCommand { get; }

        public FeeManagementViewModel()
        {
            LoadCommand = new RelayCommand(async () => await LoadAsync());
            RecordPaymentCommand = new RelayCommand<FeeDto>(RecordPayment);
        }

        private async Task LoadAsync()
        {
            Fees.Clear();
            var data = await _service.GetFeesAsync();
            foreach (var f in data)
                Fees.Add(f);
        }

        private async void RecordPayment(FeeDto fee)
        {
            if (fee == null || fee.IsPaid) return;

            decimal paymentAmount = fee.PendingAmount; // Full payment for now
            await _service.RecordPaymentAsync(fee.FeeId, paymentAmount);

            fee.PaidAmount += paymentAmount;
        }
    }
}
