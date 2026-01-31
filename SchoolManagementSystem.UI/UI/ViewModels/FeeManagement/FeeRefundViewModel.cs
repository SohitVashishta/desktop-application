using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.FeeManagement
{
    public class FeeRefundViewModel : NotifyPropertyChangedBase
    {
        private readonly FeeService _service;

        public FeeRefundModel Refund { get; set; } = new();

        public ObservableCollection<string> RefundModes { get; } =
            new() { "Cash", "UPI", "Bank Transfer" };

        public ICommand SaveRefundCommand => new RelayCommand(async () =>
        {
            if (Refund.RefundAmount <= 0)
            {
                MessageBox.Show("Invalid refund amount");
                return;
            }

            await _service.RefundAsync(Refund);

            MessageBox.Show("Refund processed successfully");

            Refund = new FeeRefundModel { RefundDate = DateTime.Today };
            OnPropertyChanged(nameof(Refund));
        });
    }

}
