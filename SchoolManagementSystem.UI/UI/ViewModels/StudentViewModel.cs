using ClosedXML.Excel;
using SchoolManagementSystem.Business;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using SchoolManagementSystem.UI.UI.Views.Students;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class StudentViewModel : BaseViewModel
    {
        private readonly IStudentService _studentService;

        private readonly int _pageSize = 10;
        private int _currentPage = 1;
        private string _searchText = string.Empty;
        private DateTime? _fromDate;
        private DateTime? _toDate;
        private Timer? _searchTimer;

        public ObservableCollection<Student> Students { get; } = new();
        public ObservableCollection<Student> PagedStudents { get; } = new();

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
                UpdatePagedStudents();
            }
        }

        public int TotalPages =>
            (int)Math.Ceiling((double)Students.Count / _pageSize);

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                DebounceSearch();
            }
        }

        public DateTime? FromDate
        {
            get => _fromDate;
            set
            {
                _fromDate = value;
                OnPropertyChanged();
                _ = ApplyFiltersAsync();
            }
        }

        public DateTime? ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                OnPropertyChanged();
                _ = ApplyFiltersAsync();
            }
        }

        // COMMANDS
        public ICommand RefreshCommand { get; }
        public ICommand ExportExcelCommand { get; }
        public ICommand PrevPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand EditStudentCommand { get; }
        public ICommand DeleteStudentCommand { get; }

        // ✅ Constructor Injection
        public StudentViewModel(IStudentService studentService)
        {
            _studentService = studentService;

            RefreshCommand = new RelayCommand(async () => await LoadStudentsAsync());
            ExportExcelCommand = new RelayCommand(ExportExcel);

            PrevPageCommand = new RelayCommand(() =>
            {
                if (CurrentPage > 1)
                    CurrentPage--;
            });

            NextPageCommand = new RelayCommand(() =>
            {
                if (CurrentPage < TotalPages)
                    CurrentPage++;
            });

            EditStudentCommand = new RelayCommand<Student>(EditStudent);
            DeleteStudentCommand = new RelayCommand<Student>(async s => await DeleteStudentAsync(s));

            _ = LoadStudentsAsync();
        }

        // ================= CORE LOGIC =================

        private async Task LoadStudentsAsync()
        {
            Students.Clear();

            var data = await _studentService.GetStudentsAsync();
            foreach (var s in data)
                Students.Add(s);

            CurrentPage = 1;
            UpdatePagedStudents();
            OnPropertyChanged(nameof(TotalPages));
        }

        private async Task ApplyFiltersAsync()
        {
            var all = await _studentService.GetStudentsAsync();

            var filtered = all.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
                filtered = filtered.Where(s =>
                    s.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    s.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    s.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            if (FromDate.HasValue)
                filtered = filtered.Where(s => s.EnrollmentDate >= FromDate.Value);

            if (ToDate.HasValue)
                filtered = filtered.Where(s => s.EnrollmentDate <= ToDate.Value);

            Students.Clear();
            foreach (var s in filtered)
                Students.Add(s);

            CurrentPage = 1;
            UpdatePagedStudents();
            OnPropertyChanged(nameof(TotalPages));
        }

        private void UpdatePagedStudents()
        {
            PagedStudents.Clear();

            var pageData = Students
                .Skip((CurrentPage - 1) * _pageSize)
                .Take(_pageSize);

            foreach (var s in pageData)
                PagedStudents.Add(s);

            OnPropertyChanged(nameof(PagedStudents));
        }

        // ================= SEARCH DEBOUNCE =================

        private void DebounceSearch()
        {
            _searchTimer?.Dispose();
            _searchTimer = new Timer(async _ =>
            {
                await Application.Current.Dispatcher.InvokeAsync(ApplyFiltersAsync);
            }, null, 400, Timeout.Infinite);
        }

        // ================= ROW ACTIONS =================

        private async void EditStudent(Student student)
        {
            var win = new StudentAddEditView(student);
            if (win.ShowDialog() == true)
                await LoadStudentsAsync();
        }

        private async Task DeleteStudentAsync(Student student)
        {
            if (MessageBox.Show("Delete student?", "Confirm",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await _studentService.DeleteStudentAsync(student.StudentId);
                await LoadStudentsAsync();
            }
        }

        // ================= EXPORT =================

        private void ExportExcel()
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Students");

            ws.Cell(1, 1).Value = "First Name";
            ws.Cell(1, 2).Value = "Last Name";
            ws.Cell(1, 3).Value = "Email";
            ws.Cell(1, 4).Value = "Enrollment Date";

            int r = 2;
            foreach (var s in Students)
            {
                ws.Cell(r, 1).Value = s.FirstName;
                ws.Cell(r, 2).Value = s.LastName;
                ws.Cell(r, 3).Value = s.Email;
                ws.Cell(r, 4).Value = s.EnrollmentDate;
                r++;
            }

            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = "Students.xlsx"
            };

            if (dlg.ShowDialog() == true)
            {
                wb.SaveAs(dlg.FileName);
                MessageBox.Show("Export completed");
            }
        }
    }
}
