using SchoolManagementSystem.Common.Enums;

namespace SchoolManagementSystem.Common.Session
{
    public static class UserSession
    {
        public static int UserId { get; set; }
        public static string Username { get; set; } = "";
        public static UserRole Role { get; set; }

        public static bool IsAdmin => Role == UserRole.Admin;
        public static bool IsTeacher => Role == UserRole.Teacher;
        public static bool IsStudent => Role == UserRole.Student;
        public static bool IsParent => Role == UserRole.Parent;
        public static bool IsAccountant => Role == UserRole.Accountant;
        public static bool IsLibrarian => Role == UserRole.Librarian;
        public static bool IsHR => Role == UserRole.HR;
        public static void Start(int userId, string username, UserRole role)
        {
            UserId = userId;
            Username = username;
            Role = role;
        }

        public static void End()
        {
            UserId = 0;
            Username = null;
        }
    }
}
