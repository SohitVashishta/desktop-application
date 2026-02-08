using System;
using System.ComponentModel;

namespace SchoolManagementSystem.Models.Models
{
    public class StudentFeeAssignmentDetailModel : INotifyPropertyChanged
    {
        private decimal _feeAmount;
        private decimal _discountAmount;
        private decimal _netAmount;
        private DateTime? _dueDate;

        public int FeeHeadId { get; set; }
        public string FeeHeadName { get; set; }

        // ================= AMOUNTS =================

        public decimal FeeAmount
        {
            get => _feeAmount;
            set
            {
                if (_feeAmount == value) return;
                _feeAmount = value;
                Recalculate();
                OnPropertyChanged(nameof(FeeAmount));
            }
        }

        public decimal DiscountAmount
        {
            get => _discountAmount;
            set
            {
                if (_discountAmount == value) return;
                _discountAmount = value;
                Recalculate();
                OnPropertyChanged(nameof(DiscountAmount));
            }
        }

        public decimal NetAmount
        {
            get => _netAmount;
            private set
            {
                _netAmount = value;
                OnPropertyChanged(nameof(NetAmount));
            }
        }

        // ================= DUE =================

        public DateTime? DueDate
        {
            get => _dueDate;
            set
            {
                _dueDate = value;
                OnPropertyChanged(nameof(DueDate));
            }
        }

        // ================= HELPERS =================

        private void Recalculate()
        {
            NetAmount = Math.Max(0, FeeAmount - DiscountAmount);
        }

        // ================= INotifyPropertyChanged =================

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
