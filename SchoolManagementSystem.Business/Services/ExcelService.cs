using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class ExcelService : IExcelService
    {
        public void Export<T>(List<T> data, string sheet, string path)
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add(sheet);
            ws.Cell(1, 1).InsertTable(data);
            wb.SaveAs(path);
        }
    }

}
