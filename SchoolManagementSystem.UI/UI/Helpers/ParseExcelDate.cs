using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.UI.UI.Helpers
{
    public class ParseExcelDate
    {
        public DateTime ConvertParseExcelDate(IXLCell cell)
        {
            if (cell.DataType == XLDataType.DateTime)
                return cell.GetDateTime();

            if (cell.DataType == XLDataType.Number)
                return DateTime.FromOADate(cell.GetDouble());

            if (DateTime.TryParse(cell.GetString(), out DateTime result))
                return result;

            throw new Exception($"Invalid date value: {cell.Value}");
        }
    }
}
