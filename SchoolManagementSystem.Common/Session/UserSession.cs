using SchoolManagementSystem.Common.Enums;

namespace SchoolManagementSystem.Common.Session
{
    public static class UserSession
    {
        public static int UserId { get; set; }
        public static string Username { get; set; } = "";
        public static string Role { get; set; } = ""; // Admin / Teacher / Student

        public static bool IsAdmin => Role == "Admin";
        public static bool IsTeacher => Role == "Teacher";
        public static bool IsStudent => Role == "Student";

        public static void Clear()
        {
            UserId = 0;
            Username = "";
            Role = "";
        }
    }
}
