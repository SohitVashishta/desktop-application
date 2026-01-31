using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class DailyFeeCollectionModel
    {
        public string ReceiptNo { get; set; }
        public DateTime PaymentDate { get; set; }
        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public decimal PaidAmount { get; set; }
        public string PaymentMode { get; set; }
    }

}
