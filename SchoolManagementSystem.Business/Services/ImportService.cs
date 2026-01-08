using ClosedXML.Excel;
using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models;
using System.Data;

namespace SchoolManagementSystem.Business.Services
{
    public class ImportService
    {
        private readonly ImportRepository _repo = new();
        private readonly string _conn =
            "Server=.;Database=SchoolDB;Trusted_Connection=True;TrustServerCertificate=True";

        public void ImportStudentsWithProgress(string filePath, IProgress<int> progress)
        {
            var table = new DataTable();
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("DateOfBirth", typeof(DateTime));
            table.Columns.Add("EnrollmentDate", typeof(DateTime));

            using var wb = new XLWorkbook(filePath);
            var ws = wb.Worksheet(1);
            var rows = ws.RowsUsed().Skip(1).ToList();

            int total = rows.Count;
            int current = 0;

            DateTime defaultDob = new DateTime(2005, 1, 1);
            DateTime defaultEnroll = DateTime.Today;

            // 🔹 PHASE 1 – READ EXCEL (0–40%)
            foreach (var row in rows)
            {
                var dob = ParseExcelDateOrDefault(row.Cell(4), defaultDob);
                var enroll = ParseExcelDateOrDefault(row.Cell(5), defaultEnroll);

                table.Rows.Add(
                    row.Cell(1).GetString(),
                    row.Cell(2).GetString(),
                    row.Cell(3).GetString(),
                    dob,
                    enroll
                );

                current++;
                progress.Report(current * 40 / total);
            }

            // 🔹 PHASE 2 – BULK INSERT (40–100%)
            using var con = new SqlConnection(_conn);
            con.Open();

            using var bulk = new SqlBulkCopy(con)
            {
                DestinationTableName = "Students",
                BatchSize = 1000,
                NotifyAfter = 500
            };

            // ✅ COLUMN MAPPING (CRITICAL FIX)
            bulk.ColumnMappings.Add("FirstName", "FirstName");
            bulk.ColumnMappings.Add("LastName", "LastName");
            bulk.ColumnMappings.Add("Email", "Email");
            bulk.ColumnMappings.Add("DateOfBirth", "DateOfBirth");
            bulk.ColumnMappings.Add("EnrollmentDate", "EnrollmentDate");

            bulk.SqlRowsCopied += (s, e) =>
            {
                int percent = 40 + (int)((double)e.RowsCopied / table.Rows.Count * 60);
                progress.Report(percent);
            };

            bulk.WriteToServer(table);

            progress.Report(100);
        }


        public void ImportTeachersWithProgress(string filePath, IProgress<int> progress)
        {
            var table = new DataTable();
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Subject", typeof(string));

            using var wb = new XLWorkbook(filePath);
            var ws = wb.Worksheet(1);
            var rows = ws.RowsUsed().Skip(1).ToList();

            int total = rows.Count;
            if (total == 0) return;

            int current = 0;

            // 🔹 PHASE 1 – READ EXCEL (0–40%)
            foreach (var row in rows)
            {
                table.Rows.Add(
                     row.Cell(1).GetString(),
                    row.Cell(2).GetString(),
                    row.Cell(3).GetString(),
                    row.Cell(4).GetString(),
                    row.Cell(5).GetString()
                );

                current++;
                progress.Report(current * 40 / total);
            }

            using var con = new SqlConnection(_conn);
            con.Open();

            using var bulk = new SqlBulkCopy(con)
            {
                DestinationTableName = "Teachers",
                BatchSize = 1000,
                NotifyAfter = 100   // 🔴 MUST BE SET
            };

            // 🔴 COLUMN MAPPING (MANDATORY)
            bulk.ColumnMappings.Add("Name", "Name");
            bulk.ColumnMappings.Add("FirstName", "FirstName");
            bulk.ColumnMappings.Add("LastName", "LastName");
            bulk.ColumnMappings.Add("Email", "Email");
            bulk.ColumnMappings.Add("Subject", "Subject");

            // 🔴 PROGRESS EVENT (MUST BE BEFORE WriteToServer)
            bulk.SqlRowsCopied += (s, e) =>
            {
                int percent = 40 + (int)((double)e.RowsCopied / table.Rows.Count * 60);

                if (percent > 100) percent = 100;
                progress.Report(percent);
            };
            // 🔹 PHASE 2 – DATABASE INSERT
            bulk.WriteToServer(table);

            progress.Report(100); // ✅ DONE
        }


        private DateTime ParseExcelDateOrDefault(IXLCell cell, DateTime defaultValue)
        {
            if (cell == null || cell.IsEmpty())
                return defaultValue;

            if (cell.DataType == XLDataType.DateTime)
                return cell.GetDateTime();

            if (cell.DataType == XLDataType.Number)
                return DateTime.FromOADate(cell.GetDouble());

            if (DateTime.TryParse(cell.GetString(), out DateTime dt))
                return dt;

            return defaultValue; // fallback
        }


        public void ImportTeachers(string filePath)
        {
            var teachers = new List<Teacher>();
            var wb = new XLWorkbook(filePath);
            var ws = wb.Worksheet(1);

            foreach (var row in ws.RowsUsed().Skip(1))
            {
                teachers.Add(new Teacher
                {
                    FirstName = row.Cell(1).GetString(),
                    LastName = row.Cell(2).GetString(),
                    Email = row.Cell(3).GetString(),
                    Subject = row.Cell(4).GetString()
                });
            }

            _repo.ImportTeachers(teachers);
        }
    }
}
