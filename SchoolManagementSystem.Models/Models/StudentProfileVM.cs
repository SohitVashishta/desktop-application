using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class StudentProfileVM
    {
        public StudentModel Student { get; set; }
        public StudentAddressModel Address { get; set; }
        public StudentParentModel Parent { get; set; }

        public List<StudentClassHistoryModel> ClassHistory { get; set; }
    }
}
