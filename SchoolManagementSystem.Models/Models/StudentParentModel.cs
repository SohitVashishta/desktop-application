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
        public string MotherName { get; set; }
        public string GuardianName { get; set; }

        public string ContactNo { get; set; }
        public string Email { get; set; }
    }
}
