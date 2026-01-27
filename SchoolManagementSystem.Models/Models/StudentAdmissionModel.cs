using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    namespace SchoolManagementSystem.Models.Models
    {
        public class StudentAdmissionModel
        {
            public int StudentId { get; set; }
            public string AdmissionNo { get; set; }
            public string StudentName { get; set; }
            public string Gender { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string BloodGroup { get; set; }

            public int AcademicYearId { get; set; }
            public int ClassId { get; set; }
            public int SectionId { get; set; }
            public DateTime AdmissionDate { get; set; }

            public string FatherName { get; set; }
            public string MotherName { get; set; }
            public string MobileNo { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }

            public string Category { get; set; }
            public string Religion { get; set; }
            public string Nationality { get; set; }
        }
    }

}
