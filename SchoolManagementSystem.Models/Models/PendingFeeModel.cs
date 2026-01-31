using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class PendingFeeModel
    {
        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public decimal TotalFees { get; set; }
        public decimal PaidFees { get; set; }
        public decimal Balance { get; set; }
    }

}
