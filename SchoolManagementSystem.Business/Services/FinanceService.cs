using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly IFinanceRepository _repository;

        // ✅ Constructor Injection via INTERFACE
        public FinanceService(IFinanceRepository repository)
        {
            _repository = repository;
        }

        // ================= DASHBOARD =================

        public async Task<decimal> GetPendingFeesAmountAsync()
        {
            return await _repository.GetPendingAmountAsync();
        }

        // ================= LIST =================

        public async Task<List<FeeInvoice>> GetInvoicesAsync()
        {
            return await _repository.GetAllAsync();
        }

        // ================= ADD =================

        public async Task AddInvoiceAsync(FeeInvoice invoice)
        {
            if (invoice == null)
                return;

            await _repository.AddInvoiceAsync(invoice);
        }

        // ================= PAY =================

        public async Task MarkInvoicePaidAsync(int invoiceId, string paymentMode)
        {
            if (invoiceId <= 0 || string.IsNullOrWhiteSpace(paymentMode))
                return;

            await _repository.MarkPaidAsync(invoiceId, paymentMode);
        }
    }
}
