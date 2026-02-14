using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class BookTransactionService : IBookTransactionService
{
    
        private readonly IBookTransactionRepository _repository;

        public BookTransactionService(IBookTransactionRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<BookTransactionModel> GetPaged(string search, string status, int pageNumber, int pageSize)
            => _repository.GetPaged(search, status, pageNumber, pageSize);

        public int GetCount(string search, string status)
            => _repository.GetCount(search, status);

        public IEnumerable<BookTransactionModel> GetAllIssued()
            => _repository.GetAllIssued();

        public BookTransactionModel GetTransactionById(int transactionId)
            => _repository.GetById(transactionId);

        public IEnumerable<StudentModel> GetAllStudents()
            => _repository.GetAllStudents();

        public IEnumerable<BookModel> GetAllBooks()
            => _repository.GetAllBooks();

    #region ISSUE BOOK

    public void IssueBook(int bookId, int studentId, DateTime issueDate, DateTime dueDate)
    {
        if (bookId <= 0 || studentId <= 0)
            throw new Exception("Invalid book or student.");

        var book = _repository.GetBookById(bookId);

        if (book == null)
            throw new Exception("Book not found.");

        if (book.AvailableCopies <= 0)
            throw new Exception("Book out of stock.");

        if (_repository.IsBookAlreadyIssued(bookId, studentId))
            throw new Exception("Book already issued to this student.");

        _repository.IssueBook(bookId, studentId, issueDate, dueDate);

        // 🔥 Decrease stock
        
    }


    #endregion

    #region RETURN BOOK

    public void ReturnBook(int transactionId)
        {
            var transaction = _repository.GetById(transactionId);

            if (transaction == null)
                throw new Exception("Transaction not found.");

            if (transaction.Status == "Returned")
                throw new Exception("Already returned.");

            DateTime returnDate = DateTime.Now;
            decimal fine = 0;

            if (returnDate.Date > transaction.DueDate.Date)
            {
                int lateDays = (returnDate.Date - transaction.DueDate.Date).Days;
                fine = lateDays * 5; // ₹5 per day
            }

            _repository.ReturnBook(transactionId, returnDate, fine);

            // 🔹 Auto stock increment
           
        }

        #endregion

        public (decimal totalCollectedFine, decimal pendingFine) GetFineSummary()
            => _repository.GetFineSummary();
    }
