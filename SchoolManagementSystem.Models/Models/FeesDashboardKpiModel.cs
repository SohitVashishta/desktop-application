using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeesDashboardKpiModel
    {
        public int TotalStudents { get; set; }
        public decimal TotalFees { get; set; }
        public decimal CollectedFees { get; set; }
        public decimal PendingFees { get; set; }
    }

    public class MonthlyCollectionModel
    {
        public string MonthName { get; set; }
        public decimal CollectedAmount { get; set; }
    }

    public class ClassCollectionModel
    {
        public string ClassName { get; set; }
        public decimal CollectedFees { get; set; }
    }

}
