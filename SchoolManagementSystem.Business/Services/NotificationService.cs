using SchoolManagementSystem.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagementSystem.Models.Models;

namespace SchoolManagementSystem.Business.Services
{
    public class NotificationService
    {
        public List<Notification> GetUnread(int userId)
        {
            using var ctx = new SchoolDbContext();
            return ctx.Notifications
                      .Where(x => x.UserId == userId && !x.IsRead)
                      .ToList();
        }
    }

}
