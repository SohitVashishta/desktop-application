using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class StudentFeeModel
    {
        public int FeeId { get; set; }
        public int StudentId { get; set; }
        public decimal TotalFees { get; set; }
        public decimal PaidFees { get; set; }
        public string PaymentMode { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ReceiptNo { get; set; }
    }
}
