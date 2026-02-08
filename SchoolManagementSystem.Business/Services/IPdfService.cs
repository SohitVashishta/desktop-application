using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public interface IPdfService
    {
        void GenerateReceipt(FeeReceiptModel model, string filePath);
        void GenerateReport<T>(List<T> data, string title, string filePath);
    }

}
