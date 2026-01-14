using SchoolManagementSystem.Business.Auth;
using SchoolManagementSystem.UI.UI.Helpers;
using SchoolManagementSystem.UI.UI.Views;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(async () => await LoginAsync());
        }

        // ================== PROPERTIES ==================

        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        private bool _hasError;
        public bool HasError
        {
            get => _hasError;
            set { _hasError = value; OnPropertyChanged(); }
        }

        // ================== COMMAND ==================

        public ICommand LoginCommand { get; }

        // ================== LOGIC ==================

        private async Task LoginAsync()
        {
            HasError = false;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password))
            {
                ShowError("Username and password are required.");
                return;
            }

            var success = await _authService.LoginAsync(Username, Password);

            if (success!=null)
            {
                ShowError("Invalid username or password.");
                return;
            }

            // 🔐 LOGIN SUCCESS → OPEN MAIN WINDOW
            Application.Current.Dispatcher.Invoke(() =>
            {
                var main = new MainWindow();
                main.Show();

                foreach (Window w in Application.Current.Windows)
                {
                    if (w is Views.LoginView)
                    {
                        w.Close();
                        break;
                    }
                }
            });
        }

        private void ShowError(string message)
        {
            ErrorMessage = message;
            HasError = true;
        }
    }
}
