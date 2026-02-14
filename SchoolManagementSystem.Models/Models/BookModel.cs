using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class BookModel
    {
        public int BookId { get; set; }
        public string BookCode { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public bool IsActive { get; set; }
        public int AvailableQuantity { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
