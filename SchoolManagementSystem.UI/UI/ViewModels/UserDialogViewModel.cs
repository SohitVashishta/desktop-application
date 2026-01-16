using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Common.Enums;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using SchoolManagementSystem.UI.UI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

public class UserDialogViewModel : BaseViewModel
{
    private readonly IUserService _userService;

    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";

    public bool IsActive { get; set; } = true;

    public IEnumerable<UserRole> Roles { get; }
    public UserRole SelectedRole { get; set; } = UserRole.None;

    public ICommand SaveCommand { get; }
    public event Func<Task>? UserSaved;

    public UserDialogViewModel(IUserService userService)
    {
        _userService = userService;

        Roles = Enum.GetValues(typeof(UserRole))
                    .Cast<UserRole>()
                    .Where(r => r != UserRole.None)
                    .ToList();

        SaveCommand = new RelayCommand(async () => await SaveAsync());
    }

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Username) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password) ||
            SelectedRole == UserRole.None)
        {
            MessageBox.Show("All fields are required");
            return;
        }

        var user = new User
        {
            Username = Username,
            Email = Email,
            Role = SelectedRole,
            IsActive = IsActive   // ✅ FROM UI
        };

        await _userService.CreateUserAsync(user, Password);

        if (UserSaved != null)
            await UserSaved.Invoke();

        CloseWindow();
    }

    private void CloseWindow()
    {
        Application.Current.Windows
            .OfType<Window>()
            .Single(w => w.IsActive)
            .Close();
    }
}

