using ClosedXML.Excel;
using SchoolManagementSystem.Business;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using SchoolManagementSystem.UI.UI.Views.Teachers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class TeacherViewModel : BaseViewModel
    {
        private readonly ITeacherService _teacherService;
        private readonly int _pageSize = 10;
        private Timer? _searchTimer;

        private int _currentPage = 1;
        private string _searchText = string.Empty;
        private DateTime? _fromDate;
        private DateTime? _toDate;

        private readonly ObservableCollection<TeacherMaster> _allTeachers = new();

        // ================= COLLECTIONS =================

        public ObservableCollection<TeacherMaster> Teachers { get; } = new();
        public ObservableCollection<TeacherMaster> PagedTeachers { get; } = new();

        // ================= PAGING =================

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage == value) return;
                _currentPage = value;
                OnPropertyChanged();
                UpdatePagedTeachers();
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
                ApplyFilters();
            }
        }

        public DateTime? ToDate
        {
            get => _toDate;
            set
            {
                _toDate = value;
                OnPropertyChanged();
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

        public TeacherViewModel(ITeacherService teacherService)
        {
            _teacherService = teacherService;

            RefreshCommand = new RelayCommand(async () => await LoadTeachersAsync());
            ExportExcelCommand = new RelayCommand(ExportExcel);

            PrevPageCommand = new RelayCommand(
                () => CurrentPage--,
                () => CurrentPage > 1);

            NextPageCommand = new RelayCommand(
                () => CurrentPage++,
                () => CurrentPage < TotalPages);

           
            DeleteTeacherCommand = new RelayCommand<TeacherMaster>(
                async t => await DeleteTeacherAsync(t));

            _ = LoadTeachersAsync();
        }

        // ================= DATA LOAD =================

        private async Task LoadTeachersAsync()
        {
            _allTeachers.Clear();
            Teachers.Clear();

            var data = await _teacherService.GetTeachersAsync();
            foreach (var teacher in data)
                _allTeachers.Add(teacher);

            ApplyFilters();
        }

        // ================= FILTERING =================

        private void ApplyFilters()
        {
            Teachers.Clear();

            var query = _allTeachers.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(t =>
                    t.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    t.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    t.Subject.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    t.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            if (FromDate.HasValue)
                query = query.Where(t => t.CreatedDate >= FromDate.Value);

            if (ToDate.HasValue)
                query = query.Where(t => t.CreatedDate <= ToDate.Value);

            foreach (var teacher in query)
                Teachers.Add(teacher);

            CurrentPage = 1;
            UpdatePagedTeachers();
            OnPropertyChanged(nameof(TotalPages));
        }

        // ================= PAGING =================

        private void UpdatePagedTeachers()
        {
            PagedTeachers.Clear();

            var pageData = Teachers
                .Skip((CurrentPage - 1) * _pageSize)
                .Take(_pageSize);

            foreach (var teacher in pageData)
                PagedTeachers.Add(teacher);

            OnPropertyChanged(nameof(PagedTeachers));
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

        

        private async Task DeleteTeacherAsync(TeacherMaster teacher)
        {
            if (MessageBox.Show("Delete teacher?", "Confirm",
                MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            await _teacherService.DeleteTeacherAsync(teacher.TeacherId);
            await LoadTeachersAsync();
        }

        // ================= EXPORT =================

        private void ExportExcel()
        {
            using var wb = new XLWorkbook();
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
    }
}
