using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class StudentModel
    {
        public int StudentId { get; set; }

        public string AdmissionNo { get; set; }
        public string StudentName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
public string FirstName { get; set; }
public string LastName { get; set; }
        public string Email { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public int AcademicYearId { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

        /* ===== UI / JOIN ONLY (NOT STORED) ===== */
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string AcademicYearName { get; set; }
    }
}
