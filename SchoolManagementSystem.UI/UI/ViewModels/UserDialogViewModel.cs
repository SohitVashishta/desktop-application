using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using SchoolManagementSystem.UI.UI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

public class UserDialogViewModel : SchoolManagementSystem.UI.UI.Helpers.BaseViewModel
{
    private readonly IUserService _userService;

    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }

    public ICommand SaveCommand { get; }

    public event Func<Task> UserSaved;

    public UserDialogViewModel(IUserService userService)
    {
        _userService = userService;
        SaveCommand = new RelayCommand(async () => await SaveAsync());
    }

    private async Task SaveAsync()
    {
        var user = new User
        {
            Username = Username,
            Email = Email,
            Role = Role
        };

        await _userService.CreateUserAsync(user, Password);

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
