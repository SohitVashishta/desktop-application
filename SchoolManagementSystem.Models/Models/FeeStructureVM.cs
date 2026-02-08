using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeeStructureVM
    {
        public int FeeStructureId { get; set; }
        public string AcademicYear { get; set; }
        public string ClassName { get; set; }
        public string FeeType { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsActive { get; set; }
    }
}
