using MaterialDesignThemes.Wpf;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Common.Enums;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;



namespace SchoolManagementSystem.UI.UI.ViewModels.Admin
{
    public class AssignRolesViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        public SnackbarMessageQueue SnackbarMessageQueue { get; }
            = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

        public AssignRolesViewModel(IUserService userService)
        {
            _userService = userService;

            SaveAccessCommand = new RelayCommand(
                async () => await SaveAsync(),
                () => SelectedUser != null && !IsSaving
            );
        }

        public async Task LoadOnStartupAsync()
        {
            await LoadUsersAsync();
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
                ((RelayCommand)SaveAccessCommand).RaiseCanExecuteChanged();
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

        // ================= ROLES =================

        public ObservableCollection<UserRole> Roles { get; } =
            new(Enum.GetValues(typeof(UserRole)).Cast<UserRole>());

        // ================= COMMANDS =================

        public ICommand SaveAccessCommand { get; }

        // ================= DATA =================

        private bool _isLoading;

        private async Task LoadUsersAsync()
        {
            if (_isLoading) return;

            try
            {
                _isLoading = true;

                Users.Clear();
                FilteredUsers.Clear();

                var users = await _userService.GetAllUsersAsync();

                foreach (var user in users)
                    Users.Add(user);

                ApplyFilter();
            }
            finally
            {
                _isLoading = false;
            }
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

            foreach (var user in query)
                FilteredUsers.Add(user);

            OnPropertyChanged(nameof(IsUserListEmpty));
        }


        // ================= ACTIONS =================
        private bool _isSaving;
        public bool IsSaving
        {
            get => _isSaving;
            set
            {
                _isSaving = value;
                OnPropertyChanged();
                ((RelayCommand)SaveAccessCommand).RaiseCanExecuteChanged();
            }
        }

        private async Task SaveAsync()
        {
            if (SelectedUser == null)
                return;

            try
            {
                IsSaving = true;

                await _userService.UpdateUserAsync(SelectedUser);

                SnackbarMessageQueue.Enqueue("✔ Access updated successfully");
            }
            catch
            {
                SnackbarMessageQueue.Enqueue("❌ Failed to update access");
            }
            finally
            {
                IsSaving = false;
            }
        }


    }
}
