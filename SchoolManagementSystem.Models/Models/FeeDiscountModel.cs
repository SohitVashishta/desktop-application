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
        public int FeeHeadId { get; set; }

        public decimal DiscountAmount { get; set; }
        public bool IsPercentage { get; set; }   // true = %, false = flat
        public string Reason { get; set; }

        public DateTime CreatedOn { get; set; }
    }

}
