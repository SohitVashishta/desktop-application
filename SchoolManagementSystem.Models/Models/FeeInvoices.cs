using System;

namespace SchoolManagementSystem.Models
{
    public class FeeInvoice
    {
        public int FeeInvoiceId { get; set; }

        public int StudentId { get; set; }

        public decimal Amount { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsPaid { get; set; }

        public DateTime? PaidOn { get; set; }

        public string PaymentMode { get; set; } = string.Empty;

        // Navigation (optional but recommended)
        public Student Student { get; set; }
    }
}
