using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeeRefundModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string ReceiptNo { get; set; }
        public decimal RefundAmount { get; set; }
        public DateTime RefundDate { get; set; }
        public string RefundMode { get; set; }
        public string Reason { get; set; }
    }

}
