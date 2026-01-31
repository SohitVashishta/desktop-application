using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class StudentFeeAssignmentModel
    {
        public int StudentFeeAssignmentId { get; set; }
        public int StudentId { get; set; }
        public int AcademicYearId { get; set; }

        public decimal TotalFees { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetFees { get; set; }
        public decimal LateFee { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public String PaymentMode { get; set; }

        public List<StudentFeeAssignmentDetailModel> Details { get; set; }
            = new();
    }
}
