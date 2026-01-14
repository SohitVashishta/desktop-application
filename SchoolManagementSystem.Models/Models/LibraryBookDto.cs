using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class LibraryBookDto
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int TotalCopies { get; set; }
        public int IssuedCopies { get; set; }

        public bool IsIssued => IssuedCopies > 0;
        public int AvailableCopies => TotalCopies - IssuedCopies;
    }
}
