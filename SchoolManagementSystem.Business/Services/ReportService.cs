using ClosedXML.Excel;
using SchoolManagementSystem.Business;
using SchoolManagementSystem.Business.Services;
using System.Threading.Tasks;

public class ReportService : IReportService
{
    private readonly IStudentService _studentService;

    // ✅ Inject dependency
    public ReportService(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public async Task ExportStudentsToExcelAsync(string path)
    {
        var students = await _studentService.GetStudentsAsync();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.AddWorksheet("Students");

        // Header
        worksheet.Cell(1, 1).Value = "First Name";
        worksheet.Cell(1, 2).Value = "Email";

        int row = 2;
        foreach (var s in students)
        {
            worksheet.Cell(row, 1).Value = s.FirstName;
            worksheet.Cell(row, 2).Value = s.Email;
            row++;
        }

        workbook.SaveAs(path);
    }
}
