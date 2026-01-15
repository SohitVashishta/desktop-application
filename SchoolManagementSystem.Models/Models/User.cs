using SchoolManagementSystem.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.None; // ✅ FIX

        public bool IsActive { get; set; }

        // ===== UI HELPERS =====
        public string StatusText => IsActive ? "Active" : "Inactive";
        public string ToggleText => IsActive ? "Deactivate" : "Activate";
    }
}
