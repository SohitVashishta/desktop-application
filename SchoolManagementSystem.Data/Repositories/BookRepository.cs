using Microsoft.Data.SqlClient;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        public BookModel GetById(int id)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Book_GetById", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@BookId", id);

            con.Open();

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new BookModel
                {
                    BookId = Convert.ToInt32(reader["BookId"]),
                    BookCode = reader["BookCode"].ToString(),
                    Title = reader["Title"].ToString(),
                    Author = reader["Author"].ToString(),
                    Category = reader["Category"]?.ToString(),
                    TotalCopies = Convert.ToInt32(reader["TotalCopies"]),
                    AvailableCopies = Convert.ToInt32(reader["AvailableCopies"]),
                    IsActive = Convert.ToBoolean(reader["IsActive"]),
                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                };
            }

            return null;
        }

        public IEnumerable<BookModel> GetAll(string search)
        {
            var list = new List<BookModel>();

            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Book_GetList", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Search",
                string.IsNullOrWhiteSpace(search) ? (object)DBNull.Value : search);

            con.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new BookModel
                {
                    BookId = Convert.ToInt32(reader["BookId"]),
                    BookCode = reader["BookCode"].ToString(),
                    Title = reader["Title"].ToString(),
                    Author = reader["Author"].ToString(),
                    Category = reader["Category"]?.ToString(),
                    TotalCopies = Convert.ToInt32(reader["TotalCopies"]),
                    AvailableCopies = Convert.ToInt32(reader["AvailableCopies"])
                });
            }

            return list;
        }

        public void Add(BookModel model)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Book_Insert", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@BookCode", model.BookCode);
            cmd.Parameters.AddWithValue("@Title", model.Title);
            cmd.Parameters.AddWithValue("@Author", model.Author);
            cmd.Parameters.AddWithValue("@Category", model.Category);
            cmd.Parameters.AddWithValue("@TotalCopies", model.TotalCopies);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Update(BookModel model)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Book_Update", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@BookId", model.BookId);
            cmd.Parameters.AddWithValue("@BookCode", model.BookCode);
            cmd.Parameters.AddWithValue("@Title", model.Title);
            cmd.Parameters.AddWithValue("@Author", model.Author);
            cmd.Parameters.AddWithValue("@Category", model.Category);
            cmd.Parameters.AddWithValue("@TotalCopies", model.TotalCopies);
            cmd.Parameters.AddWithValue("@AvailableCopies", model.AvailableCopies);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Book_Delete", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@BookId", id);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public BookSummaryModel GetSummary()
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Book_GetSummary", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            con.Open();
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new BookSummaryModel
                {
                    TotalBooks = Convert.ToInt32(reader["TotalBooks"]),
                    TotalCopies = Convert.ToInt32(reader["TotalCopies"]),
                    AvailableCopies = Convert.ToInt32(reader["AvailableCopies"]),
                    IssuedCopies = Convert.ToInt32(reader["IssuedCopies"])
                };
            }

            return new BookSummaryModel();
        }
    }

}
