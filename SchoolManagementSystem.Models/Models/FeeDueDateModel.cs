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
        public string FeeType { get; set; }

        public int FeeHeadId { get; set; }
        public int DueDay { get; set; }
        public int GraceDays { get; set; }
    }

}
