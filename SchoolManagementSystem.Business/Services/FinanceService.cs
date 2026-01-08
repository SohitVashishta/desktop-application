using System.Collections.Generic;
using System.Linq;
using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models;

namespace SchoolManagementSystem.Business.Services
{
    public class FinanceService
    {
        private readonly FinanceRepository _repo = new();

        // DASHBOARD
        public decimal GetPendingFeesAmount()
            => _repo.GetPendingAmount();

        // LIST
        public List<FeeInvoice> GetInvoices()
            => _repo.GetAll().ToList();

        // ADD
        public void AddInvoice(FeeInvoice invoice)
            => _repo.Add(invoice);

        // PAY
        public void MarkInvoicePaid(int invoiceId, string paymentMode)
            => _repo.MarkPaid(invoiceId, paymentMode);
    }
}
