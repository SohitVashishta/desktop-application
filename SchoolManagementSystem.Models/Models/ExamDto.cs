using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class ExamDto
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public string ExamDate { get; set; }
        public int MaxMarks { get; set; }
        public bool IsApproved { get; set; }
    }
}
