using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class StaffDto
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public string Role { get; set; } // Teacher / Admin / Accountant
        public decimal BasicSalary { get; set; }
        public decimal Allowance { get; set; }
        public decimal Deduction { get; set; }

        public decimal NetSalary => BasicSalary + Allowance - Deduction;
    }
}
