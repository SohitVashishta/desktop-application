using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using SchoolManagementSystem.UI.UI.Views.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.Library
{
    public class BookDialogViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly IBookService _service;
        private readonly bool _isEdit;
        public Action RequestFocusOnError { get; set; }

        public int BookId { get; set; }

        private string _bookCode;
        public string BookCode
        {
            get => _bookCode;
            set { _bookCode = value; OnPropertyChanged(); }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        private string _author;
        public string Author
        {
            get => _author;
            set { _author = value; OnPropertyChanged(); }
        }

        private string _category;
        public string Category
        {
            get => _category;
            set { _category = value; OnPropertyChanged(); }
        }

        private int _totalCopies;
        public int TotalCopies
        {
            get => _totalCopies;
            set { _totalCopies = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Action CloseAction { get; set; }

        public BookDialogViewModel(IBookService service, BookModel model = null)
        {
            _service = service;

            if (model != null)
            {
                _isEdit = true;

                BookId = model.BookId;
                BookCode = model.BookCode;
                Title = model.Title;
                Author = model.Author;
                Category = model.Category;
                TotalCopies = model.TotalCopies;
            }

            SaveCommand = new RelayCommand(OnSave);
            CancelCommand = new RelayCommand(() => CloseAction?.Invoke());
        }

        private void OnSave()
        {
            if (!IsValid()) return;
            if (!CanSave())
            {
                RequestFocusOnError?.Invoke();
                return;
            }

            var model = new BookModel
            {
                BookId = BookId,
                BookCode = BookCode,
                Title = Title,
                Author = Author,
                Category = Category,
                TotalCopies = TotalCopies
            };

            if (_isEdit)
                _service.UpdateBook(model);
            else
                _service.AddBook(model);

            CloseAction?.Invoke();
        }
        private bool CanSave()
        {
            return string.IsNullOrWhiteSpace(this[nameof(Title)]) &&
                   string.IsNullOrWhiteSpace(this[nameof(BookCode)]) &&
                   string.IsNullOrWhiteSpace(this[nameof(TotalCopies)]);
        }

        private bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(Title)]) &&
                   string.IsNullOrEmpty(this[nameof(BookCode)]) &&
                   string.IsNullOrEmpty(this[nameof(TotalCopies)]);
        }

        #region Validation

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Title) && string.IsNullOrWhiteSpace(Title))
                    return "Title is required.";

                if (columnName == nameof(BookCode) && string.IsNullOrWhiteSpace(BookCode))
                    return "Book Code is required.";

                if (columnName == nameof(TotalCopies) && TotalCopies <= 0)
                    return "Total copies must be greater than 0.";

                return null;
            }
        }

        #endregion
    }

}
