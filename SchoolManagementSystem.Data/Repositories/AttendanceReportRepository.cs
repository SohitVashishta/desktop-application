using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Data.Repositories
{
    public class AttendanceReportRepository
    {
        private readonly string _conn =
            "Server=.;Database=SchoolDB;Trusted_Connection=True;TrustServerCertificate=True";

        // 1️⃣ Date-wise attendance
        public List<AttendanceReportRow> GetAttendance(DateTime from, DateTime to)
        {
            var list = new List<AttendanceReportRow>();

            using var con = new SqlConnection(_conn);
            var cmd = new SqlCommand(@"
                SELECT S.FirstName + ' ' + S.LastName AS StudentName,
                       A.AttendanceDate,
                       A.IsPresent
                FROM Attendance A
                JOIN Students S ON S.StudentId = A.StudentId
                WHERE A.AttendanceDate BETWEEN @from AND @to
                ORDER BY StudentName, AttendanceDate", con);

            cmd.Parameters.AddWithValue("@from", from);
            cmd.Parameters.AddWithValue("@to", to);

            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new AttendanceReportRow
                {
                    StudentName = dr["StudentName"].ToString()!,
                    Date = (DateTime)dr["AttendanceDate"],
                    IsPresent = (bool)dr["IsPresent"]
                });
            }
            return list;
        }

        // 2️⃣ Monthly summary
        public List<MonthlyAttendanceSummary> GetMonthlySummary(int month, int year)
        {
            var list = new List<MonthlyAttendanceSummary>();

            using var con = new SqlConnection(_conn);
            var cmd = new SqlCommand(@"
                SELECT S.FirstName + ' ' + S.LastName AS StudentName,
                       COUNT(*) TotalDays,
                       SUM(CASE WHEN A.IsPresent = 1 THEN 1 ELSE 0 END) PresentDays
                FROM Attendance A
                JOIN Students S ON S.StudentId = A.StudentId
                WHERE MONTH(A.AttendanceDate) = @m
                  AND YEAR(A.AttendanceDate) = @y
                GROUP BY S.FirstName, S.LastName", con);

            cmd.Parameters.AddWithValue("@m", month);
            cmd.Parameters.AddWithValue("@y", year);

            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new MonthlyAttendanceSummary
                {
                    StudentName = dr["StudentName"].ToString()!,
                    TotalDays = (int)dr["TotalDays"],
                    PresentDays = (int)dr["PresentDays"]
                });
            }
            return list;
        }

        // 3️⃣ Student-wise report
        public List<AttendanceReportRow> GetStudentWise(int studentId)
        {
            var list = new List<AttendanceReportRow>();

            using var con = new SqlConnection(_conn);
            var cmd = new SqlCommand(@"
                SELECT AttendanceDate, IsPresent
                FROM Attendance
                WHERE StudentId = @sid
                ORDER BY AttendanceDate", con);

            cmd.Parameters.AddWithValue("@sid", studentId);

            con.Open();
            using var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new AttendanceReportRow
                {
                    Date = (DateTime)dr["AttendanceDate"],
                    IsPresent = (bool)dr["IsPresent"]
                });
            }
            return list;
        }
    }
}
