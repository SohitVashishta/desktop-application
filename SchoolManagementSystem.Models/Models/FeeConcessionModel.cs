using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeeConcessionModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }

        public string ConcessionType { get; set; }
        public string DiscountType { get; set; } // Flat / Percentage
        public decimal DiscountValue { get; set; }

        public decimal AppliedAmount { get; set; }
        public string Reason { get; set; }
    }

}
