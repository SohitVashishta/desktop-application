using SchoolManagementSystem.Data.Repositories;
using SchoolManagementSystem.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Business.Services
{
    public class CommunicationService : ICommunicationService
    {
        private readonly ICommunicationRepository _repository
            = new CommunicationRepository();

        public Task<List<NotificationDto>> GetAllAsync()
            => _repository.GetNotificationsAsync();

        public Task SendAsync(NotificationDto notification)
            => _repository.AddNotificationAsync(notification);
    }
}
