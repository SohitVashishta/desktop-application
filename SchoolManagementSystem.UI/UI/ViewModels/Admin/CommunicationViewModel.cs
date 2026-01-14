using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.Admin
{
    public class CommunicationViewModel : BaseViewModel
    {
        public ObservableCollection<NotificationDto> Notifications { get; }
            = new ObservableCollection<NotificationDto>();

        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        private string _message;
        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(); }
        }

        private string _type = "Announcement";
        public string Type
        {
            get => _type;
            set { _type = value; OnPropertyChanged(); }
        }

        public ICommand SendCommand { get; }

        public CommunicationViewModel()
        {
            SendCommand = new RelayCommand(Send);
        }

        private void Send()
        {
            if (string.IsNullOrWhiteSpace(Title) ||
                string.IsNullOrWhiteSpace(Message))
                return;

            Notifications.Insert(0, new NotificationDto
            {
                Title = Title,
                Message = Message,
                Type = Type,
                TargetAudience = "All",
                CreatedOn = DateTime.Now.ToString("dd-MMM-yyyy HH:mm")
            });

            Title = string.Empty;
            Message = string.Empty;
        }
    }
}
