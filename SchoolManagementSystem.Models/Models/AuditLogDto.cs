using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Action { get; set; }
        public string Module { get; set; }
        public string DateTime { get; set; }
    }
}
