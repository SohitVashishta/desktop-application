namespace SchoolManagementSystem.Models
{
    public class MonthlyAttendanceSummary
    {
        public string StudentName { get; set; } = "";
        public int TotalDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays => TotalDays - PresentDays;
        public double Percentage =>
            TotalDays == 0 ? 0 : (PresentDays * 100.0) / TotalDays;
    }
}
