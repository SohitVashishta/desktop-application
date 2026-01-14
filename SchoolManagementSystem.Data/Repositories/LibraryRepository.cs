using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class LibraryRepository : ILibraryRepository
    {
        private readonly List<LibraryBookDto> _books = new()
        {
            new LibraryBookDto
            {
                BookId = 1,
                BookName = "C# Programming",
                Author = "MS Press",
                TotalCopies = 10,
                IssuedCopies = 3
            },
            new LibraryBookDto
            {
                BookId = 2,
                BookName = "Mathematics – Class 10",
                Author = "NCERT",
                TotalCopies = 15,
                IssuedCopies = 5
            }
        };

        public Task<List<LibraryBookDto>> GetBooksAsync()
            => Task.FromResult(_books);

        public Task AddBookAsync(LibraryBookDto book)
        {
            book.BookId = _books.Count + 1;
            _books.Add(book);
            return Task.CompletedTask;
        }

        public Task IssueBookAsync(int bookId)
        {
            var book = _books.FirstOrDefault(b => b.BookId == bookId);
            if (book != null && book.AvailableCopies > 0)
                book.IssuedCopies++;

            return Task.CompletedTask;
        }

        public Task ReturnBookAsync(int bookId)
        {
            var book = _books.FirstOrDefault(b => b.BookId == bookId);
            if (book != null && book.IssuedCopies > 0)
                book.IssuedCopies--;

            return Task.CompletedTask;
        }
    }
}
