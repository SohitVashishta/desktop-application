using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeeReportModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }

        public string ClassName { get; set; }
        public string SectionName { get; set; }

        public decimal TotalFees { get; set; }
        public decimal Concession { get; set; }
        public decimal PaidFees { get; set; }
        public decimal BalanceFees { get; set; }

        public DateTime? PaymentDate { get; set; }
        public string PaymentMode { get; set; }
    }

}
