using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public interface ICommunicationRepository
    {
        Task<List<NotificationDto>> GetNotificationsAsync();
        Task AddNotificationAsync(NotificationDto notification);
    }
}
