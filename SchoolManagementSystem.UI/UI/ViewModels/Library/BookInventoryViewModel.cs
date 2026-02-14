using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using SchoolManagementSystem.UI.UI.Views.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.Library
{
    public class BookInventoryViewModel : BaseViewModel
    {
        private readonly IBookService _bookService;

        public ObservableCollection<BookModel> Books { get; set; }

        #region Properties

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                LoadBooks();
            }
        }

        private int _totalBooks;
        public int TotalBooks
        {
            get => _totalBooks;
            set { _totalBooks = value; OnPropertyChanged(); }
        }

        private int _totalCopies;
        public int TotalCopies
        {
            get => _totalCopies;
            set { _totalCopies = value; OnPropertyChanged(); }
        }

        private int _availableCopies;
        public int AvailableCopies
        {
            get => _availableCopies;
            set { _availableCopies = value; OnPropertyChanged(); }
        }

        private int _issuedCopies;
        public int IssuedCopies
        {
            get => _issuedCopies;
            set { _issuedCopies = value; OnPropertyChanged(); }
        }
        private readonly bool _isEdit;

        public int BookId { get; set; }

        private string _bookCode;
        public string BookCode
        {
            get => _bookCode;
            set
            {
                _bookCode = value;
                OnPropertyChanged();
                Validate();
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
                Validate();
            }
        }

        private string _author;
        public string Author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged();
            }
        }

        private string _category;
        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
            }
        }

        
        #endregion

        #region Commands

        public ICommand AddBookCommand { get; }
        public ICommand EditBookCommand { get; }
        public ICommand DeleteBookCommand { get; }
        public RelayCommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Action CloseAction { get; set; }
        #endregion
        private void Validate()
        {
            SaveCommand.RaiseCanExecuteChanged();
        }
        public BookInventoryViewModel(IBookService bookService, BookModel model = null)
        {
            _bookService = bookService;
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
            Books = new ObservableCollection<BookModel>();

            AddBookCommand = new RelayCommand(OnAddBook);
            EditBookCommand = new RelayCommand<BookModel>(OnEditBook);
            DeleteBookCommand = new RelayCommand<BookModel>(OnDeleteBook);

            LoadBooks();
            LoadSummary();
        }

        #region Methods

        private void LoadBooks()
        {
            Books.Clear();

            var list = _bookService.GetBooks(SearchText);

            foreach (var item in list)
                Books.Add(item);
        }

        private void LoadSummary()
        {
            var summary = _bookService.GetSummary();

            TotalBooks = summary.TotalBooks;
            TotalCopies = summary.TotalCopies;
            AvailableCopies = summary.AvailableCopies;
            IssuedCopies = summary.IssuedCopies;
        }

        private void OnAddBook()
        {
            var dialog = new BookDialog();
            var vm = new BookDialogViewModel(_bookService);
            dialog.DataContext = vm;

            vm.CloseAction = () => dialog.Close();

            dialog.ShowDialog();

            LoadBooks();
            LoadSummary();
        }


        private void OnEditBook(BookModel model)
        {
            if (model == null) return;

            var dialog = new BookDialog();
            var vm = new BookDialogViewModel(_bookService, model);
            dialog.DataContext = vm;

            vm.CloseAction = dialog.Close;

            dialog.ShowDialog();

            LoadBooks();
            LoadSummary();
        }


        private void OnDeleteBook(BookModel model)
        {
            if (model == null) return;

            _bookService.DeleteBook(model.BookId);

            LoadBooks();
            LoadSummary();
        }

        #endregion
    }
}
