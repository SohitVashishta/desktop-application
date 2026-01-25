using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class StudentClassHistoryModel
    {
        public int HistoryId { get; set; }
        public int StudentId { get; set; }

        public int AcademicYearId { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }

        public bool IsCurrent { get; set; }
    }
}
