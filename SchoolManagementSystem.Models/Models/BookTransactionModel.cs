using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class BookTransactionModel
    {
        public int TransactionId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string StudentName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal FineAmount { get; set; }
        public string Status { get; set; }
        public bool IsOverdue
        {
            get
            {
                return Status == "Issued" && DueDate.Date < DateTime.Now.Date;
            }
        }

    }

}
