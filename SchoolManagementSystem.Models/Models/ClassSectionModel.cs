using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class ClassSectionModel
    {
        public int ClassSectionId { get; set; }

        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public int AcademicYearId { get; set; }

        public string SectionName { get; set; }

        public bool IsActive { get; set; }
    }
}
