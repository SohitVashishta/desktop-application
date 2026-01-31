using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeeReceiptModel
    {
        public int StudentId { get; set; }
        public string ReceiptNo { get; set; }
        public string StudentName { get; set; }
        public string ClassSection { get; set; }
        public decimal NetFees { get; set; }
        public decimal LateFee { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMode { get; set; }
    }

}
