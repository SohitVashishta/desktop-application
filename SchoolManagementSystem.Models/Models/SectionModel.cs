using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class SectionModel
    {
        public int SectionId { get; set; }

        public string SectionName { get; set; }   // A, B, C

        public bool IsActive { get; set; }
    }
}
