using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class StudentFeeAssignmentDetailModel
    {
        public int FeeHeadId { get; set; }
        public string FeeHeadName { get; set; }

        public decimal FeeAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetAmount { get; set; }

        public DateTime DueDate { get; set; }
    }

}
