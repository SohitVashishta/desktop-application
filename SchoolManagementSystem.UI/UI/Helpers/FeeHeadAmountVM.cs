using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.UI.UI.Helpers
{
    public class FeeHeadAmountVM : NotifyPropertyChangedBase
    {
        private int _feeHeadId;
        public int FeeHeadId
        {
            get => _feeHeadId;
            set
            {
                _feeHeadId = value;
                OnPropertyChanged(nameof(FeeHeadId));
            }
        }

        private string _feeHeadName;
        public string FeeHeadName
        {
            get => _feeHeadName;
            set
            {
                _feeHeadName = value;
                OnPropertyChanged(nameof(FeeHeadName));
            }
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        // Optional (future use)
        public bool IsMandatory { get; set; }
        public bool IsRefundable { get; set; }
    }
}