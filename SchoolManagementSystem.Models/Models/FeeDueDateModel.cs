using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeeDueDateModel
    {
        public int FeeDueDateId { get; set; }
        public int AcademicYearId { get; set; }
        public int ClassId { get; set; }
        public int FeeHeadId { get; set; }

        public DateTime DueDate { get; set; }
        public decimal LateFeePerDay { get; set; }
    }

}
