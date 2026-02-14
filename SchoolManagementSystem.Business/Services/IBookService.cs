using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface IBookService
    {
        IEnumerable<BookModel> GetBooks(string search);
        void AddBook(BookModel model);
        void UpdateBook(BookModel model);
        void DeleteBook(int id);
        BookSummaryModel GetSummary();
    }
}
