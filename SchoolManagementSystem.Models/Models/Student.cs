using System;

namespace SchoolManagementSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }   // PK
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
