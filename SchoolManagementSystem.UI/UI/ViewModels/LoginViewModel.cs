using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Common.Session;
using SchoolManagementSystem.UI.UI.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly Window _window;
        private readonly AuthService _authService = new();

        private string _username = "";
        private string _errorMessage = "";

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public LoginViewModel(Window window)
        {
            _window = window;
        }

        public void Login(string password)
        {
            ErrorMessage = "";

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Username and password are required";
                return;
            }

            var user = _authService.Login(Username, password);

            if (user == null)
            {
                ErrorMessage = "Invalid username or password";
                return;
            }

            // ✅ SET SESSION (CRITICAL FOR ROLE-BASED UI)
            UserSession.UserId = user.UserId;
            UserSession.Username = user.Username;
            UserSession.Role = user.Role;

            // ✅ SUCCESS MESSAGE
            MessageBox.Show(
                $"Welcome {user.Username}!",
                "Login Successful",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );

            // ✅ OPEN MAIN WINDOW
            new MainWindow().Show();
            _window.Close();
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
