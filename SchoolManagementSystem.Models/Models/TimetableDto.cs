using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class TimetableDto
    {
        public int TimetableId { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string Subject { get; set; }
        public string Teacher { get; set; }
        public string Day { get; set; }
        public string TimeSlot { get; set; }
    }
}
