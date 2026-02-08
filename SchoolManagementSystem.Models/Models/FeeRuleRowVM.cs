using SchoolManagementSystem.Models.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeeRuleRowVM : NotifyPropertyChangedBase
    {
        public int FeeHeadId { get; set; }
        public string FeeHeadName { get; set; }

        // Discount
        public string DiscountType { get; set; }   // None / Percentage / Flat
        public decimal DiscountValue { get; set; }

        // Due Date
        public int DueDay { get; set; }            // 1–31
        public int GraceDays { get; set; }         // optional
    }
}
