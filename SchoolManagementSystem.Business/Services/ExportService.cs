using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class ExportService
    {
        public byte[] ExportStudents(List<StudentDto> students)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Students");

            ws.Cell(1, 1).Value = "Name";
            ws.Cell(1, 2).Value = "Class";

            int row = 2;
            foreach (var s in students)
            {
                ws.Cell(row, 1).Value = s.Name;
                ws.Cell(row, 2).Value = s.Class;
                row++;
            }

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }
        public byte[] GeneratePdfReport()
        {
            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            doc.Add(new Paragraph("Student Report"));
            doc.Add(new Paragraph("Generated on " + DateTime.Now));

            doc.Close();
            return ms.ToArray();
        }
    }

}
