using ClosedXML.Excel;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using SchoolManagementSystem.UI.UI.Views.Students;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class StudentViewModel : INotifyPropertyChanged
    {
        private readonly StudentService _service = new();
        private readonly int _pageSize = 10;
        private int _currentPage = 1;
        private string _searchText = "";
        private DateTime? _fromDate;
        private DateTime? _toDate;
        private Timer? _searchTimer;

        public ObservableCollection<Student> Students { get; set; } = new();
        public ObservableCollection<Student> PagedStudents { get; set; } = new();

        public int CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnPropertyChanged(nameof(CurrentPage)); }
        }

        public int TotalPages =>
            (int)Math.Ceiling((double)Students.Count / _pageSize);

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                DebounceSearch();
            }
        }

        public DateTime? FromDate
        {
            get => _fromDate;
            set { _fromDate = value; ApplyFilters(); }
        }

        public DateTime? ToDate
        {
            get => _toDate;
            set { _toDate = value; ApplyFilters(); }
        }

        // COMMANDS
        public ICommand RefreshCommand { get; }
        public ICommand ExportExcelCommand { get; }
        public ICommand PrevPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand EditStudentCommand { get; }
        public ICommand DeleteStudentCommand { get; }

        public StudentViewModel()
        {
            RefreshCommand = new RelayCommand(LoadStudents);
            ExportExcelCommand = new RelayCommand(ExportExcel);
            PrevPageCommand = new RelayCommand(() => ChangePage(-1), () => CurrentPage > 1);
            NextPageCommand = new RelayCommand(() => ChangePage(1), () => CurrentPage < TotalPages);

            EditStudentCommand = new RelayCommand<Student>(EditStudent);
            DeleteStudentCommand = new RelayCommand<Student>(DeleteStudent);

            LoadStudents();
        }

        // ================= CORE LOGIC =================

        private void LoadStudents()
        {
            Students.Clear();
            foreach (var s in _service.GetStudents())
                Students.Add(s);

            CurrentPage = 1;
            UpdatePagedStudents();
            OnPropertyChanged(nameof(TotalPages));
        }

        private void ApplyFilters()
        {
            var filtered = _service.GetStudents().AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
                filtered = filtered.Where(s =>
                    s.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    s.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    s.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

            if (FromDate.HasValue)
                filtered = filtered.Where(s => s.EnrollmentDate >= FromDate);

            if (ToDate.HasValue)
                filtered = filtered.Where(s => s.EnrollmentDate <= ToDate);

            Students = new ObservableCollection<Student>(filtered);
            OnPropertyChanged(nameof(Students));

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

        private void ChangePage(int delta)
        {
            CurrentPage += delta;
            UpdatePagedStudents();
        }

        // ================= SEARCH DEBOUNCE =================

        private void DebounceSearch()
        {
            _searchTimer?.Dispose();
            _searchTimer = new Timer(_ =>
            {
                Application.Current.Dispatcher.Invoke(ApplyFilters);
            }, null, 400, Timeout.Infinite);
        }

        // ================= ROW ACTIONS =================

        private void EditStudent(Student student)
        {
            var win = new StudentAddEditView(student);
            if (win.ShowDialog() == true)
                LoadStudents();
        }

        private void DeleteStudent(Student student)
        {
            if (MessageBox.Show("Delete student?", "Confirm",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _service.DeleteStudent(student.StudentId);
                LoadStudents();
            }
        }

        // ================= EXPORT =================

        private void ExportExcel()
        {
            var wb = new XLWorkbook();
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

        // ================= INotify =================

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
