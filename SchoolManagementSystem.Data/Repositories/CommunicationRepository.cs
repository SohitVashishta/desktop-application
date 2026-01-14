using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Data.Repositories
{
    public class CommunicationRepository : ICommunicationRepository
    {
        private readonly List<NotificationDto> _store = new();

        public Task<List<NotificationDto>> GetNotificationsAsync()
            => Task.FromResult(_store);

        public Task AddNotificationAsync(NotificationDto notification)
        {
            notification.Id = _store.Count + 1;
            notification.CreatedOn = System.DateTime.Now.ToString("dd-MMM-yyyy HH:mm");
            _store.Add(notification);
            return Task.CompletedTask;
        }
    }
}
