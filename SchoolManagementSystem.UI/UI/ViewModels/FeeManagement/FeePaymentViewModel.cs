using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.FeeManagement
{
    public class FeePaymentViewModel : NotifyPropertyChangedBase
    {
        private readonly FeeService _service;

        public StudentFeePaymentModel Payment { get; }

        public decimal TotalFees { get; }
        public decimal PaidTillNow { get; }
        public decimal Balance => TotalFees - PaidTillNow - Payment.PaidAmount;

        public ICommand SavePaymentCommand { get; }

        public FeePaymentViewModel(
            StudentFeeAssignmentModel assignment,
            decimal paidTillNow,
            FeeService service)
        {
            _service = service;

            TotalFees = assignment.NetFees;
            PaidTillNow = paidTillNow;

            Payment = new StudentFeePaymentModel
            {
                StudentId = assignment.StudentId,
                StudentFeeAssignmentId = assignment.StudentFeeAssignmentId,
                ReceiptNo = ReceiptNumberGenerator.Generate(
                    new Random().Next(1, 999999)) // replace with DB ID
            };

            SavePaymentCommand = new RelayCommand(async () =>
            {
                await _service.PayAsync(Payment);
            });
        }
    }

}
