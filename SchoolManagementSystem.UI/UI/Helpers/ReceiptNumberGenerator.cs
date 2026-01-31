using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.UI.UI.Helpers
{
    public static class ReceiptNumberGenerator
    {
        public static string Generate(int nextId)
        {
            return $"RCPT-{DateTime.Now.Year}-{nextId:D6}";
        }
    }

}
