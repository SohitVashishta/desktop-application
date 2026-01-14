using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeeDto
    {
        public int FeeId { get; set; }
        public string StudentName { get; set; }
        public string Class { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount => TotalAmount - PaidAmount;
        public string DueDate { get; set; }
        public bool IsPaid => PendingAmount <= 0;
    }
}
