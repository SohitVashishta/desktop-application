using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class SubjectModel
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }

        public int GradeId { get; set; }
        public string GradeName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

}
