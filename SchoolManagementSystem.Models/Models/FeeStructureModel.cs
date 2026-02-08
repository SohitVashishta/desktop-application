using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeeStructureModel
    {
        public int FeeStructureId { get; set; }

        public int AcademicYearId { get; set; }
        public int ClassId { get; set; }
        public int? SectionId { get; set; }

        public string FeeType { get; set; }   // Monthly / Quarterly / Yearly / OneTime

        public decimal TotalAmount { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

        // 🔗 Child records
        public List<FeeStructureDetailModel> FeesDetails { get; set; }
            = new List<FeeStructureDetailModel>();
    }
}
