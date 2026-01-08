using ClosedXML.Excel;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using SchoolManagementSystem.UI.UI.Views.Teachers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class TeacherViewModel : INotifyPropertyChanged
    {
        private readonly TeacherService _service = new();
        private readonly int _pageSize = 10;

        private int _currentPage = 1;
        private string _searchText = "";
        private DateTime? _fromDate;
        private DateTime? _toDate;
        private Timer? _searchTimer;

        public ObservableCollection<Teacher> Teachers { get; set; } = new();
        public ObservableCollection<Teacher> PagedTeachers { get; set; } = new();

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }

        public int TotalPages =>
            (int)Math.Ceiling((double)Teachers.Count / _pageSize);

        // ================= FILTERS =================

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
            set
            {
                _fromDate = value;
                ApplyFilters();
            }
        }

        public DateTime? ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                ApplyFilters();
            }
        }

        // ================= COMMANDS =================

        public ICommand RefreshCommand { get; }
        public ICommand ExportExcelCommand { get; }
        public ICommand PrevPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand EditTeacherCommand { get; }
        public ICommand DeleteTeacherCommand { get; }

        // ================= CONSTRUCTOR =================

        public TeacherViewModel()
        {
            RefreshCommand = new RelayCommand(LoadTeachers);
            ExportExcelCommand = new RelayCommand(ExportExcel);

            PrevPageCommand = new RelayCommand(
                () => ChangePage(-1),
                () => CurrentPage > 1);

            NextPageCommand = new RelayCommand(
                () => ChangePage(1),
                () => CurrentPage < TotalPages);

            EditTeacherCommand = new RelayCommand<Teacher>(EditTeacher);
            DeleteTeacherCommand = new RelayCommand<Teacher>(DeleteTeacher);

            LoadTeachers();
        }

        // ================= CORE LOGIC =================

        private void LoadTeachers()
        {
            Teachers.Clear();

            foreach (var t in _service.GetTeachers())
                Teachers.Add(t);

            CurrentPage = 1;
            UpdatePagedTeachers();
            OnPropertyChanged(nameof(TotalPages));
        }

        private void ApplyFilters()
        {
            var query = _service.GetTeachers().AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(t =>
                    t.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    t.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    t.Subject.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    t.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            // Optional date filter (safe even if not used)
            if (FromDate.HasValue)
                query = query.Where(t => t.CreatedDate >= FromDate.Value);

            if (ToDate.HasValue)
                query = query.Where(t => t.CreatedDate <= ToDate.Value);

            Teachers = new ObservableCollection<Teacher>(query);
            OnPropertyChanged(nameof(Teachers));

            CurrentPage = 1;
            UpdatePagedTeachers();
            OnPropertyChanged(nameof(TotalPages));
        }

        private void UpdatePagedTeachers()
        {
            PagedTeachers.Clear();

            var pageData = Teachers
                .Skip((CurrentPage - 1) * _pageSize)
                .Take(_pageSize);

            foreach (var t in pageData)
                PagedTeachers.Add(t);

            OnPropertyChanged(nameof(PagedTeachers));
        }

        private void ChangePage(int delta)
        {
            CurrentPage += delta;
            UpdatePagedTeachers();
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

        private void EditTeacher(Teacher teacher)
        {
            var win = new TeacherAddEditView(teacher);
            if (win.ShowDialog() == true)
                LoadTeachers();
        }

        private void DeleteTeacher(Teacher teacher)
        {
            if (MessageBox.Show("Delete teacher?", "Confirm",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _service.DeleteTeacher(teacher.TeacherId);
                LoadTeachers();
            }
        }

        // ================= EXPORT =================

        private void ExportExcel()
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Teachers");

            ws.Cell(1, 1).Value = "First Name";
            ws.Cell(1, 2).Value = "Last Name";
            ws.Cell(1, 3).Value = "Subject";
            ws.Cell(1, 4).Value = "Email";

            int r = 2;
            foreach (var t in Teachers)
            {
                ws.Cell(r, 1).Value = t.FirstName;
                ws.Cell(r, 2).Value = t.LastName;
                ws.Cell(r, 3).Value = t.Subject;
                ws.Cell(r, 4).Value = t.Email;
                r++;
            }

            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = "Teachers.xlsx"
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
