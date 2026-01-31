using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeeStructureDetailModel
    {
        public int FeeStructureDetailId { get; set; }
        public int FeeHeadId { get; set; }
        public string FeeHeadName { get; set; }
        public decimal Amount { get; set; }
        public string ClassName { get; set; }
        public string Frequency { get; set; } // Monthly / Quarterly / OneTime
    }
}
