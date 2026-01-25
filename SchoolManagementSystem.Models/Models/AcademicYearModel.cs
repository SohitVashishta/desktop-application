using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class AcademicYearModel
    {
        public int AcademicYearId { get; set; }

        public string YearName { get; set; }   // e.g. 2024–2025

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsCurrent { get; set; }
        public bool IsActive { get; set; }
    }
}
