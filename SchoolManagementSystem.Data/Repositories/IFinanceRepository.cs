using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface IFinanceRepository
    {
        Task<decimal> GetPendingAmountAsync();
        Task<List<FeeInvoice>> GetAllAsync();
        Task AddInvoiceAsync(FeeInvoice invoice);
        Task MarkPaidAsync(int invoiceId, string paymentMode);
    }
}
