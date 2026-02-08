using SchoolManagementSystem.Common.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string? PasswordSalt { get; set; }      // ✅ nullable
        public string? FullName { get; set; }           // ✅ nullable
        public string? MobileNo { get; set; }           // ✅ nullable

        public bool IsActive { get; set; }

        public DateTime? LastLoginOn { get; set; }      // ✅ MUST be nullable

        public string? CreatedBy { get; set; }          // ✅ nullable

        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.None;

        // UI helpers
        public string StatusText => IsActive ? "Active" : "Inactive";
        public string ToggleText => IsActive ? "Deactivate" : "Activate";
    }

}
