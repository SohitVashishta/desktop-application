using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string TargetAudience { get; set; } // All, Teachers, Parents, Students
        public string Type { get; set; } // Announcement / Alert / Message
        public string CreatedOn { get; set; }
        public bool IsRead { get; set; }
        public int UserId { get; set; }
    }
}
