using SchoolManagementSystem.Models.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models.ViewModels.FeeManagement
{
    public class StudentFeeDetailModel : NotifyPropertyChangedBase
    {
        public int FeeHeadId { get; set; }
        public string FeeHeadName { get; set; }

        public decimal OriginalAmount { get; set; }

        private decimal _discountAmount;
        public decimal DiscountAmount
        {
            get => _discountAmount;
            set
            {
                _discountAmount = value;
                FinalAmount = OriginalAmount - value;
                OnPropertyChanged(nameof(DiscountAmount));
                OnPropertyChanged(nameof(FinalAmount));
            }
        }

        private decimal _finalAmount;
        public decimal FinalAmount
        {
            get => _finalAmount;
            set
            {
                _finalAmount = value;
                OnPropertyChanged(nameof(FinalAmount));
            }
        }

        public object? DueDay { get; set; }
    }

}
