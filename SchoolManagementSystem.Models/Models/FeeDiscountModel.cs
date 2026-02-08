using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeeDiscountModel
    {
        public int FeeDiscountId { get; set; }

        public int AcademicYearId { get; set; }
        public int ClassId { get; set; }
        public int? SectionId { get; set; }

        public int FeeHeadId { get; set; }

        public bool IsPercentage { get; set; }
        public decimal DiscountValue { get; set; }   // 🔑 replaces DiscountAmount

        public string? Reason { get; set; }

        public bool IsActive { get; set; }
    }

}
