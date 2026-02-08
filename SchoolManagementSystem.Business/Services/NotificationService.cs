using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models.Models;

namespace SchoolManagementSystem.Business.Services
{
    public class NotificationService :INotificationService
    {
        private readonly SchoolDbContext _context;

        // ✅ Constructor Injection
        public NotificationService(SchoolDbContext context)
        {
            _context = context;
        }

        // ================= GET UNREAD =================

        public async Task<List<NotificationDto>> GetUnreadAsync(int userId)
        {
            if (userId <= 0)
                return new List<NotificationDto>();

            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedOn)
                .ToListAsync();
        }

        // ================= MARK AS READ =================

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId);

            if (notification == null)
                return;

            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
        public async Task SendSmsAsync(string mobile, string msg)
        {
            var client = new HttpClient();
            await client.GetAsync($"https://sms-api/send?to={mobile}&msg={msg}");
        }

        public Task SendWhatsAppAsync(string mobile, string message)
        {
            throw new NotImplementedException();
        }
    }
}
