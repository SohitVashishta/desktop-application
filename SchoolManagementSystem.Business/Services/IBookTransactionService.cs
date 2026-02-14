using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface IBookTransactionService
    {
        IEnumerable<BookTransactionModel> GetPaged(string search, string status, int pageNumber, int pageSize);
        int GetCount(string search, string status);

        IEnumerable<BookTransactionModel> GetAllIssued();
        BookTransactionModel GetTransactionById(int transactionId);

        void IssueBook(int bookId, int studentId, DateTime issueDate, DateTime dueDate);
        void ReturnBook(int transactionId);

        IEnumerable<StudentModel> GetAllStudents();
        IEnumerable<BookModel> GetAllBooks();

        (decimal totalCollectedFine, decimal pendingFine) GetFineSummary();
    }
}
