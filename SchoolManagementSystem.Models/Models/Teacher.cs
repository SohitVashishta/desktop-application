namespace SchoolManagementSystem.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Subject { get; set; } = "";
        public DateTime ? CreatedDate { get; set; } = DateTime.Now; //CreatedDate
    }
}
