using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Models.Models
{
    public class ClassModel
    {
        public int ClassId { get; set; }

        public string ClassName { get; set; }

        public int GradeId { get; set; }
        public string GradeName { get; set; }   // from JOIN (not stored)

        public string Section { get; set; }

        public string RoomNumber { get; set; }

        public int MaxStudents { get; set; }

        public int? LeadTeacherId { get; set; }
        public string LeadTeacherName { get; set; }   // from JOIN

        public int? AssistantTeacherId { get; set; }
        public string AssistantTeacherName { get; set; } // from JOIN

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Used for optimistic concurrency
        /// </summary>
        public byte[] RowVersion { get; set; }

        /* ================= UI HELPERS ================= */
        public string Status => IsActive ? "Active" : "Inactive";
    }
}
