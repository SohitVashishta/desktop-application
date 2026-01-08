using System.Data;
using System.IO;
using ClosedXML.Excel;
using SchoolManagementSystem.Business.Services;

public class ReportService
{
    public void ExportStudentsToExcel(string path)
    {
        var service = new StudentService();
        var students = service.GetStudents();

        var wb = new XLWorkbook();
        var ws = wb.AddWorksheet("Students");

        ws.Cell(1, 1).Value = "First Name";
        ws.Cell(1, 2).Value = "Email";

        int row = 2;
        foreach (var s in students)
        {
            ws.Cell(row, 1).Value = s.FirstName;
            ws.Cell(row, 2).Value = s.Email;
            row++;
        }

        wb.SaveAs(path);
    }
}
