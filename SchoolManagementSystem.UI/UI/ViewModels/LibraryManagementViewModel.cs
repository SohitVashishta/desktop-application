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

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class LibraryManagementViewModel
    {
        private readonly ILibraryService _service = new LibraryService();

        public ObservableCollection<LibraryBookDto> Books { get; }
            = new();

        // Form fields
        public string BookName { get; set; }
        public string Author { get; set; }
        public int TotalCopies { get; set; }

        public ICommand LoadCommand { get; }
        public ICommand AddBookCommand { get; }
        public ICommand IssueBookCommand { get; }
        public ICommand ReturnBookCommand { get; }

        public LibraryManagementViewModel()
        {
            LoadCommand = new RelayCommand(async () => await LoadAsync());
            AddBookCommand = new RelayCommand(async () => await AddBookAsync());
            IssueBookCommand = new RelayCommand<LibraryBookDto>(IssueBook);
            ReturnBookCommand = new RelayCommand<LibraryBookDto>(ReturnBook);
        }

        private async Task LoadAsync()
        {
            Books.Clear();
            foreach (var b in await _service.GetBooksAsync())
                Books.Add(b);
        }

        private async Task AddBookAsync()
        {
            if (string.IsNullOrWhiteSpace(BookName) || TotalCopies <= 0)
                return;

            var book = new LibraryBookDto
            {
                BookName = BookName,
                Author = Author,
                TotalCopies = TotalCopies,
                IssuedCopies = 0
            };

            await _service.AddBookAsync(book);
            Books.Add(book);

            BookName = Author = string.Empty;
            TotalCopies = 0;
        }

        private async void IssueBook(LibraryBookDto book)
        {
            if (book == null || book.AvailableCopies <= 0) return;
            await _service.IssueBookAsync(book.BookId);
            book.IssuedCopies++;
        }

        private async void ReturnBook(LibraryBookDto book)
        {
            if (book == null || book.IssuedCopies <= 0) return;
            await _service.ReturnBookAsync(book.BookId);
            book.IssuedCopies--;
        }
    }
}
