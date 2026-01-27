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
        public string RollNo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string StudentName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }

        public string MotherTongue { get; set; }
        public string Religion { get; set; }
        public string Category { get; set; }
        public string BloodGroup { get; set; }

        public string AadhaarNo { get; set; }
        public string Nationality { get; set; }
        public string HasSibling { get; set; }
        public string Email { get; set; }

        public int AcademicYearId { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }

        public DateTime AdmissionDate { get; set; } = DateTime.Today;
        public string AdmissionType { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedOn { get; set; }
    }
}
