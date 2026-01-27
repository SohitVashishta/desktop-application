using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class StudentParentModel
    {
        public int ParentId { get; set; }
        public int StudentId { get; set; }

        public string FatherName { get; set; }
        public string FatherQualification { get; set; }
        public string FatherOccupation { get; set; }
        public string FatherOrganization { get; set; }
        public string FatherMobile { get; set; }
        public string FatherAadhaar { get; set; }

        public string MotherName { get; set; }
        public string MotherQualification { get; set; }
        public string MotherOccupation { get; set; }
        public string MotherOrganization { get; set; }
        public string MotherMobile { get; set; }
        public string MotherAadhaar { get; set; }

        public string GuardianName { get; set; }
        public string GuardianRelation { get; set; }
        public string GuardianMobile { get; set; }
        public string GuardianEmail { get; set; }

        public string ContactNo { get; set; }
        public string Email { get; set; }
    }
}
