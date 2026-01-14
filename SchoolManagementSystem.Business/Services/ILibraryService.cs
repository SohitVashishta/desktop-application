using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface ILibraryService
    {
        Task<List<LibraryBookDto>> GetBooksAsync();
        Task AddBookAsync(LibraryBookDto book);
        Task IssueBookAsync(int bookId);
        Task ReturnBookAsync(int bookId);
    }
}
