using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.UI.UI.Helpers;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels
{
    public class ResetPasswordViewModel : BaseViewModel
    {
        private readonly IUserService _service;

        private string _newPassword = string.Empty;

        public int UserId { get; }

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged();
            }
        }

        public ICommand ResetCommand { get; }

        public ResetPasswordViewModel(int userId, IUserService service)
        {
            
            UserId = userId;

            // ✅ RelayCommand expects Func<object, Task>
            ResetCommand = new RelayCommand( ResetAsync);
            _service = service;
        }

        private async Task ResetAsync()
        {
            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                MessageBox.Show("Password cannot be empty");
                return;
            }

            await _service.ResetPasswordAsync(UserId, NewPassword);

            MessageBox.Show("Password reset successfully");
            NewPassword = string.Empty;
        }
    }
}
