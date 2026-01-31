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
    public class FeeConcessionViewModel : NotifyPropertyChangedBase
    {
        private readonly FeeService _service;

        public FeeConcessionModel Concession { get; set; } = new();

        public ObservableCollection<string> ConcessionTypes { get; } =
            new() { "Scholarship", "Sibling", "Staff", "Other" };

        public ObservableCollection<string> DiscountTypes { get; } =
            new() { "Flat", "Percentage" };

        public ICommand ApplyConcessionCommand => new RelayCommand(async () =>
        {
            if (Concession.DiscountValue <= 0)
            {
                MessageBox.Show("Invalid discount value");
                return;
            }

            await _service.ApplyFeeConcessionAsync(Concession);

            MessageBox.Show("Concession applied successfully");

            Concession = new FeeConcessionModel();
            OnPropertyChanged(nameof(Concession));
        });
    }

}
