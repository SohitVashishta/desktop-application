using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<BookModel> GetAll(string search);
        void Add(BookModel model);
        void Update(BookModel model);
        void Delete(int id);
        BookSummaryModel GetSummary();
        BookModel GetById(int id);
    }
}
