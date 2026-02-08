using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class StudentFeeAssignmentVM
    {
        public int StudentId { get; set; }

        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }

        public string FeeType { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }

        public DateTime DueDate { get; set; }
    }

}
