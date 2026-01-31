using ClosedXML.Excel;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.FeeManagement
{
    public class FeeReportViewModel : NotifyPropertyChangedBase
    {
        private readonly FeeService _feeService;
        public FeeReportViewModel(FeeService feeService)
        {
            _feeService = feeService;
        }

        public ObservableCollection<DailyFeeCollectionModel> DailyCollection { get; }
            = new();

        public DateTime FromDate { get; set; } = DateTime.Today;
        public DateTime ToDate { get; set; } = DateTime.Today;

        public ICommand LoadReportCommand => new RelayCommand(async () =>
        {
            DailyCollection.Clear();
            var data = await _feeService.GetDailyCollectionAsync(FromDate, ToDate);
            foreach (var d in data)
                DailyCollection.Add(d);
        });
        public ObservableCollection<FeeReportModel> Reports { get; }
     = new();

        public int SelectedStudentId { get; set; }

        public ICommand LoadLedgerCommand => new RelayCommand(async () =>
        {
            Reports.Clear();
            var data = await _feeService.GetStudentLedgerAsync(SelectedStudentId);
            foreach (var r in data)
                Reports.Add(r);
        });


public void ExportToExcel(List<FeeReportModel> data)
    {
        var wb = new XLWorkbook();
        var ws = wb.AddWorksheet("Fee Report");

        ws.Cell(1, 1).InsertTable(data);
        wb.SaveAs("FeeReport.xlsx");
    }

}

}
