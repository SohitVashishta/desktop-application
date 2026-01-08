namespace SchoolManagementSystem.Models
{
    public class AttendanceReportRow
    {
        public string StudentName { get; set; } = "";
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
    }
}
