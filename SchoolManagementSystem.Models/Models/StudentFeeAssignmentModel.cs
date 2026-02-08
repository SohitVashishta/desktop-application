using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolManagementSystem.Models.Models
{
    public class StudentFeeAssignmentModel
    {
        public int StudentFeeAssignmentId { get; set; }
        public int StudentId { get; set; }
        public int AcademicYearId { get; set; }

        public string FeeType { get; set; }

        public List<StudentFeeAssignmentDetailModel> Details { get; set; }

        public decimal TotalFees => Details.Sum(x => x.FeeAmount);
        public decimal DiscountAmount => Details.Sum(x => x.DiscountAmount);
        public decimal NetFees => Details.Sum(x => x.NetAmount);

        public decimal TotalAmount => TotalFees;
        public decimal TotalDiscount => DiscountAmount;
        public decimal NetAmount => NetFees;

        public decimal LateFee { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount => Math.Max(0, NetFees + LateFee - PaidAmount);

        public string PaymentMode { get; set; }
        public DateTime PaymentDate { get; set; }

        public bool IsPaid => BalanceAmount == 0;
        // ✅ HEADER PROPERTY
        public DateTime DueDate { get; set; }
    }

}
