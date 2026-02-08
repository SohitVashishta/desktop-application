using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class AdminDashboardStats
    {
        public int TotalUsers { get; set; }
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalClasses { get; set; }
        public int TotalPendingFees { get; set; }
        public int TotalFees { get; set; }
        public int UpcomingExams { get; set; }
        public int TotalStaff { get; set; }

        public List<string> RecentActivities { get; set; } = new();
        public List<string> Announcements { get; set; } = new();

        public string DatabaseStatus { get; set; } = "Unknown";
        public string ServerStatus { get; set; } = "Unknown";
        public string LastBackupDate { get; set; } = "-";
    }
}
