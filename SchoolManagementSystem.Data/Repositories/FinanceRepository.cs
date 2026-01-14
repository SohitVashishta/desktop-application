using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Models;
using SchoolManagementSystem.Models.Models;

namespace SchoolManagementSystem.Data.Repositories
{
    public class FinanceRepository : IFinanceRepository
    {
        private readonly SchoolDbContext _context;

        // ✅ DbContext injected
        public FinanceRepository(SchoolDbContext context)
        {
            _context = context;
        }

        // =============================
        // DASHBOARD: TOTAL PENDING FEES
        // =============================
        public async Task<decimal> GetPendingAmountAsync()
        {
            return await _context.FeeInvoices
                .AsNoTracking()
                .Where(f => !f.IsPaid)
                .SumAsync(f => f.Amount);
        }

        // =============================
        // LIST INVOICES (SAFE)
        // =============================
        public async Task<List<FeeInvoice>> GetAllAsync()
        {
            return await _context.FeeInvoices
                .AsNoTracking()
                .OrderByDescending(f => f.FeeInvoiceId)
                .ToListAsync();
        }

        // =============================
        // ADD INVOICE
        // =============================
        public async Task AddInvoiceAsync(FeeInvoice invoice)
        {
            await _context.FeeInvoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
        }

        // =============================
        // MARK AS PAID
        // =============================
        public async Task MarkPaidAsync(int invoiceId, string paymentMode)
        {
            var invoice = await _context.FeeInvoices
                .FirstOrDefaultAsync(x => x.FeeInvoiceId == invoiceId);

            if (invoice == null)
                return;

            invoice.IsPaid = true;
            invoice.PaidOn = DateTime.Today;
            invoice.PaymentMode = paymentMode;

            await _context.SaveChangesAsync();
        }
    }
}
