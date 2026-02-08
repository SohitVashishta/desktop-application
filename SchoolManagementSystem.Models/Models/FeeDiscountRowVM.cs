using SchoolManagementSystem.Models.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeeDiscountRowVM : NotifyPropertyChangedBase
    {
        public int FeeHeadId { get; set; }
        public string FeeHeadName { get; set; }

        public string DiscountType { get; set; }   // Percentage / Flat
        public decimal DiscountValue { get; set; }
    }

}
