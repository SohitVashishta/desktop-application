using SchoolManagementSystem.Business.Services;
using SchoolManagementSystem.Common.Enums;
using SchoolManagementSystem.Models.Models;
using SchoolManagementSystem.UI.UI.Helpers;
using SchoolManagementSystem.UI.UI.Views.Admin;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SchoolManagementSystem.UI.UI.ViewModels.Admin
{
    public class UserManagementViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        // ================= DATA =================
        public ObservableCollection<User> Users { get; } = new();
        public ObservableCollection<User> FilteredUsers { get; } = new();

        // ================= SEARCH =================
        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
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
                OnPropertyChanged();
                ApplyFilters();
            }
        }

        // ================= COMMANDS =================
        public ICommand RefreshCommand { get; }
        public ICommand AddUserCommand { get; }

        // ================= CONSTRUCTOR =================
        public UserManagementViewModel(IUserService userService)
        {
            _userService = userService;

            RefreshCommand = new RelayCommand(async () => await LoadUsersAsync());
            AddUserCommand = new RelayCommand(OpenAddUserDialog);

           
        }

        // ================= LOAD =================
        private async Task LoadUsersAsync()
        {
            Users.Clear();
            FilteredUsers.Clear();

            var users = await _userService.GetAllUsersAsync();

            foreach (var u in users)
            {
                Users.Add(u);
                FilteredUsers.Add(u);
            }
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
            {
                query = query.Where(u => u.Role == SelectedRole.Value);
            }

            foreach (var u in query)
                FilteredUsers.Add(u);
        }

        // ================= ADD USER =================
        private void OpenAddUserDialog()
        {
            var vm = new AddUserViewModel();

            vm.UserSaved += async user =>
            {
                // ✅ SAVE (SYNC – matches your service)
              await  _userService.CreateUserAsync(user,user.PasswordHash);

                // ✅ REFRESH GRID
               await LoadOnStartupAsync();
            };

            var dialog = new AddUserDialog(vm)
            {
                Owner = Application.Current.MainWindow
            };

            dialog.ShowDialog();
        }

        
        public async Task LoadOnStartupAsync()
        {
            await LoadUsersAsync();
        }




    }
}
