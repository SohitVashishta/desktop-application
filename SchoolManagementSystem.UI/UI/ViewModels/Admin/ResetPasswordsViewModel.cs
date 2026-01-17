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
    public class ResetPasswordsViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        public SnackbarMessageQueue SnackbarMessageQueue { get; }
            = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

        public ResetPasswordsViewModel(IUserService userService)
        {
            _userService = userService;

            OpenResetDialogCommand = new RelayCommand(
                () => IsResetDialogOpen = true,
                () => SelectedUser != null && !IsSaving
            );

            ConfirmResetCommand = new RelayCommand(
                async () => await ResetPasswordAsync(),
                () => SelectedUser != null && !IsSaving
            );

            CancelResetCommand = new RelayCommand(() =>
            {
                IsResetDialogOpen = false;
            });
        }

        // ================= USERS =================

        public ObservableCollection<User> Users { get; } = new();
        public ObservableCollection<User> FilteredUsers { get; } = new();

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
                RaiseCommands();
            }
        }

        // ================= SEARCH =================

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public bool IsUserListEmpty => !FilteredUsers.Any();

        // ================= DIALOG =================

        private bool _isResetDialogOpen;
        public bool IsResetDialogOpen
        {
            get => _isResetDialogOpen;
            set
            {
                _isResetDialogOpen = value;
                OnPropertyChanged();
            }
        }

        // ================= STATE =================

        private bool _isSaving;
        public bool IsSaving
        {
            get => _isSaving;
            set
            {
                _isSaving = value;
                OnPropertyChanged();
                RaiseCommands();
            }
        }

        // ================= COMMANDS =================

        public ICommand OpenResetDialogCommand { get; }
        public ICommand ConfirmResetCommand { get; }
        public ICommand CancelResetCommand { get; }

        private void RaiseCommands()
        {
            ((RelayCommand)OpenResetDialogCommand).RaiseCanExecuteChanged();
            ((RelayCommand)ConfirmResetCommand).RaiseCanExecuteChanged();
        }

        // ================= DATA =================

        public async Task LoadOnStartupAsync()
        {
            Users.Clear();
            FilteredUsers.Clear();

            var users = await _userService.GetAllUsersAsync();
            foreach (var u in users)
                Users.Add(u);

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            FilteredUsers.Clear();

            var query = Users.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(u =>
                    u.Username.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            foreach (var u in query)
                FilteredUsers.Add(u);

            OnPropertyChanged(nameof(IsUserListEmpty));
        }

        // ================= ACTION =================

        private async Task ResetPasswordAsync()
        {
            if (SelectedUser == null)
                return;

            try
            {
                IsSaving = true;

                await _userService.ResetPasswordAsync(SelectedUser.UserId, SelectedUser.PasswordHash);

                SnackbarMessageQueue.Enqueue("✔ Password reset successfully");
            }
            catch
            {
                SnackbarMessageQueue.Enqueue("❌ Failed to reset password");
            }
            finally
            {
                IsSaving = false;
                IsResetDialogOpen = false;
            }
        }
    }
}
