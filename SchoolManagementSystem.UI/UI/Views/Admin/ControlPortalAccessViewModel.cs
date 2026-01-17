using MaterialDesignThemes.Wpf;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.Admin
{
    public class ControlPortalAccessViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        public SnackbarMessageQueue SnackbarMessageQueue { get; }
            = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

        public ControlPortalAccessViewModel(IUserService userService)
        {
            _userService = userService;

            ToggleAccessCommand = new RelayCommand<User>(
                async u => await ToggleAccessAsync(u)
            );
        }

        public async Task LoadOnStartupAsync()
        {
            Users.Clear();
            var users = await _userService.GetAllUsersAsync();

            foreach (var u in users)
                Users.Add(u);

            ApplyFilter();
        }

        // ================= USERS =================
        public ObservableCollection<User> Users { get; } = new();
        public ObservableCollection<User> FilteredUsers { get; } = new();

        // ================= COMMAND =================
        public ICommand ToggleAccessCommand { get; }

        // ================= FILTER =================
        private void ApplyFilter()
        {
            FilteredUsers.Clear();
            foreach (var u in Users)
                FilteredUsers.Add(u);
        }

        // ================= ACTION =================
        private async Task ToggleAccessAsync(User user)
        {
            try
            {
                user.IsActive = !user.IsActive;
                await _userService.UpdateUserAsync(user);

                SnackbarMessageQueue.Enqueue(
                    user.IsActive
                        ? "✔ Portal access enabled"
                        : "❌ Portal access disabled"
                );
            }
            catch
            {
                SnackbarMessageQueue.Enqueue("❌ Failed to update access");
            }
        }
    }
}
