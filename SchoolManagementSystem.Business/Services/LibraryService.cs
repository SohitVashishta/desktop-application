using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly ILibraryRepository _repository
            = new LibraryRepository();

        public Task<List<LibraryBookDto>> GetBooksAsync()
            => _repository.GetBooksAsync();

        public Task AddBookAsync(LibraryBookDto book)
            => _repository.AddBookAsync(book);

        public Task IssueBookAsync(int bookId)
            => _repository.IssueBookAsync(bookId);

        public Task ReturnBookAsync(int bookId)
            => _repository.ReturnBookAsync(bookId);
    }
}
