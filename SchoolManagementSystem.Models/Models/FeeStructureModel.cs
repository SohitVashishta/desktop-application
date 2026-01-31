using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class FeeStructureModel
    {
        public string FeeHeadName { get; set; } // FeeHeadName
        public int FeeStructureId { get; set; }
        public int AcademicYearId { get; set; }
        public int ClassId { get; set; }
        public int FeeHeadId { get; set; }
        public decimal Amount { get; set; }
        public ObservableCollection<FeeStructureDetailModel> Details { get; set; }
            = new();
    }
}
