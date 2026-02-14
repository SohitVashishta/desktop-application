using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.Library
{
    public class IssueBookDialogViewModel : BaseViewModel
    {
        private readonly IBookTransactionService _service;

        public ObservableCollection<StudentModel> Students { get; set; }
        public ObservableCollection<BookModel> Books { get; set; }

        private StudentModel _selectedStudent;

        public StudentModel SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged();
                Validate();
            }
        }

        private BookModel _selectedBook;
        public BookModel SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
                UpdateStock();
                Validate();
            }
        }

        private DateTime _issueDate = DateTime.Now;
        public DateTime IssueDate
        {
            get => _issueDate;
            set
            {
                _issueDate = value;
                DueDate = _issueDate.AddDays(7);
                OnPropertyChanged();
            }
        }

        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set { _dueDate = value; OnPropertyChanged(); }
        }

        private string _stockInfo;
        public string StockInfo
        {
            get => _stockInfo;
            set { _stockInfo = value; OnPropertyChanged(); }
        }

        private bool _isFormValid;
        public bool IsFormValid
        {
            get => _isFormValid;
            set { _isFormValid = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Action CloseAction { get; set; }

        public IssueBookDialogViewModel(IBookTransactionService service)
        {
            _service = service;

            Students = new ObservableCollection<StudentModel>(_service.GetAllStudents());
            Books = new ObservableCollection<BookModel>(_service.GetAllBooks());

            DueDate = IssueDate.AddDays(7);

            SaveCommand = new RelayCommand(OnSave);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke());
        }

        private void UpdateStock()
        {
            if (SelectedBook == null)
                return;

            if (SelectedBook.AvailableQuantity <= 0)
            {
                StockInfo = "⚠ Out of stock";
                IsFormValid = false;
            }
            else
            {
                StockInfo = $"Available: {SelectedBook.AvailableQuantity}";
            }
        }

        private void Validate()
        {
            IsFormValid =
                SelectedStudent != null &&
                SelectedBook != null &&
                SelectedBook.AvailableQuantity > 0;
        }

        private void OnSave()
        {
            try
            {
                _service.IssueBook(
                SelectedBook.BookId,
                SelectedStudent.StudentId,
                IssueDate,
                DueDate);
            MessageBox.Show("Book issued successfully.",
           "Success",
           MessageBoxButton.OK,
           MessageBoxImage.Information);

            CloseAction?.Invoke();  // 🔥 CLOSE DIALOG
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }

}
