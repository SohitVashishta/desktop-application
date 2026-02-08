using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeeHeadModel
    {
        public int FeeHeadId { get; set; }
        public string FeeHeadName { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsActive { get; set; }
        public decimal Amount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal Discount { get; set; }
        public bool IsRefundable { get; set; }
        public string Frequency { get; set; }
        public int  ClassId { get; set; }
        public DateTime DueDate { get; set; }
        public string Description { get; set; }
        public string ClassName { get; set; }
               
    }

}
