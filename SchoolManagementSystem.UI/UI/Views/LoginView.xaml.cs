using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Common.Enums;
using SchoolManagementSystem.Common.Session;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Models.Models;
using System.Windows;

namespace SchoolManagementSystem.UI.UI.Views
{
    public partial class LoginView : Window
    {
        private readonly AuthService _authService = new();

        public LoginView()
        {
            InitializeComponent();
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            txtError.Visibility = Visibility.Collapsed;

            var username = txtUsername.Text.Trim();
            var password = pwdBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                txtError.Text = "Username and password are required";
                txtError.Visibility = Visibility.Visible;
                return;
            }

            var user = _authService.Login(username, password);

            if (user == null)
            {
                txtError.Text = "Invalid username or password";
                txtError.Visibility = Visibility.Visible;
                return;
            }

            // ✅ SET SESSION (CRITICAL)
            UserSession.UserId = user.UserId;
            UserSession.Username = user.Username;
            UserSession.Role = user.Role; // Admin / Teacher / Student

            // ✅ OPEN MAIN WINDOW
            var main = new MainWindow();
            main.Show();

            Close();
        }
        

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Please contact the system administrator to reset your password.",
                "Forgot Password",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

    }
}
