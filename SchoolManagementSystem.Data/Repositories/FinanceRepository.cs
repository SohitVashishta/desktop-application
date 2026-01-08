using System.Linq;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Data.Repositories
{
    public class FinanceRepository
    {
        // =============================
        // DASHBOARD: TOTAL PENDING FEES
        // =============================
        public decimal GetPendingAmount()
        {
            using var ctx = new SchoolDbContext();

            return ctx.FeeInvoices
                      .Where(f => !f.IsPaid)
                      .Sum(f => f.Amount);
        }

        // =============================
        // LIST INVOICES
        // =============================
        public IQueryable<FeeInvoice> GetAll()
        {
            var ctx = new SchoolDbContext();
            return ctx.FeeInvoices;
        }

        // =============================
        // ADD INVOICE
        // =============================
        public void Add(FeeInvoice invoice)
        {
            using var ctx = new SchoolDbContext();
            ctx.FeeInvoices.Add(invoice);
            ctx.SaveChanges();
        }

        // =============================
        // MARK AS PAID
        // =============================
        public void MarkPaid(int invoiceId, string paymentMode)
        {
            using var ctx = new SchoolDbContext();

            var invoice = ctx.FeeInvoices.FirstOrDefault(x => x.FeeInvoiceId == invoiceId);
            if (invoice == null) return;

            invoice.IsPaid = true;
            invoice.PaidOn = System.DateTime.Today;
            invoice.PaymentMode = paymentMode;

            ctx.SaveChanges();
        }
    }
}
