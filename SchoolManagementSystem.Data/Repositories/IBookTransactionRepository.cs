using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface IBookTransactionRepository
    {
        // 🔹 Pagination
        IEnumerable<BookTransactionModel> GetPaged(string search, string status, int pageNumber, int pageSize);
        int GetCount(string search, string status);

        // 🔹 Single
        BookTransactionModel GetById(int transactionId);

        // 🔹 Issue / Return
        void IssueBook(int bookId, int studentId, DateTime issueDate, DateTime dueDate);
        void ReturnBook(int transactionId, DateTime returnDate, decimal fineAmount);

        // 🔹 Stock
        void UpdateBookStock(int bookId, int quantityChange);
        BookModel GetBookById(int bookId);

        // 🔹 Validation
        bool IsBookAlreadyIssued(int bookId, int studentId);

        // 🔹 Lists
        IEnumerable<BookTransactionModel> GetAllIssued();
        IEnumerable<StudentModel> GetAllStudents();
        IEnumerable<BookModel> GetAllBooks();

        // 🔹 Fine Summary
        (decimal totalCollectedFine, decimal pendingFine) GetFineSummary();
    }
}
