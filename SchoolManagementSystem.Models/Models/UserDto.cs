using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public string PasswordHash { get; set; }
    }
}
