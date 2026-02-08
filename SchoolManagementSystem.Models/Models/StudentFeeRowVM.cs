using SchoolManagementSystem.Models.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class StudentFeeRowVM : NotifyPropertyChangedBase
    {
        public int FeeHeadId { get; set; }
        public string FeeHeadName { get; set; }

        public decimal BaseAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount => BaseAmount - DiscountAmount;

        public DateTime DueDate { get; set; }
    }

}
