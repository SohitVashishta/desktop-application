using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Models
{
    [Table("TeacherMaster")]
    public class TeacherMaster
    {
        [Key]
        public int TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Subject { get; set; } = "";
        public bool IsActive { get; set; }
        public DateTime ? CreatedDate { get; set; } = DateTime.Now; //CreatedDate
    }
}
