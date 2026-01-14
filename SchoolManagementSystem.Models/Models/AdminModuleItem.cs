using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class AdminModuleItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }   // Segoe MDL2 icon
        public string ModuleKey { get; set; } // Navigation key
    }
}
