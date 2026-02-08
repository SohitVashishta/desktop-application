using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeeHeadDueDateVM
    {
        public int FeeHeadId { get; set; }
        public string FeeHeadName { get; set; }
        public int DueDay { get; set; }      // e.g. 10th
        public int GraceDays { get; set; }   // e.g. +5 days
    }

}
