using SchoolManagementSystem.Common.Enums;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.Admin
{
    public class AddUserViewModel : BaseViewModel
    {
        // ================= FIELDS =================
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Password provided by VIEW (PasswordBox)
        public Func<string>? PasswordProvider { get; set; }

        // ================= ROLES =================
        public ObservableCollection<UserRole> Roles { get; } =
            new ObservableCollection<UserRole>(
                Enum.GetValues(typeof(UserRole)).Cast<UserRole>());

        private UserRole _selectedRole;
        public UserRole SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
            }
        }

        // ================= COMMANDS =================
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        // ================= EVENTS =================
        public event Action<User>? UserSaved;

        public AddUserViewModel()
        {
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Close);
        }

        // ================= SAVE =================
        private void Save()
        {
            var password = PasswordProvider?.Invoke();

            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Username and Password are required",
                    "Validation",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var user = new User
            {
                Username = Username.Trim(),
                Email = Email?.Trim(),
                Role = SelectedRole,
                IsActive = true,
                PasswordHash = HashPassword(password)
            };

            UserSaved?.Invoke(user);
            Close();
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(
                sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        private void Close()
        {
            Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.IsActive)
                ?.Close();
        }
    }
}
