using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.FeeManagement
{
    public class StudentFeeAssignViewModel : NotifyPropertyChangedBase
    {
        private readonly IFeeService _feeService;

        /* ================= CORE DATA ================= */

        public StudentModel Student { get; }
        public StudentFeeAssignmentModel Assignment { get; }

        /* ================= PAYMENT INPUT ================= */

        private decimal _paidAmount;
        public decimal PaidAmount
        {
            get => _paidAmount;
            set
            {
                _paidAmount = value;
                OnPropertyChanged(nameof(PaidAmount));
                OnPropertyChanged(nameof(BalanceAmount));
            }
        }

        private string _selectedPaymentMode;
        public string SelectedPaymentMode
        {
            get => _selectedPaymentMode;
            set
            {
                _selectedPaymentMode = value;
                OnPropertyChanged(nameof(SelectedPaymentMode));
            }
        }

        public DateTime PaymentDate { get; set; } = DateTime.Today;

        /* ================= CALCULATED ================= */

        public decimal NetFees => Assignment.NetFees;
        public decimal LateFee => Assignment.LateFee;
        public decimal BalanceAmount => Math.Max(0, NetFees + LateFee - PaidAmount);

        /* ================= COMMANDS ================= */

        public ICommand SaveCommand { get; }
        public ICommand SaveAndPrintCommand { get; }

        /* ================= CONSTRUCTOR ================= */

        public StudentFeeAssignViewModel(
            StudentModel student,
            List<FeeStructureModel> feeStructure,
            List<FeeDiscountModel> discounts,
            List<FeeDueDateModel> dueDates,
            IFeeService feeService)
        {
            Student = student ?? throw new ArgumentNullException(nameof(student));
            _feeService = feeService ?? throw new ArgumentNullException(nameof(feeService));

            Assignment = FeeCalculator.Calculate(
                student, feeStructure, discounts, dueDates);

            SaveCommand = new RelayCommand(async () => await SaveAsync());
            SaveAndPrintCommand = new RelayCommand(async () => await SaveAndPrintAsync());
        }

        /* ================= SAVE ================= */

        private async Task SaveAsync()
        {
            Assignment.PaidAmount = PaidAmount;
            Assignment.PaymentMode = SelectedPaymentMode;
            Assignment.PaymentDate = PaymentDate;

            await _feeService.SaveFeeAssignmentAsync(Assignment);
        }

        /* ================= SAVE + PRINT ================= */

        private async Task SaveAndPrintAsync()
        {
            await SaveAsync();

            var receipt = new FeeReceiptModel
            {
                ReceiptNo = GenerateReceiptNo(),
                StudentId = Student.StudentId,
                StudentName = Student.StudentName,
                ClassSection = $"{Student.ClassName} - {Student.SectionName}",
                NetFees = NetFees,
                LateFee = LateFee,
                PaidAmount = PaidAmount,
                BalanceAmount = BalanceAmount,
                PaymentMode = SelectedPaymentMode,
                PaymentDate = PaymentDate
            };

            await _feeService.SaveReceiptAsync(receipt);

            string path = $@"D:\Receipts\{receipt.ReceiptNo}.pdf";
            FeeReceiptPdfGenerator.Generate(receipt, path);

            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }

        /* ================= HELPERS ================= */

        private static string GenerateReceiptNo()
        {
            return $"RCPT-{DateTime.Now:yyyyMMddHHmmss}";
        }
    }
}
