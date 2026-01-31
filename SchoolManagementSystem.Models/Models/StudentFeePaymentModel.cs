using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class StudentFeePaymentModel
    {
        public int FeePaymentId { get; set; }
        public int StudentId { get; set; }
        public int StudentFeeAssignmentId { get; set; }

        public string ReceiptNo { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Today;
        public string PaymentMode { get; set; }

        public decimal PaidAmount { get; set; }
    }

}
