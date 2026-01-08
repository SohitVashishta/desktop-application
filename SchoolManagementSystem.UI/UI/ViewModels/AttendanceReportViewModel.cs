using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class AttendanceReportViewModel
    {
        private readonly AttendanceReportRepository _repo = new();
        public SeriesCollection ChartSeries { get; set; }
    = new SeriesCollection();
        public ObservableCollection<AttendanceReportRow> ReportData { get; set; }
            = new ObservableCollection<AttendanceReportRow>();

        public ObservableCollection<MonthlyAttendanceSummary> MonthlySummary { get; set; }
            = new ObservableCollection<MonthlyAttendanceSummary>();

        public DateTime FromDate { get; set; } = DateTime.Today.AddDays(-7);
        public DateTime ToDate { get; set; } = DateTime.Today;

        public ICommand LoadReportCommand { get; }
        public ICommand LoadMonthlyCommand { get; }
        public ICommand ExportExcelCommand { get; }
        public ICommand PrintCommand { get; }

        public AttendanceReportViewModel()
        {
            LoadReportCommand = new RelayCommand(LoadReport);
            LoadMonthlyCommand = new RelayCommand(LoadMonthly);
            ExportExcelCommand = new RelayCommand(ExportExcel);
            PrintCommand = new RelayCommand(Print);
        }

        private void LoadReport()
        {
            ReportData.Clear();

            var data = _repo.GetAttendance(FromDate, ToDate);
            foreach (var r in data)
                ReportData.Add(r);

            // ⭐ UPDATE CHART
            LoadChart();
        }
        

        private void LoadMonthly()
        {
            MonthlySummary.Clear();
            var data = _repo.GetMonthlySummary(ToDate.Month, ToDate.Year);
            foreach (var r in data)
                MonthlySummary.Add(r);
        }

        // 1️⃣ EXPORT TO EXCEL
        private void ExportExcel()
        {
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "CSV File|*.csv",
                FileName = "AttendanceReport.csv"
            };

            if (dlg.ShowDialog() != true) return;

            using var sw = new System.IO.StreamWriter(dlg.FileName);
            sw.WriteLine("Student,Date,Present");

            foreach (var r in ReportData)
                sw.WriteLine($"{r.StudentName},{r.Date:d},{(r.IsPresent ? "Yes" : "No")}");

            MessageBox.Show("Exported successfully");
        }

        // 5️⃣ PRINT
        private void Print()
        {
            PrintDialog dlg = new PrintDialog();
            if (dlg.ShowDialog() == true)
            {
                FlowDocument doc = new FlowDocument();
                foreach (var r in ReportData)
                    doc.Blocks.Add(new Paragraph(
                        new Run($"{r.StudentName} - {r.Date:d} - {(r.IsPresent ? "Present" : "Absent")}")));

                dlg.PrintDocument(((IDocumentPaginatorSource)doc).DocumentPaginator,
                    "Attendance Report");
            }
        }
        private void LoadChart()
        {
            int present = ReportData.Count(x => x.IsPresent);
            int absent = ReportData.Count(x => !x.IsPresent);

            ChartSeries.Clear();

            ChartSeries.Add(new PieSeries
            {
                Title = "Present",
                Values = new ChartValues<int> { present }
            });

            ChartSeries.Add(new PieSeries
            {
                Title = "Absent",
                Values = new ChartValues<int> { absent }
            });
        }

    }
}
