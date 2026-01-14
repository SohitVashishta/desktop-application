using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class AssetDto
    {
        public int AssetId { get; set; }
        public string AssetName { get; set; }
        public string Category { get; set; } // Book, Lab, Furniture, IT
        public string Location { get; set; }
        public string Status { get; set; }   // Active, Maintenance, Retired
        public decimal PurchaseCost { get; set; }
        public string PurchaseDate { get; set; }
    }
}
