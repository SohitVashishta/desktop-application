using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using SchoolManagementSystem.UI.UI.ViewModels;
using SchoolManagementSystem.UI.UI.Views.Admin;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace SchoolManagementSystem.UI.UI.ViewModels.Admin
{
    public class UserManagementViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        public ObservableCollection<User> Users { get; } = new();
        public ICommand AddUserCommand { get; }

        public UserManagementViewModel(IUserService userService)
        {
            _userService = userService;

            AddUserCommand = new RelayCommand(async () => await OpenAddUserDialogAsync());

            _ = LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            Users.Clear();

            var users = await _userService.GetAllUsersAsync();

            foreach (var user in users)
                Users.Add(user);
        }

        private async Task OpenAddUserDialogAsync()
        {
            var vm = new AddUserViewModel(_userService);
            vm.UserSaved += async () => await LoadUsersAsync();

            var dialog = new UserDialog
            {
                DataContext = vm
            };

            dialog.ShowDialog();
        }
    }

}
