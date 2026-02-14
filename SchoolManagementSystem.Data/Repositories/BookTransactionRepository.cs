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
    public class BookTransactionRepository : IBookTransactionRepository
    {
        public IEnumerable<BookTransactionModel> GetPaged(string search, string status, int pageNumber, int pageSize)
        {
            var list = new List<BookTransactionModel>();

            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_BookTransaction_GetList", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Search",
                string.IsNullOrWhiteSpace(search) ? (object)DBNull.Value : search);

            cmd.Parameters.AddWithValue("@Status",
                string.IsNullOrWhiteSpace(status) ? (object)DBNull.Value : status);

            cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            con.Open();

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new BookTransactionModel
                {
                    TransactionId=Convert.ToInt32(reader["TransactionId"]),
                    BookId = Convert.ToInt32(reader["BookId"]),
                    BookName =(reader["BookName"]).ToString(),
                    StudentName = reader["StudentName"].ToString(),
                    IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                    DueDate = Convert.ToDateTime(reader["DueDate"]),
                    ReturnDate = reader["ReturnDate"] == DBNull.Value
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(reader["ReturnDate"]),
                    FineAmount = Convert.ToDecimal(reader["FineAmount"]),
                    Status = reader["Status"].ToString()
                });
            }

            return list;
        }
        public int GetCount(string search, string status)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_BookTransaction_GetCount", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Search",
                string.IsNullOrWhiteSpace(search) ? (object)DBNull.Value : search);

            cmd.Parameters.AddWithValue("@Status",
                string.IsNullOrWhiteSpace(status) ? (object)DBNull.Value : status);

            con.Open();

            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        public BookTransactionModel GetById(int transactionId)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_BookTransaction_GetById", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@TransactionId", transactionId);

            con.Open();

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new BookTransactionModel
                {
                    TransactionId = Convert.ToInt32(reader["TransactionId"]),
                    BookId = Convert.ToInt32(reader["BookId"]),
                    StudentName = reader["StudentName"].ToString(),
                    IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                    DueDate = Convert.ToDateTime(reader["DueDate"]),
                    ReturnDate = reader["ReturnDate"] == DBNull.Value
                                ? (DateTime?)null
                                : Convert.ToDateTime(reader["ReturnDate"]),
                    FineAmount = Convert.ToDecimal(reader["FineAmount"]),
                    Status = reader["Status"].ToString()
                };
            }

            return null;
        }
        public void ReturnBook(int transactionId, DateTime returnDate, decimal fineAmount)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_BookTransaction_Return", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@TransactionId", transactionId);
            cmd.Parameters.AddWithValue("@ReturnDate", returnDate);
            cmd.Parameters.AddWithValue("@FineAmount", fineAmount);

            con.Open();
            cmd.ExecuteNonQuery();
        }
        public (decimal totalCollectedFine, decimal pendingFine) GetFineSummary()
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_BookTransaction_GetFineSummary", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            con.Open();
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return (
                    reader["TotalCollectedFine"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TotalCollectedFine"]),
                    reader["PendingFine"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["PendingFine"])
                );
            }

            return (0, 0);
        }

        public void InsertIssue(int bookId, string studentName, DateTime issueDate, DateTime dueDate)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_BookTransaction_Issue", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@BookId", bookId);
            cmd.Parameters.AddWithValue("@StudentName", studentName);
            cmd.Parameters.AddWithValue("@IssueDate", issueDate);
            cmd.Parameters.AddWithValue("@DueDate", dueDate);

            con.Open();
            cmd.ExecuteNonQuery();
        }
        public IEnumerable<BookModel> GetAllBooks()
        {
            var list = new List<BookModel>();

            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("SELECT BookId, Title, AvailableCopies as AvailableQuantity FROM Books", (SqlConnection)con);

            con.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new BookModel
                {
                    BookId = Convert.ToInt32(reader["BookId"]),
                    Title = reader["Title"].ToString(),
                    AvailableQuantity = Convert.ToInt32(reader["AvailableQuantity"])
                });
            }

            return list;
        }

        public IEnumerable<StudentModel> GetAllStudents()
        {
            var list = new List<StudentModel>();

            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("SELECT StudentId, StudentName FROM StudentMaster", (SqlConnection)con);

            con.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new StudentModel
                {
                    StudentId = Convert.ToInt32(reader["StudentId"]),
                    StudentName = reader["StudentName"].ToString()
                });
            }

            return list;
        }

        public void UpdateBookStock(int bookId, int quantityChange)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Book_UpdateStock", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@BookId", bookId);
            cmd.Parameters.AddWithValue("@QuantityChange", quantityChange);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public bool IsBookAlreadyIssued(int bookId, int studentId)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_BookTransaction_CheckDuplicate", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@BookId", bookId);
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            con.Open();
            return Convert.ToBoolean(cmd.ExecuteScalar());
        }

        public void IssueBook(int bookId, int studentId, DateTime issueDate, DateTime dueDate)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_BookTransaction_Issue", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@BookId", bookId);
            cmd.Parameters.AddWithValue("@StudentId", studentId);
            cmd.Parameters.AddWithValue("@IssueDate", issueDate);
            cmd.Parameters.AddWithValue("@DueDate", dueDate);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public BookModel GetBookById(int bookId)
        {
            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_Book_GetById", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@BookId", bookId);

            con.Open();
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new BookModel
                {
                    BookId = Convert.ToInt32(reader["BookId"]),
                    Title = reader["Title"].ToString(),
                    AvailableCopies = Convert.ToInt32(reader["AvailableCopies"])
                };
            }

            return null;
        }
       
        public IEnumerable<BookTransactionModel> GetAllIssued()
        {
            var list = new List<BookTransactionModel>();

            using var con = DbConnectionFactory.Create();
            using var cmd = new SqlCommand("usp_BookTransaction_GetAllIssued", (SqlConnection)con)
            {
                CommandType = CommandType.StoredProcedure
            };

            con.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new BookTransactionModel
                {
                    TransactionId = Convert.ToInt32(reader["TransactionId"]),
                    BookId = Convert.ToInt32(reader["BookId"]),
                    StudentName = reader["StudentName"].ToString(),
                    IssueDate = Convert.ToDateTime(reader["IssueDate"]),
                    DueDate = Convert.ToDateTime(reader["DueDate"]),
                    FineAmount = Convert.ToDecimal(reader["FineAmount"]),
                    Status = reader["Status"].ToString()
                });
            }

            return list;
        }


    }
}
