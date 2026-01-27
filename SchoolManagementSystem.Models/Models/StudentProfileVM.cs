using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public StudentFeeModel Fees { get; set; }
        public ObservableCollection<StudentDocumentModel> Documents { get; set; }
        public List<StudentClassHistoryModel> ClassHistory { get; set; }
    }
}
