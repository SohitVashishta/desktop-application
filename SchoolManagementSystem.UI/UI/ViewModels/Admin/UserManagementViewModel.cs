using MaterialDesignThemes.Wpf;
using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Common.Enums;
using SchoolManagementSystem.Common.Security;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace SchoolManagementSystem.UI.UI.ViewModels.Admin
{
    /// <summary>
    /// ViewModel responsible for:
    /// - User listing (filter, pagination)
    /// - Add / Edit user dialog
    /// - Password validation (Add mode only)
    /// </summary>
    public class UserManagementViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        #region ================= UI STATE =================

        private bool _isDialogOpen;
        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set
            {
                // When dialog closes, always clear sensitive state
                if (_isDialogOpen && !value)
                    ResetDialogState();

                _isDialogOpen = value;
                OnPropertyChanged();
            }
        }

        private bool _isSaving;
        public bool IsSaving
        {
            get => _isSaving;
            set
            {
                _isSaving = value;
                OnPropertyChanged();
                NotifyCanSaveChanged();
            }
        }

        #endregion

        #region ================= ADD / EDIT MODE =================

        private bool _isAddMode;
        /// <summary>
        /// True when dialog is opened via "Add User"
        /// False when editing an existing user
        /// </summary>
        public bool IsAddMode
        {
            get => _isAddMode;
            set
            {
                _isAddMode = value;
                OnPropertyChanged();
                NotifyCanSaveChanged();
            }
        }

        #endregion

        #region ================= PASSWORD STATE (ADD MODE ONLY) =================

        /// <summary>
        /// Plain password entered by the user (never stored in User entity)
        /// </summary>
        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                ValidatePasswords();
            }
        }

        private string _confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
                ValidatePasswords();
            }
        }

        /// <summary>
        /// Whether password eye (show/hide) is enabled
        /// </summary>
        private bool _showPassword;
        public bool ShowPassword
        {
            get => _showPassword;
            set
            {
                _showPassword = value;
                OnPropertyChanged();
            }
        }

        private int _passwordStrengthValue;
        public int PasswordStrengthValue
        {
            get => _passwordStrengthValue;
            set
            {
                _passwordStrengthValue = value;
                OnPropertyChanged();
            }
        }

        private string _passwordStrength;
        public string PasswordStrength
        {
            get => _passwordStrength;
            set
            {
                _passwordStrength = value;
                OnPropertyChanged();
            }
        }

        private bool _isPasswordStrong;
        public bool IsPasswordStrong
        {
            get => _isPasswordStrong;
            set
            {
                _isPasswordStrong = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region ================= SAVE ENABLE LOGIC =================

        /// <summary>
        /// Determines whether Save button should be enabled
        /// </summary>
        public bool CanSave
        {
            get
            {
                if (IsSaving || EditUser == null)
                    return false;

                if (string.IsNullOrWhiteSpace(EditUser.Username) ||
                    string.IsNullOrWhiteSpace(EditUser.Email))
                    return false;

                // Password rules apply only when adding a user
                if (IsAddMode)
                {
                    return
                        !string.IsNullOrWhiteSpace(Password) &&
                        Password == ConfirmPassword &&
                        IsPasswordStrong;
                }

                return true; // Edit mode
            }
        }

        private void NotifyCanSaveChanged()
        {
            OnPropertyChanged(nameof(CanSave));
            ((RelayCommand)SaveUserCommand).RaiseCanExecuteChanged();
        }

        #endregion

        #region ================= USERS & FILTERING =================

        public ObservableCollection<User> Users { get; } = new();
        public ObservableCollection<User> FilteredUsers { get; } = new();

        private User _editUser;
        public User EditUser
        {
            get => _editUser;
            set
            {
                _editUser = value;
                OnPropertyChanged();
                NotifyCanSaveChanged();
            }
        }

        private string _dialogTitle;
        public string DialogTitle
        {
            get => _dialogTitle;
            set
            {
                _dialogTitle = value;
                OnPropertyChanged();
            }
        }

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

        public ObservableCollection<string> StatusOptions { get; } =
     new ObservableCollection<string> { "All", "Active", "Inactive" };

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

        #endregion

        #region ================= PAGINATION =================

        public int PageSize { get; } = 10;

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }

        public string PageInfo => $"Page {CurrentPage}";
        public bool CanGoPrevious => CurrentPage > 1;
        public bool CanGoNext => Users.Count > CurrentPage * PageSize;

        #endregion

        #region ================= COMMANDS =================

        public ICommand RefreshCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand EditUserCommand { get; }
        public ICommand SaveUserCommand { get; }
        public ICommand CancelDialogCommand { get; }
        public ICommand ToggleStatusCommand { get; }
        public ICommand ResetFiltersCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }

        #endregion

        #region ================= SNACKBAR =================

        public SnackbarMessageQueue SnackbarMessageQueue { get; } = new();

        #endregion

        #region ================= CONSTRUCTOR =================

        public UserManagementViewModel(IUserService userService)
        {
            _userService = userService;

            RefreshCommand = new RelayCommand(async () => await LoadUsersAsync());
            AddUserCommand = new RelayCommand(OpenAddDialog);
            EditUserCommand = new RelayCommand<User>(OpenEditDialog);
            SaveUserCommand = new RelayCommand(async () => await SaveAsync(), () => CanSave);

            CancelDialogCommand = new RelayCommand(() =>
            {
                IsDialogOpen = false;
                ResetDialogState();
            });

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

        #endregion

        #region ================= PASSWORD VALIDATION =================
        private Brush _passwordStrengthBrush = Brushes.Transparent;
        public Brush PasswordStrengthBrush
        {
            get => _passwordStrengthBrush;
            set
            {
                _passwordStrengthBrush = value;
                OnPropertyChanged();
            }
        }

        private void ValidatePasswords()
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordStrengthValue = 0;
                PasswordStrengthBrush = Brushes.Transparent;
                IsPasswordStrong = false;
                NotifyCanSaveChanged();
                return;
            }

            int score = 0;
            if (Password.Length >= 8) score += 25;
            if (Password.Any(char.IsUpper)) score += 25;
            if (Password.Any(char.IsLower)) score += 25;
            if (Password.Any(char.IsDigit)) score += 25;

            PasswordStrengthValue = score;

            if (score < 50)
            {
                PasswordStrengthBrush = Brushes.Red;
                IsPasswordStrong = false;
            }
            else if (score < 75)
            {
                PasswordStrengthBrush = Brushes.Orange;
                IsPasswordStrong = false;
            }
            else
            {
                PasswordStrengthBrush = Brushes.Green;
                IsPasswordStrong = true;
            }

            NotifyCanSaveChanged();
        }

        #endregion

        #region ================= DATA LOADING =================

        public async Task LoadOnStartupAsync()
        {
            await LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            Users.Clear();
            FilteredUsers.Clear();

            var users = await _userService.GetAllUsersAsync();
            foreach (var user in users)
                Users.Add(user);

            ApplyFilters();
        }

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

            foreach (var user in query
                     .Skip((CurrentPage - 1) * PageSize)
                     .Take(PageSize))
            {
                FilteredUsers.Add(user);
            }

            OnPropertyChanged(nameof(PageInfo));
            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
        }

        #endregion

        #region ================= ADD / EDIT =================

        private void OpenAddDialog()
        {
            DialogTitle = "Add User";
            IsAddMode = true;

            EditUser = new User { IsActive = true };
            IsDialogOpen = true;
        }

        private void OpenEditDialog(User user)
        {
            DialogTitle = "Edit User";
            IsAddMode = false;

            EditUser = new User
            {
                UserId = user.UserId,
                Username = user.Username,
                FullName = user.FullName,
                PasswordSalt= user.PasswordHash,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive,
                PasswordHash = user.PasswordHash,
                LastLoginOn = user.LastLoginOn,
                CreatedBy = user.CreatedBy

            };

            IsDialogOpen = true;
        }

        #endregion

        #region ================= SAVE =================

        private async Task SaveAsync()
        {
            try
            {
                IsSaving = true;

                if (IsAddMode)
                    await _userService.CreateUserAsync(EditUser, Password);
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

        private void ResetDialogState()
        {
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            ShowPassword = false;
            IsAddMode = false;
            EditUser = null;
        }

        #endregion

        #region ================= STATUS =================

        private async Task ToggleStatusAsync(User user)
        {
            user.IsActive = !user.IsActive;
            await _userService.UpdateUserAsync(user);
            SnackbarMessageQueue.Enqueue("User status updated");
        }

        #endregion
    }
}
