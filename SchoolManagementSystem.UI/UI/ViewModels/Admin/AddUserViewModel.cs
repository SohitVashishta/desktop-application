using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.Admin
{
    public class AddUserViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ObservableCollection<string> Roles { get; }
        public string SelectedRole { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Func<Task> UserSaved;

        public AddUserViewModel(IUserService userService)
        {
            _userService = userService;

            Roles = new ObservableCollection<string>
            {
                "Admin",
                "Teacher",
                "Student"
            };

            SaveCommand = new RelayCommand(async () => await SaveAsync());
            CancelCommand = new RelayCommand(Close);
        }

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(SelectedRole))
            {
                MessageBox.Show("All fields are required");
                return;
            }

            var user = new User
            {
                Username = Username,
                Email = Email,
                Role = SelectedRole
            };

            // ✅ SAVE TO DATABASE
            await _userService.CreateUserAsync(user, Password);

            // ✅ Notify parent to reload grid
            if (UserSaved != null)
                await UserSaved.Invoke();

            Close();
        }

        private void Close()
        {
            Application.Current.Windows
                .OfType<Window>()
                .Single(w => w.IsActive)
                .Close();
        }
    }
}
