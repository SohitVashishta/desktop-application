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
    public class UserManagementViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        // ================= UI STATE =================
        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set { _isDialogOpen = value; OnPropertyChanged(); }
        }

        private bool _isSaving;
        public bool IsSaving
        {
            get => _isSaving;
            set
            {
                _isSaving = value;
                OnPropertyChanged();
                ((RelayCommand)SaveUserCommand).RaiseCanExecuteChanged();
            }
        }

        // ================= SNACKBAR =================
        public SnackbarMessageQueue SnackbarMessageQueue { get; } = new();

        // ================= USERS =================
        public ObservableCollection<User> Users { get; } = new();
        public ObservableCollection<User> FilteredUsers { get; } = new();

        // ================= DIALOG =================
        private User _editUser;
        public User EditUser
        {
            get => _editUser;
            set { _editUser = value; OnPropertyChanged(); }
        }

        private string _dialogTitle;
        public string DialogTitle
        {
            get => _dialogTitle;
            set { _dialogTitle = value; OnPropertyChanged(); }
        }

        // ================= SEARCH =================
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                CurrentPage = 1;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        // ================= STATUS FILTER =================
        private string _selectedStatus = "All";
        public string SelectedStatus
        {
            get => _selectedStatus;
            set
            {
                _selectedStatus = value;
                CurrentPage = 1;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        // ================= ROLES =================
        public ObservableCollection<UserRole> Roles { get; } =
            new(Enum.GetValues(typeof(UserRole)).Cast<UserRole>());

        private UserRole? _selectedRole;
        public UserRole? SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                CurrentPage = 1;
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        // ================= PAGINATION =================
        public int PageSize { get; } = 10;

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set { _currentPage = value; OnPropertyChanged(); }
        }

        public string PageInfo => $"Page {CurrentPage}";
        public bool CanGoPrevious => CurrentPage > 1;
        public bool CanGoNext => Users.Count > CurrentPage * PageSize;

        // ================= COMMANDS =================
        public ICommand RefreshCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand SaveUserCommand { get; }
        public ICommand CancelDialogCommand { get; }
        public ICommand ToggleStatusCommand { get; }
        public ICommand ResetFiltersCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }

        // ================= CONSTRUCTOR =================
        public UserManagementViewModel(IUserService userService)
        {
            _userService = userService;

            RefreshCommand = new RelayCommand(async () => await LoadUsersAsync());
            AddUserCommand = new RelayCommand(OpenAddDialog);
            EditUserCommand = new RelayCommand<User>(OpenEditDialog);
            SaveUserCommand = new RelayCommand(async () => await SaveAsync(), () => !IsSaving);
            CancelDialogCommand = new RelayCommand(() => IsDialogOpen = false);
            ToggleStatusCommand = new RelayCommand<User>(async u => await ToggleStatusAsync(u));

            ResetFiltersCommand = new RelayCommand(() =>
            {
                SearchText = string.Empty;
                SelectedRole = null;
                SelectedStatus = "All";
            });

            NextPageCommand = new RelayCommand(() =>
            {
                CurrentPage++;
                ApplyFilters();
            }, () => CanGoNext);

            PrevPageCommand = new RelayCommand(() =>
            {
                CurrentPage--;
                ApplyFilters();
            }, () => CanGoPrevious);
        }

        // ================= LOAD =================
        public async Task LoadOnStartupAsync()
        {
            await LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            Users.Clear();
            FilteredUsers.Clear();

            var users = await _userService.GetAllUsersAsync();

            foreach (var u in users)
                Users.Add(u);

            ApplyFilters();
        }

        // ================= FILTER =================
        private void ApplyFilters()
        {
            FilteredUsers.Clear();

            var query = Users.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(u =>
                    u.Username.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

            if (SelectedRole.HasValue)
                query = query.Where(u => u.Role == SelectedRole.Value);

            if (SelectedStatus == "Active")
                query = query.Where(u => u.IsActive);
            else if (SelectedStatus == "Inactive")
                query = query.Where(u => !u.IsActive);

            query = query
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize);

            foreach (var u in query)
                FilteredUsers.Add(u);

            OnPropertyChanged(nameof(PageInfo));
            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
        }

        // ================= ADD / EDIT =================
        private void OpenAddDialog()
        {
            DialogTitle = "Add User";
            EditUser = new User { IsActive = true };
            IsDialogOpen = true;
        }

        private void OpenEditDialog(User user)
        {
            DialogTitle = "Edit User";

            // Clone user to avoid instant grid mutation
            EditUser = new User
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                PasswordHash = user.PasswordHash
            };

            IsDialogOpen = true;
        }

        // ================= SAVE =================
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(EditUser.Username))
            {
                SnackbarMessageQueue.Enqueue("Username is required");
                return;
            }

            try
            {
                IsSaving = true;

                if (EditUser.UserId == 0)
                    await _userService.CreateUserAsync(EditUser, EditUser.PasswordHash);
                else
                    await _userService.UpdateUserAsync(EditUser);

                SnackbarMessageQueue.Enqueue("User saved successfully");

                IsDialogOpen = false;
                await LoadUsersAsync();
            }
            catch
            {
                SnackbarMessageQueue.Enqueue("Failed to save user");
            }
            finally
            {
                IsSaving = false;
            }
        }

        // ================= STATUS =================
        private async Task ToggleStatusAsync(User user)
        {
            user.IsActive = !user.IsActive;
            await _userService.UpdateUserAsync(user);
            SnackbarMessageQueue.Enqueue("User status updated");
        }
    }
}
