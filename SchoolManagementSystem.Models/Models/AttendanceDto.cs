using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class AttendanceDto
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public bool Present { get; set; }
    }

}
