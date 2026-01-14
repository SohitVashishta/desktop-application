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
    public class HrPayrollViewModel
    {
        private readonly IHrService _service = new HrService();

        public ObservableCollection<StaffDto> Staff { get; } = new();

        // Form fields
        public string StaffName { get; set; }
        public string Role { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Allowance { get; set; }
        public decimal Deduction { get; set; }

        public ICommand LoadCommand { get; }
        public ICommand AddStaffCommand { get; }
        public ICommand ProcessPayrollCommand { get; }

        public HrPayrollViewModel()
        {
            LoadCommand = new RelayCommand(async () => await LoadAsync());
            AddStaffCommand = new RelayCommand(async () => await AddStaffAsync());
            ProcessPayrollCommand = new RelayCommand<StaffDto>(ProcessPayroll);
        }

        private async Task LoadAsync()
        {
            Staff.Clear();
            foreach (var s in await _service.GetStaffAsync())
                Staff.Add(s);
        }

        private async Task AddStaffAsync()
        {
            if (string.IsNullOrWhiteSpace(StaffName)) return;

            var staff = new StaffDto
            {
                StaffName = StaffName,
                Role = Role,
                BasicSalary = BasicSalary,
                Allowance = Allowance,
                Deduction = Deduction
            };

            await _service.AddStaffAsync(staff);
            Staff.Add(staff);

            StaffName = Role = string.Empty;
            BasicSalary = Allowance = Deduction = 0;
        }

        private async void ProcessPayroll(StaffDto staff)
        {
            if (staff == null) return;
            await _service.ProcessPayrollAsync(staff.StaffId);
        }
    }
}
