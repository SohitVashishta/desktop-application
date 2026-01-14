using SchoolManagementSystem.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class LoginResult
    {
        public int UserId { get; set; }
        public UserRole Role { get; set; }
        public string UserName { get; set; }
        public int? StudentId { get; set; }   // for Student/Parent
        public int? TeacherId { get; set; }
    }

}
