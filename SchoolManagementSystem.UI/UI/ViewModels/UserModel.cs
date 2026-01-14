using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class UserModel : BaseViewModel
    {
        public string Username { get; }
        public string Email { get; }
        public string Role { get; }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set { _isActive = value; OnPropertyChanged(); OnPropertyChanged(nameof(StatusText)); }
        }

        public string StatusText => IsActive ? "Active" : "Inactive";
        public string ToggleText => IsActive ? "Deactivate" : "Activate";
        public Brush StatusColor => IsActive ? Brushes.Green : Brushes.Red;

        public UserModel(string username, string email, string role, bool isActive)
        {
            Username = username;
            Email = email;
            Role = role;
            IsActive = isActive;
        }
    }

}
