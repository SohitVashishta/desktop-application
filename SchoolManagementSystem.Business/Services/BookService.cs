using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;

        public BookService(IBookRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<BookModel> GetBooks(string search)
        {
            return _repository.GetAll(search);
        }

        public void AddBook(BookModel model)
        {
            // Basic validation (business logic layer)
            if (string.IsNullOrWhiteSpace(model.Title))
                throw new System.Exception("Book title is required.");

            if (model.TotalCopies <= 0)
                throw new System.Exception("Total copies must be greater than 0.");

            _repository.Add(model);
        }

        public void UpdateBook(BookModel model)
        {
            var existing = _repository.GetById(model.BookId);

            int issuedCopies = existing.TotalCopies - existing.AvailableCopies;

            if (model.TotalCopies < issuedCopies)
                throw new Exception("Total copies cannot be less than issued copies.");

            model.AvailableCopies = model.TotalCopies - issuedCopies;

            _repository.Update(model);
        }

        public void DeleteBook(int id)
        {
            if (id <= 0)
                throw new System.Exception("Invalid Book ID.");

            _repository.Delete(id);
        }

        public BookSummaryModel GetSummary()
        {
            return _repository.GetSummary();
        }
    }
}
