using SchoolManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface IFinanceService
    {
        // Dashboard
        Task<decimal> GetPendingFeesAmountAsync();

        // List
        Task<List<FeeInvoice>> GetInvoicesAsync();

        // Add
        Task AddInvoiceAsync(FeeInvoice invoice);

        // Pay
        Task MarkInvoicePaidAsync(int invoiceId, string paymentMode);
    }
}
